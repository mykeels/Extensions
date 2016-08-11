using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace Extensions
{
    public static class RectangleExtensions
    {
        public static bool EqualsTo(this Rectangle rect1, Rectangle rect2)
        {
            if (rect1.X.Equals(rect2.X) & rect1.Y.Equals(rect2.Y) & rect1.Width.Equals(rect2.Width) & rect1.Height.Equals(rect2.Height))
            {
                return true;
            }
            return false;
        }
        
        public static Rectangle Overlap(this Rectangle rect1, Rectangle rect2)
        {
            Rectangle rightmost = default(Rectangle);
            if (rect1.X < rect2.X)
            {
                rightmost = rect2;
            }
            else
            {
                rightmost = rect1;
            }
            dynamic newx = Math.Min(rect1.X, rect2.X);
            dynamic newy = Math.Min(rect1.Y, rect2.Y);
            dynamic w = Math.Max(rect1.X, rect2.X) - newx + rightmost.Width;
            dynamic h = Math.Max(rect1.Y, rect2.Y) - newy + rightmost.Height;
            return new Rectangle(newx, newy, w, h);
        }
        
        public static Rectangle Inflate(this Rectangle rect, int size)
        {
            return new Rectangle(rect.X - size, rect.Y - size, rect.Width + size, rect.Height + size);
        }
        
        public static bool CloseTo(this Rectangle rect1, Rectangle rect2, int threshold)
        {
            int middle = threshold / 2;
            rect1 = rect1.Inflate(middle);
            rect2 = rect2.Inflate(middle);
            return rect1.IntersectsWith(rect2);
        }
        
        public static bool Contains(this IEnumerable<Rectangle> rects, Rectangle rect)
        {
            for (int i = 0; i <= rects.Count() - 1; i++)
            {
                if (rects.ElementAt(i).EqualsTo(rect))
                {
                    return true;
                }
            }
            return false;
        }
        
        public static IEnumerable<Rectangle> Distinct(this IEnumerable<Rectangle> rects)
        {
            List<Rectangle> ret = new List<Rectangle>();
            for (int i = 0; i <= rects.Count() - 1; i++)
            {
                if (!ret.Contains(rects.ElementAt(i)))
                {
                    ret.Add(rects.ElementAt(i));
                }
            }
            return ret.AsEnumerable();
        }
    }
}
