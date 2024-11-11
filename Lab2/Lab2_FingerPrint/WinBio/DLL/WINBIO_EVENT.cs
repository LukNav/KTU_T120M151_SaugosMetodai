using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct WINBIO_EVENT
    {
        [FieldOffset(0)]
        public WINBIO_EVENT_TYPE Type;
        [FieldOffset(4)]
        public _WINVIO_EVENT_UNCLAIMED Unclaimed;
        [FieldOffset(4)]
        public _WINBIO_EVENT_UNCLAIMED_IDENTIFY UnclaimedIdentity;
        [FieldOffset(4)]
        public _WINBIO_EVENT_ERROR Error;
    }
}
