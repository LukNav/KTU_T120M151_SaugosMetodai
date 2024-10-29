using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _WINBIO_EVENT_ERROR
    {
        public int ErrorCode;
    }
}
