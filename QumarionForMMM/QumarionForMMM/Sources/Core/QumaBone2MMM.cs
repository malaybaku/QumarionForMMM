using System.Linq;
using System.Collections.Generic;

using DxMath;
using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    /// <summary>QUMARION SDKのボーンを正規化処理するためのクラスです。</summary>
    public class QumaBone2MikuMikuMoving
    {
        //インスタンスに対応するQumarionのボーン
        private readonly Bone _bone;

        //親のボーン(ルート要素ではnull)
        private readonly QumaBone2MikuMikuMoving _parent;

        //子要素一覧(末端部では要素数0の配列)
        private readonly QumaBone2MikuMikuMoving[] _childs;

        //疑似的な座標軸として「MMMのXYZ軸に平行なローカル座標軸」を仮想的に割り当てるための3つの軸
        private readonly Vector3 xAxis;
        private readonly Vector3 yAxis;
        private readonly Vector3 zAxis;
        //上記メンバに関する補足: 仮想的な座標を挟む小技は以前モーションキャプチャ系を使ったときの流用。
        //参考: http://www.baku-dreameater.net/archives/5741
        //本クラスでは上記記事に書いてる変換の逆変換に相当する処理を行う

        /// <summary>Qumarionで割り当てられているボーンの種類を取得します。</summary>
        public StandardPSBones QumaBoneType { get; }

        /// <summary><see cref="QumaBoneType"/>に対応したMMDボーンの種類を取得します。</summary>
        public MMDStandardBone MMDBone { get; }

        /// <summary><see cref="MMDBone"/>が適切に割り当てられているかどうかを取得します。</summary>
        public bool IsValidBone { get; }

        /// <summary>このボーンを含む末端の子ボーンまでを列挙したものを再帰的に取得します。</summary>
        public IEnumerable<QumaBone2MikuMikuMoving> ChildBones
            => _childs.SelectMany(child => child.ChildBones)
                      .Concat(new QumaBone2MikuMikuMoving[] { this });

        /// <summary>初期状態(Tポーズ)での回転を取得します。</summary>
        public Quaternion InitialRotation { get; }

        /// <summary>Tポーズからの回転をMMDのXY(-Z)軸基準で表したものを取得します。</summary>
        public Quaternion Rotation { get; private set; }

        /// <summary>Qumarion側のボーン情報と親ボーンを指定してインスタンスを初期化します。</summary>
        /// <param name="bone">Qumarion側のボーン</param>
        /// <param name="boneType">Qumarion側のボーンが標準ボーンのどれに該当するか</param>
        /// <param name="parent">親ボーン(ルートのボーンを生成する場合nullを指定)</param>
        public QumaBone2MikuMikuMoving(Bone bone, StandardPSBones boneType, QumaBone2MikuMikuMoving parent)
        {
            _bone = bone;
            _parent = parent;

            QumaBoneType = boneType;
            //StandardPSBones -> MMDBoneの対応確認
            try
            {
                MMDBone = QumaBone2MMDBone.GetMMDBone(boneType);
                IsValidBone = true;
            }
            catch (KeyNotFoundException)
            {
                MMDBone = MMDStandardBone.Hip;
                IsValidBone = false;
            }

            InitialRotation = CreateQuaternion(_bone.InitialLocalMatrix);

            //ゼロ回転状態での固定座標軸を参照するため親のワールド座標を確認: ルート(Hips)はワールド座標直下。
            Matrix4f initMat = (_bone.Parent != null) ?
                _bone.Parent.InitialWorldMatrix :
                Matrix4f.Unit;

            //疑似座標系の初期化: QumarionもMMMも右手系(左-上-前)で一緒なのでほぼそのまま使ってOK
            xAxis = new Vector3(initMat.M11, initMat.M21, initMat.M31);
            yAxis = new Vector3(initMat.M12, initMat.M22, initMat.M32);
            zAxis = new Vector3(initMat.M13, initMat.M23, initMat.M33);

            //再帰的に子ボーンを初期化。
            _childs = bone
                .Childs
                .Select(b => new QumaBone2MikuMikuMoving(b, StandardPSBonesUtil.GetStandardPSBone(b.Name), this))
                .ToArray();
        }

        /// <summary>ボーンの姿勢を更新します。</summary>
        public void Update()
        {
            //子要素の更新
            foreach (var child in _childs)
            {
                child.Update();
            }

            //このボーン自体の姿勢更新
            var lmat = _bone.LocalMatrix;

            //ゼロ回転からではなくTポーズからの変化を知りたいので差分を求める
            var rotationMat = MatrixRotationDif.CreateDifFrom(lmat, _bone.InitialLocalMatrix);
            //クォータニオン表現に修正
            var rotation = CreateQuaternion(rotationMat);

            //回転軸を取得し、疑似座標系に投影して回転軸がXY(-Z)軸基準になるようにする
            Vector3 axis = rotation.Axis;
            float angle = rotation.Angle;

            Vector3 axisNormalized =
                axis.X * xAxis +
                axis.Y * yAxis +
                axis.Z * zAxis;

            Rotation = Quaternion.RotationAxis(axisNormalized, angle);
        }

        //Qumarionから得た姿勢行列(左-上-前の右手系)をMMMの回転表現(に近いもの)に修正
        private static Quaternion CreateQuaternion(Matrix4f lmat)
        {
            return MatrixToQuaternion.CreateFrom(lmat);
        }

    }
}
