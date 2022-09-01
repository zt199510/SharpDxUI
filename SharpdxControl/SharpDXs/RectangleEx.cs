using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.SharpDXs
{
    public static class RectangleEx
    {

        public static Point Location(this Rectangle rectangle)
        {
            return new Point(rectangle.X, rectangle.Y);
        }

        public static Size Size(this Rectangle rectangle)
        {
            return new Size(rectangle.Width, rectangle.Height);
        }

        public static void Offset(this Rectangle rectangle, Point location)
        {
            rectangle.Offset(location.X, location.Y);
        }

        public static SharpDX.Rectangle ToRawRectangle(this System.Drawing.Rectangle rectangle)
        {
            return new SharpDX.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }


    }
}
