using System;
using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    // http://msdn.microsoft.com/en-us/library/dd401657(VS.85).aspx
    
    [StructLayout(LayoutKind.Explicit)]
    public struct WINBIO_IDENTITY
    {
        [FieldOffset(0)]
        public WINBIO_IDENTITY_TYPE Type;
        [FieldOffset(4)]
        public uint Null;
        [FieldOffset(4)]
        public uint Wildcard;
        [FieldOffset(4)]
        public Guid TemplateGuid;
        [FieldOffset(4)]
        public _WINBIO_IDENTITY_SID AccountSid;
    }
}
