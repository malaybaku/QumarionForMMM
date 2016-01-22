using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    /// <summary>座標回転の差を求める処理を提供します。</summary>
    public static class MatrixRotationDif
    {

        /// <summary>
        /// 回転の最終結果と中間状態の回転行列を指定し、中間状態から最終状態への回転を求めます。
        /// </summary>
        /// <param name="result">最終的な回転行列。並進成分は無視されます。</param>
        /// <param name="transient">中間の回転行列。並進成分は無視されます。</param>
        /// <returns>中間状態から最終状態への回転を表す行列</returns>
        public static Matrix4f CreateDifFrom(Matrix4f result, Matrix4f transient)
        {
            //式としてはひじょーに簡単で、求める行列をX, 中間状態の行列をA, 最終状態の行列をBとするとき
            //X * A = B
            //が成り立てばよいので直ちに次の解を得る。
            //X = B * A^(-1)

            //ここでAは4x4直交行列であるから
            //A^(-1) = (Aの転置)
            //が成り立つので下記の実装でOK

            return result.Rotate.Multiply(transient.Rotate.Transpose());
        }

    }
}
