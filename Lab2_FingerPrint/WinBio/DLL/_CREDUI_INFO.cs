using System;
using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _CREDUI_INFO
    {
        public int cbSize;
        public IntPtr hwndParent;
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 255)]
        public string pszMessageText;
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 255)]
        public string pszCaptionText;
        public IntPtr hbmBanner;
    }
}
