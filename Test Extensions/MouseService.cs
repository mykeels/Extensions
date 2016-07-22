using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Extensions;
using Extensions.Models;
using WindowsInput;

namespace Test_Extensions
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MouseService" in both code and config file together.
    public class MouseService : IMouseService
    {
        public static Screen currentScreen { get; set; }
        public static string serviceUrl { get; set; }

        public static State state { get; set; }

        public enum State
        {
            Pending,
            Listening,
            Sending
        }

        public enum Screen
        {
            Primary,
            Secondary
        }

        public void StartListening()
        {
            if (state != State.Listening)
            {
                state = State.Listening;
                MouseCtrl.Stuff.StartListening();
            }
            Console.WriteLine("Listening Start: " + DateTime.Now.ToString());
        }

        public void StopListening()
        {
            state = State.Pending;
            Console.WriteLine("Listening Stop: " + DateTime.Now.ToString());
        }

        public Mouse.Win32.Point GetMousePosition()
        {
            if (state != State.Listening)
            {
                state = State.Listening;
                MouseCtrl.Stuff.testMouseCtrl();
            }
            return Mouse.GetCursorPosition();
        }

        public void MouseLeftDown()
        {
            Mouse.MouseEvent(Mouse.Event.LeftDown);
        }

        public void MouseLeftClick()
        {
            Mouse.MouseEvent((new List<Mouse.Event>()).Push(Mouse.Event.LeftDown).Push(Mouse.Event.LeftUp));
        }

        public void MouseLeftUp()
        {
            Mouse.MouseEvent(Mouse.Event.LeftUp);
        }

        public void MouseRightDown()
        {
            Mouse.MouseEvent(Mouse.Event.RightDown);
        }

        public void MouseRightClick()
        {
            Mouse.MouseEvent((new List<Mouse.Event>()).Push(Mouse.Event.RightDown).Push(Mouse.Event.RightUp));
        }

        public void MouseRightUp()
        {
            Mouse.MouseEvent(Mouse.Event.RightUp);
        }

        public string GetPCName()
        {
            return MouseCtrl.Stuff.GetPCName();
        }

        public void PushWhoIAm(string pcName)
        {
            if (String.IsNullOrEmpty(MouseCtrl.Stuff.otherPCName))
            {
                MouseCtrl.Stuff.otherPCName = pcName;
                if (MouseCtrl.Stuff.otherClient == null || MouseCtrl.Stuff.otherClient.State != CommunicationState.Opened)
                {
                    MouseCtrl.Stuff.otherClient = new MouseServiceReference.MouseServiceClient("BasicHttpBinding_IMouseService",
                    new EndpointAddress(new Uri(String.Format(MouseCtrl.Stuff.GetServiceUrl().Replace("localhost", "{0}").Replace("[::]", "{0}"), MouseCtrl.Stuff.otherPCName))));
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        public void MouseMiddleUp()
        {
            Mouse.MouseEvent(Mouse.Event.MiddleUp);
        }

        public void MouseMiddleDown()
        {
            Mouse.MouseEvent(Mouse.Event.MiddleDown);
        }

        public void MouseMiddleClick()
        {
            Mouse.MouseEvent((new List<Mouse.Event>()).Push(Mouse.Event.MiddleDown).Push(Mouse.Event.MiddleUp));
        }

        public void MouseWheel(int delta)
        {
            Mouse.MouseEvent(Mouse.Event.Wheel, (uint)delta);
        }

        public void KeyUp(System.Windows.Forms.Keys key, bool ctrl = false, bool win = false, bool alt = false, bool shift = false)
        {
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();
            if (ctrl) modifiers.Add(VirtualKeyCode.CONTROL);
            if (alt) modifiers.Add(VirtualKeyCode.MENU);
            if (shift) modifiers.Add(VirtualKeyCode.SHIFT);
            if (win) modifiers.Add(VirtualKeyCode.LWIN);
            if (modifiers.IsEmpty()) InputSimulator.SimulateKeyPress((VirtualKeyCode)(int)key);
            else
            {
                if (ctrl && alt && key == System.Windows.Forms.Keys.S)
                {
                    if (currentScreen == Screen.Primary) currentScreen = Screen.Secondary;
                    else currentScreen = Screen.Primary;
                }
                else
                { //send modified keystrokes to windows
                    InputSimulator.SimulateModifiedKeyStroke(modifiers.AsEnumerable(), (VirtualKeyCode)(int)key);
                }
            }
        }
    }
}
