using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CAPTURE_CALLBACK_CONTEXT
    {
        public WINBIO_BIR_PURPOSE Purpose;
        public WINBIO_BIR_DATA_FLAGS Flags;
    }
}
