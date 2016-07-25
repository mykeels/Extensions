using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Extensions.Models
{
    public class Desktop
    {
        public static Bitmap TakeScreenShot()
        {
            Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            Bitmap ret = new Bitmap(rect.Width, rect.Height);
            {
                Graphics g = Graphics.FromImage(ret);
                {
                    g.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, 0, 0, ret.Size, CopyPixelOperation.SourceCopy);
                }
                return ret;
            }
        }
    }
}
