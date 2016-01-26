using DxMath;
using System;

namespace QumarionForMMM
{
    static class QuaternionDecomposition
    {
        /// <summary>英語Wikipediaに載ってる標準解法ベースでオイラー角への分解を行います。</summary>
        public static void DecompositeStandardEuler(Quaternion q, out Quaternion roll, out Quaternion nonRoll)
        {
            //NOTE: 大事なこととして普通の学術計算のやつと座標軸違うので直す
            //「前、右、下」を順に割り当てる: 航空機力学の座標系に合わす為
            float qx = q.X;
            float qy = -q.Z;
            float qz = -q.Y;
            //コレは回転方向の修正
            float qw = -q.W;

            //標準解法: ただし右手系向き計算なので回転向き直す
            float phi = -(float)Math.Atan2(2.0f * (qw * qx + qy * qz), 1 - 2.0f * (qx * qx + qy * qy));
            float theta = -(float)Math.Asin(2.0f * (qw * qy - qx * qz));
            float psi = -(float)Math.Atan2(2.0f * (qw * qz + qx * qy), 1 - 2.0f * (qy * qy + qz * qz));

            //yaw = new Quaternion(0, (float)Math.Sin(0.5 * psi), 0, (float)Math.Cos(0.5 * psi));
            //pitch = new Quaternion(0, 0, (float)Math.Sin(0.5 * theta), (float)Math.Cos(0.5 * theta));
            roll = new Quaternion((float)Math.Sin(0.5 * phi), 0, 0, (float)Math.Cos(0.5 * phi));

            //nonRollについては回転結果が揃うように決める: なんのミスだかわかんないけどうまく分解できてないっぽいので…
            //nonRoll * roll = q をnonRollについて解いてるだけ。
            //nonRoll = Quaternion.Multiply(q, Quaternion.Conjugate(roll));
            nonRoll = Quaternion.Multiply(Quaternion.Conjugate(roll), q);
        }
    }
}
