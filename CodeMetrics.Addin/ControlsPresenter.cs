using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.OLE.Interop;

namespace CodeMetrics.Addin
{
    public class ControlsPresenter
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public void ShowControl(UserControl control, IntPtr parentHandle, POINT methodLocation)
        {
            SetLocation(control, methodLocation);

            SetParent(control.Handle, parentHandle);
            ShowWindow(control.Handle, 8);
        }

        private static void SetLocation(UserControl control, POINT methodLocation)
        {
            control.Left = methodLocation.x;
            control.Top = methodLocation.y;
        }
    }
}