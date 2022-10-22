using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.SharpDXs
{
    public static class ColorEx
    {
        public static RawColorBGRA ToRawColorBGRA(this System.Drawing.Color color)
        {
            return new RawColorBGRA(color.B, color.G, color.R, color.A);
        }

        public static RawColor4 ToRawColor4(this System.Drawing.Color color)
        {
            return new RawColor4((int)color.R, (int)color.G, (int)color.B, (int)color.A);
        }

        public static System.Drawing.Color ToColor(this Color4 color)
        {
            return System.Drawing.Color.FromArgb(Convert.ToInt32(color.Alpha * 255f), Convert.ToInt32(color.Red * 255f), Convert.ToInt32(color.Green * 255f), Convert.ToInt32(color.Blue * 255f));
        }


    }
}
