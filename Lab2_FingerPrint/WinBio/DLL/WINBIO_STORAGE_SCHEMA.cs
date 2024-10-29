using System;
using System.Runtime.InteropServices;

namespace WinBioWrapper.DLL
{
    // http://msdn.microsoft.com/en-us/library/dd401663(VS.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    public struct WINBIO_STORAGE_SCHEMA
    {
        public WINBIO_BIOMETRIC_TYPE BiometricFactor;
        public Guid DatabaseId;
        public Guid DataFormat;
        public WINBIO_DATABASE Attributes;
        public WINBIO_STRING FilePath;
        public WINBIO_STRING ConnectionString;
    }
}
