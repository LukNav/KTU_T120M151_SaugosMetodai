using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _WINVIO_EVENT_UNCLAIMED
    {
        public uint UnitId;
        public WINBIO_REJECT_DETAIL RejectDetail;
    }
}
