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

                if(_armRollPair.Keys.Contains(mmdBone))
                {
                    var rollBone = MMDStandardBones.GetBone(model, _armRollPair[mmdBone]);

                    //追加ロジック: 回転をもとのボーンと「捩」ボーンに分割する
                    Quaternion roll, nonRoll;
                    QuaternionDecomposition.DecompositeStandardEuler(src.Rotation, out roll, out nonRoll);

                    nonRoll = GetRotation(nonRoll, mmdBone);
                    roll = GetRotation(roll, mmdBone);

                    //本体側
                    var motion = dest.CurrentLocalMotion;
                    motion.Rotation = nonRoll;
                    dest.CurrentLocalMotion = motion;

                    //追加: 対応する「捩」ボーン
                    var rollMotion = rollBone.CurrentLocalMotion;
                    rollMotion.Rotation = roll;
                    rollBone.CurrentLocalMotion = rollMotion;

                }
                else
                {
                    //普通のボーン

                    //Qumarion側の正規化回転をMMDモデルのローカル回転に直す
                    var rotation = GetRotation(src.Rotation, mmdBone);

                    var motion = dest.CurrentLocalMotion;
                    motion.Rotation = rotation;
                    dest.CurrentLocalMotion = motion;
                }

            }
        }

        private void ApplyIK(Model model)
        {
            //追加: 加速度センサONの場合に回転不変性取るよう改善
            var boneHip = QumarionModel.Bones[StandardPSBones.Hips];
            var initPosHip = boneHip.InitialWorldMatrix.Translate;
            var posHip = boneHip.WorldMatrix.Translate;
            //NOTE: 
            var rotHipInverse = MatrixRotationDif.CreateDifFrom(
                boneHip.WorldMatrix.Rotate,
                boneHip.InitialWorldMatrix.Rotate
                ).Transpose();


            foreach (var t in _targetIKBones)
            {
                var src = QumarionModel.Bones[QumaBone2MMDBone.GetQumaBone(t)];
                var dest = MMDStandardBones.GetBone(model, t);
                if (dest == null) continue;

                //1: Tポーズでの Hip -> t のベクトル取得(これは直立)
                var initPosWorld = src.InitialWorldMatrix.Translate;
                var initPos = new Vector3f(
                    initPosWorld.X - initPosHip.X,
                    initPosWorld.Y - initPosHip.Y,
                    initPosWorld.Z - initPosHip.Z
                    );
                //2: 現在の Hip -> t のベクトルを回転除去しつつ取得

                var posWorld = src.WorldMatrix.Translate;
                var pos = new Vector3f(
                    posWorld.X - posHip.X,
                    posWorld.Y - posHip.Y,
                    posWorld.Z - posHip.Z
                    );
                //ここ大事: posのベクトルは体の回転込みの値なので逆回転して相殺
                pos = MatrixUtil.Multiply(rotHipInverse, pos);

                //NOTE: MMMではZ軸が後ろ向き正だがQumarionだと前向き正
                var dif = new Vector3(
                    pos.X - initPos.X,
                    pos.Y - initPos.Y,
                    -pos.Z + initPos.Z
                    );

                var motion = dest.CurrentLocalMotion;
                motion.Move = dif * LegIKScaleFactor;
                dest.CurrentLocalMotion = motion;
            }
        }

        //モデルを接地させる(接地の定義は実装を見よ。)
        private void BoundCenterToGround(Model model)
        {
            //追加: 加速度センサONの場合に回転不変性取るよう改善
            var boneHip = QumarionModel.Bones[StandardPSBones.Hips];
            var initPosHip = boneHip.InitialWorldMatrix.Translate;
            var posHip = boneHip.WorldMatrix.Translate;
            //NOTE: 
            var rotHipInverse = MatrixRotationDif.CreateDifFrom(
                boneHip.WorldMatrix.Rotate,
                boneHip.InitialWorldMatrix.Rotate
                ).Transpose();


            var heights = new List<float>();

            foreach (var t in _groundingIKBones)
            {

                var src = QumarionModel.Bones[QumaBone2MMDBone.GetQumaBone(t)];
                var dest = MMDStandardBones.GetBone(model, t);
                if (dest == null) continue;

                //1: Tポーズでの Hip -> t のベクトル取得(これは直立)
                var initPosWorld = src.InitialWorldMatrix.Translate;
                var initPos = new Vector3f(
                    initPosWorld.X - initPosHip.X,
                    initPosWorld.Y - initPosHip.Y,
                    initPosWorld.Z - initPosHip.Z
                    );
                //2: 現在の Hip -> t のベクトルを回転除去しつつ取得

                var posWorld = src.WorldMatrix.Translate;
                var pos = new Vector3f(
                    posWorld.X - posHip.X,
                    posWorld.Y - posHip.Y,
                    posWorld.Z - posHip.Z
                    );
                //ここ大事: posのベクトルは体の回転込みの値なので逆回転して相殺
                pos = MatrixUtil.Multiply(rotHipInverse, pos);

                //NOTE: MMMではZ軸が後ろ向き正だがQumarionだと前向き正
                var dif = new Vector3(
                    pos.X - initPos.X,
                    pos.Y - initPos.Y,
                    -pos.Z + initPos.Z
                    );

                heights.Add(dif.Y * LegIKScaleFactor);
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

        private float SinArm => (float)Math.Sin(ArmAngleRad);
        private float CosArm => (float)Math.Cos(ArmAngleRad);

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
            MMDStandardBone.RightArmRoll,
            MMDStandardBone.RightElbow,
            MMDStandardBone.RightHand
        };

        private readonly MMDStandardBone[] _leftArmBones = new[]
        {
            MMDStandardBone.LeftUpperArm,
            MMDStandardBone.LeftArmRoll,
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

        //追加: X軸(をArmAngleの分だけ傾けた軸)の回転つまり「ねじり」を無効化して他ボーンに押し付けるペア
        //例: 「左腕」のねじり回転は「左腕捩」に押しつける。これによって肩の脱臼を防ぐ効果があるハズ
        private readonly Dictionary<MMDStandardBone, MMDStandardBone> _armRollPair = new Dictionary<MMDStandardBone, MMDStandardBone>()
        {
            { MMDStandardBone.LeftUpperArm, MMDStandardBone.LeftArmRoll },
            { MMDStandardBone.RightUpperArm, MMDStandardBone.RightArmRoll }
        };

        #endregion
    }

}
