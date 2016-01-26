using Baku.Quma.Pdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QumarionForMMM
{
    static class MatrixUtil
    {
        public static Vector3f Multiply(Matrix4f m, Vector3f v)
            => new Vector3f(
                m.M11* v.X + m.M12* v.Y + m.M13* v.Z,
                m.M21* v.X + m.M22* v.Y + m.M23* v.Z,
                m.M31* v.X + m.M32* v.Y + m.M33* v.Z
                );
    }
}
