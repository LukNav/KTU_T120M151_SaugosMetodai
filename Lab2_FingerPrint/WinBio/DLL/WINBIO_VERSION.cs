using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINBIO_VERSION
    {
        public int MajorVersion;
        public int MinorVersion;
        public override string ToString()
        {
            return string.Format("{0}.{1}", MajorVersion, MinorVersion);
        }
    }
}
