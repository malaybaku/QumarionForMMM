using System;
using System.Collections.Generic;
using System.Linq;

using DxMath;
using MikuMikuPlugin;
using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    /// <summary>モデル操作の主処理を表します。</summary>
    class ModelHandler : IDisposable
    {
        public ModelHandler()
        {
            QumarionModel = PdkManager.CreateStandardModelPS();
            _rootBone = new QumaBone2MikuMikuMoving(QumarionModel.Root, StandardPSBones.Hips, null);

            TryAttachQumarionToModel();            
        }

        /// <summary>Qumarion側のモデルを取得します。デバイスとしてのQumarionにもここからアクセスしてください。</summary>
        public StandardCharacterModel QumarionModel { get; }

        /// <summary>足IKの補正計算に使うスケール補正値</summary>
        public float LegIKScaleFactor { get; set; }
        
        /// <summary>腕がTポーズより斜め下になっている角度を弧度法で示した補正値</summary>
        public float ArmAngleRad { get; set; }

        /// <summary>足を常時接地させるかどうか</summary>
        public bool BindFootToGround { get; set; }

        /// <summary>
        /// PCに接続されたQumarionがあればモデルに関連づけます。
        /// 複数のQumarionがある場合の選択は自動で行われます。</summary>
        /// <returns>接続成功または既に接続済みだった場合<see cref="true"/></returns>
        public bool TryAttachQumarionToModel()
        {
            if(QumarionModel.AttachedQumarion != null)
            {
                return true;
            }

            if(PdkManager.ConnectedDeviceCount == 0)
            {
                return false;
            }

            QumarionModel.AttachQumarion(PdkManager.GetDefaultQumarion());
            return true;
        }


        /// <summary>Qumarionの情報を更新し、適用先のモデルに角度を割り当てます。</summary>
        /// <param name="model">適用先のモデル</param>
        public void Update(Model model)
        {
            if(QumarionModel.AttachedQumarion == null) return;

            //Qumarion側
            QumarionModel.Update();
            _rootBone.Update();

            //MMD側への適用: シンプルに。
            ApplyFK(model);
            ApplyIK(model);
            if (BindFootToGround) BoundCenterToGround(model);
        }       

        public void Dispose()
        {
            if (QumarionModel.AttachedQumarion != null)
            {
                QumarionModel.DetachQumarion();
            }
        }

        private void ApplyFK(Model model)
        {
            var childBonesQumarion = _rootBone.ChildBones;
            foreach(var mmdBone in _targetBones)
            {
                var src = childBonesQumarion.FirstOrDefault(
                    bone => bone.IsValidBone && bone.MMDBone == mmdBone
                    );
                var dest = MMDStandardBones.GetBone(model, mmdBone);

                if (src == null || dest == null) continue;

                //Qumarion側の正規化回転をMMDモデルのローカル回転に直す
                var rotation = GetRotation(src.Rotation, mmdBone);

                //初期回転との合成として取得: 
                var motion = dest.CurrentLocalMotion;
                //motion.Rotation = _modelInitialInfo[target].InitialRotation * rotation;
                motion.Rotation = rotation;
                dest.CurrentLocalMotion = motion;

            }
        }

        private void ApplyIK(Model model)
        {
            foreach (var t in _targetIKBones)
            {
                var src = QumarionModel.Bones[QumaBone2MMDBone.GetQumaBone(t)];
                var dest = MMDStandardBones.GetBone(model, t);
                if (dest == null) continue;

                var initpos = src.InitialWorldMatrix.Translate;
                var currentpos = src.WorldMatrix.Translate;
                //NOTE: MMMではZ軸が後ろ向き正だがQumarionだと前向き正
                var dif = new Vector3(
                    currentpos.X - initpos.X,
                    currentpos.Y - initpos.Y,
                    -currentpos.Z + initpos.Z
                    );

                var motion = dest.CurrentLocalMotion;
                motion.Move = dif * LegIKScaleFactor;
                dest.CurrentLocalMotion = motion;
            }
        }

        //モデルを接地させる(接地の定義は実装を見よ。)
        private void BoundCenterToGround(Model model)
        {
            var heights = new List<float>();

            foreach (var bone in _groundingIKBones)
            {
                var src = QumarionModel.Bones[QumaBone2MMDBone.GetQumaBone(bone)];
                var dest = MMDStandardBones.GetBone(model, bone);
                if (src == null || dest == null) continue;

                var initpos = src.InitialWorldMatrix.Translate;
                var currentpos = src.WorldMatrix.Translate;
                heights.Add(currentpos.Y - initpos.Y);
            }

            if (heights.Count == 0) return;

            //移動差分のうちもっとも低い値を取る
            float lowestPosDif = heights.Min();
            var centerBone = MMDStandardBones.GetBone(model, MMDStandardBone.Center);
            if (centerBone == null) return;

            var motion = centerBone.CurrentLocalMotion;
            //CAUTION: X, Zを勝手に書き換えないように！
            motion.Move = new Vector3(motion.Move.X, -lowestPosDif, motion.Move.Z);

            centerBone.CurrentLocalMotion = motion;



        }


        //Qumarion側で正規化した回転表現 -> MMD座標の回転への変換
        //1. MMMの回転指定法を見た所によると基本Z軸の正負だけ注意すればいい
        //2. 左右の腕については一般にMMDモデルだとTポーズからズレてるので補正する
        private Quaternion GetRotation(Quaternion q, MMDStandardBone bone)
            => _rightArmBones.Contains(bone) ? 
                   new Quaternion((q.X * CosArm - q.Y * SinArm), (q.X * SinArm + q.Y * CosArm), -q.Z, q.W) :
               _leftArmBones.Contains(bone) ?
                   new Quaternion((q.X * CosArm + q.Y * SinArm), (-q.X * SinArm + q.Y * CosArm), -q.Z, q.W) : 
                   new Quaternion(q.X, q.Y, -q.Z, q.W);

        //Qumarion -> 正規化済みの人側モデルまでの処理担当
        private readonly QumaBone2MikuMikuMoving _rootBone;

        private float SinArm => (float)(Math.Sin(ArmAngleRad * Math.PI / 180.0f));
        private float CosArm => (float)(Math.Cos(ArmAngleRad * Math.PI / 180.0f));




        #region 諸関数が対象とするボーンの一覧

        //QUMARIONから　ForwardKinematicでボーン反映したいMMD側のボーン一覧。

        //TODO: 足がまったく入ってないので追加する必要あり
        //TODO: 肩から先の動きについて、Qumaボーンを生で使うと以下の問題があることに注意
        //  1. Qumaボーンでは上腕が3自由度で動いて肩が動かないが、実際は腕を上げるときは
        //     肩の根元から引き揚げないと不自然
        //  2. 肩のロール方向や手首のロール方向運動は「捩」ボーンで解決させないと骨折に繋がる
        private readonly MMDStandardBone[] _targetBones = new[]
        {
            #region 適用先ボーン一覧
            MMDStandardBone.Hip,
            MMDStandardBone.Spine,
            MMDStandardBone.Spine2,
            MMDStandardBone.Neck,
            MMDStandardBone.Head,

            MMDStandardBone.LeftUpperArm,
            MMDStandardBone.LeftElbow,
            MMDStandardBone.LeftHand,

            MMDStandardBone.RightUpperArm,
            MMDStandardBone.RightElbow,
            MMDStandardBone.RightHand,

            MMDStandardBone.LeftLeg,
            MMDStandardBone.LeftKnee,
            MMDStandardBone.LeftFoot,
            MMDStandardBone.LeftToe,

            MMDStandardBone.RightLeg,
            MMDStandardBone.RightKnee,
            MMDStandardBone.RightFoot,
            MMDStandardBone.RightToe
            #endregion
        };

        //NOTE: 肩はQumarionの人ボーンだとうまく取れないので含めないように。左腕側も同様
        private readonly MMDStandardBone[] _rightArmBones = new[]
        {
            MMDStandardBone.RightUpperArm,
            MMDStandardBone.RightElbow,
            MMDStandardBone.RightHand
        };

        private readonly MMDStandardBone[] _leftArmBones = new[]
        {
            MMDStandardBone.LeftUpperArm,
            MMDStandardBone.LeftElbow,
            MMDStandardBone.LeftHand
        };

        private readonly MMDStandardBone[] _targetIKBones = new[]
        {
            MMDStandardBone.LeftFootIK,
            MMDStandardBone.RightFootIK
        };

        private readonly MMDStandardBone[] _groundingIKBones = new[]
        {
            MMDStandardBone.LeftFootIK,
            MMDStandardBone.RightFootIK
        };

        #endregion
    }

}
