using System;
using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    internal struct _TOKEN_USER
    {
        public _SID_AND_ATTRIBUTES User;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _SID_AND_ATTRIBUTES
    {

        public IntPtr Sid;
        public int Attributes;
    }
}
