using System;
using System.Linq;

using DxMath;
//using UnityEngine;

namespace QumarionForMMM
{
    /// <summary>回転行列をクオータニオンに変換する処理を提供します。</summary>
    public static class MatrixToQuaternion
    {
        //計算の参考元:  http://marupeke296.com/DXG_No58_RotQuaternionTrans.html
        /// <summary>
        /// QumarionDotNetの行列を回転行列とみなし、等価な回転を表すクオータニオンを生成します。
        /// </summary>
        /// <param name="mat">回転を表す行列</param>
        /// <returns>回転を表すクオータニオン</returns>
        public static Quaternion CreateFrom(Baku.Quma.Pdk.Matrix4f mat)
        {
            float wAbs = 0.5f * (float)(Math.Sqrt(mat[0, 0] + mat[1, 1] + mat[2, 2] + 1));
            float xAbs = 0.5f * (float)(Math.Sqrt(mat[0, 0] - mat[1, 1] - mat[2, 2] + 1));
            float yAbs = 0.5f * (float)(Math.Sqrt(-mat[0, 0] + mat[1, 1] - mat[2, 2] + 1));
            float zAbs = 0.5f * (float)(Math.Sqrt(-mat[0, 0] - mat[1, 1] + mat[2, 2] + 1));

            //NOTE: Math.Sqrtは負値を食うとNaNを吐くはずなので保険
            if (float.IsNaN(wAbs)) wAbs = 0.0f;
            if (float.IsNaN(xAbs)) xAbs = 0.0f;
            if (float.IsNaN(yAbs)) yAbs = 0.0f;
            if (float.IsNaN(zAbs)) zAbs = 0.0f;

            //ホントは必要な3個だけ求めればいいが可読性優先
            float prodXY = 0.25f * (mat[0, 1] + mat[1, 0]);
            float prodWZ = 0.25f * (mat[0, 1] - mat[1, 0]);
            float prodXZ = 0.25f * (mat[2, 0] + mat[0, 2]);
            float prodWY = 0.25f * (mat[2, 0] - mat[0, 2]);
            float prodYZ = 0.25f * (mat[1, 2] + mat[2, 1]);
            float prodWX = 0.25f * (mat[1, 2] - mat[2, 1]);

            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;
            float w = 0.0f;

            //いちばん絶対値がデカい成分をピボットにして他を算出する: これも参考元にある計算から
            float absMax = new float[] { wAbs, xAbs, yAbs, zAbs }.Max();

            if (absMax == wAbs)
            {
                w = wAbs;
                x = prodWX / w;
                y = prodWY / w;
                z = prodWZ / w;
            }
            else if (absMax == xAbs)
            {
                x = xAbs;
                w = prodWX / x;
                y = prodXY / x;
                z = prodXZ / x;
            }
            else if (absMax == yAbs)
            {
                y = yAbs;
                w = prodWY / y;
                x = prodXY / y;
                z = prodYZ / y;
            }
            else if (absMax == zAbs)
            {
                z = zAbs;
                w = prodWZ / z;
                x = prodXZ / z;
                y = prodYZ / z;
            }

            return new Quaternion(x, y, z, w);

        }
    }

}
