using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.SharpDXs
{
    public static class SpriteEx
    {
      

        public static void Draw(this Sprite sprite, Texture texture, Vector3 center, Vector3 postion, System.Drawing.Color color)
        {
            sprite.Draw(texture, color.ToRawColorBGRA(), null, center.ToRawVector3(), postion.ToRawVector3());
        }

        public static void Draw(this Sprite sprite, Texture texture, System.Drawing.Rectangle rectangle, Vector3 center, Vector3 postion, System.Drawing.Color color)
        {
            sprite.Draw(texture, color.ToRawColorBGRA(), rectangle.ToRawRectangle(), center.ToRawVector3(), postion.ToRawVector3());
        }


        internal static void WftWE6pZgA0j05i3Ew()
        {
        }
    }
}
