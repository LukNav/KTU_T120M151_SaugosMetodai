using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINBIO_BIR_DATA
    {
        public uint Size;
        public uint Offset;
    }
}
