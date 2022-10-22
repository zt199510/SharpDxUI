using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.SharpDXs
{
    public static class Vector3Ex
    {
        public static RawVector3 ToRawVector3(this Vector3 vector)
        {
            return new RawVector3(vector.X, vector.Y, vector.Z);
        }
    }
}
