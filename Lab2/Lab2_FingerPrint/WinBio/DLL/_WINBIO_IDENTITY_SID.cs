using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _WINBIO_IDENTITY_SID
    {
        const int SECURITY_MAX_SID_SIZE = 68;
        public uint Size;
        public fixed byte Data[SECURITY_MAX_SID_SIZE];
    }
}
