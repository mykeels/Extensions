using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Extensions;
using Extensions.Models;

namespace Test_Extensions
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMouseService" in both code and config file together.
    [ServiceContract]
    public interface IMouseService
    {
        [OperationContract]
        void StartListening();

        [OperationContract]
        void StopListening();

        [OperationContract]
        Mouse.Win32.Point GetMousePosition();

        [OperationContract]
        void MouseLeftDown();

        [OperationContract]
        void MouseLeftUp();

        [OperationContract]
        void MouseLeftClick();

        [OperationContract]
        void MouseRightDown();

        [OperationContract]
        void MouseRightUp();

        [OperationContract]
        void MouseRightClick();

        [OperationContract]
        void MouseMiddleUp();

        [OperationContract]
        void MouseMiddleDown();

        [OperationContract]
        void MouseMiddleClick();

        [OperationContract]
        void MouseWheel(int delta);

        [OperationContract]
        void KeyUp(System.Windows.Forms.Keys key, bool ctrl = false, bool win = false, bool alt = false, bool shift = false);

        [OperationContract]
        string GetPCName();

        [OperationContract]
        void PushWhoIAm(string pcName);
    }
}
