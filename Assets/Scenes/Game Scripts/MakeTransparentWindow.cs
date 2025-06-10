using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

    private void Start()
    {
#if !UNITY_EDITOR
        Margins margins = new Margins() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(GetActiveWindow(), ref margins);
#endif
    }
}
