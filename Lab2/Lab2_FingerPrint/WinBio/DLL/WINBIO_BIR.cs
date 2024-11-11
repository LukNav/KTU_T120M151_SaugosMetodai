using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINBIO_BIR
    {
        public WINBIO_BIR_DATA HeaderBlock;
        public WINBIO_BIR_DATA StandardDataBlock;
        public WINBIO_BIR_DATA VendorDataBlock;
        public WINBIO_BIR_DATA SigntureBlock;
    }
}
