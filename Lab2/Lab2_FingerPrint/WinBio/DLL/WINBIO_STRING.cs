﻿using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WINBIO_STRING
    {
        const int WINBIO_MAX_STRING_LEN = 256;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WINBIO_MAX_STRING_LEN)]
        public string Value;
    }
}
