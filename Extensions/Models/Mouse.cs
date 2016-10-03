using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Models
{
    public class Mouse
    {
        public static Win32.Point currentPosition { get; set; }

        public static Win32.Point SetCursorPosition(int x, int y)
        {   
            Win32.Point p = new Win32.Point();
            p.x = x;
            p.y = y;
            Mouse.Win32.SetCursorPos(x, y);
            currentPosition = p;
            return p;
        }

        public static Win32.Point GetCursorPosition()
        {
            currentPosition = Win32.GetCursorPosition();
            return currentPosition;
        }

        public static void MouseEvent(Event ev, uint cButtons = 0)
        {
            Win32.mouse_event((uint)ev, (uint)currentPosition.x, (uint)currentPosition.y, cButtons, 0);
        }

        public static void MouseEvent(List<Event> events)
        {
            events.ForEach((ev) =>
            {
                Win32.mouse_event((uint)ev, (uint)currentPosition.x, (uint)currentPosition.y, 0, 0);
                System.Threading.Thread.Sleep(150);
            });
        }

        public enum Event
        {
            LeftDown = 0x02,
            LeftUp = 0x04,
            RightDown = 0x08,
            RightUp = 0x10,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Wheel = 0x800
        }

        public class Win32
        {
            [DllImport("User32.Dll")]
            public static extern long SetCursorPos(int x, int y);

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out Point lpPoint);

            [DllImport("User32.Dll")]
            public static extern bool ClientToScreen(IntPtr hWnd, ref Point point);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

            [StructLayout(LayoutKind.Sequential)]
            public struct Point
            {
                public int x;
                public int y;
            }

            public static Point GetCursorPosition()
            {
                Point lpPoint;
                GetCursorPos(out lpPoint);
                return lpPoint;
            }
        }
    }
}
