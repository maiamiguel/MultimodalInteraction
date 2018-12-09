// Source https://social.msdn.microsoft.com/Forums/en-US/877f75fa-d74a-4173-a44d-62c3cf1d3baf/can-you-implement-kinect-mouse-concept?forum=kinectsdk#2ccab202-72a2-4b8a-b781-887db6e52a1e
// Author rcitaliano

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public class MouseHook
    {
        //used for sending the mouse events like
        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
        //constants for the mouse events
        //found at http://msdn.microsoft.com/en-us/library/ms646273(v=vs.85).aspx
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;  //The left button was pressed
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;  //The left button was released.

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
        //Virtual key codes
        //found at http://msdn.microsoft.com/en-us/library/dd375731(v=VS.85).aspx
        private const UInt16 VK_LBUTTON = 0x01; //left mouse button

        /// <summary>
        /// Returns negative when the button is DOWN and 0 when the button is UP
        /// </summary>
        /// <returns></returns>
        private static short GetLeftButtonState()
        {
            return GetAsyncKeyState(VK_LBUTTON);
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        public static bool IsLeftButtonUp
        {
            get
            {
                return (MouseHook.GetLeftButtonState() == 0);
            }
        }

        /// <summary>
        /// Sends the Down and Up Events for the Left Mouse Button
        /// </summary>
        public static void SendClick()
        {
            SendDown();
            SendUp();
        }

        /// <summary>
        /// Clicks as a normal user does,
        /// if the mouse button is up the button will be pressed
        /// if the mouse button is down it will be released
        /// </summary>
        /*public static void SendClickUser()
        {
            if (IsLeftButtonUp)
                SendDown();
            else
                SendUp();
        }*/

        /// <summary>
        /// Send the event Up for the Left Mouse Button
        /// </summary>
        public static void SendUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        /// <summary>
        /// Send the event Down for the Left Mouse Button
        /// </summary>
        public static void SendDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
        }
    }
}
