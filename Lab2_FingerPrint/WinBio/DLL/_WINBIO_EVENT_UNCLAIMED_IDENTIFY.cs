using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _WINBIO_EVENT_UNCLAIMED_IDENTIFY
    {
        public uint UnitId;
        public WINBIO_IDENTITY Identity;
        public WINBIO_BIOMETRIC_SUBTYPE SubFactor;
        public WINBIO_REJECT_DETAIL RejectDetail;
    }
}
