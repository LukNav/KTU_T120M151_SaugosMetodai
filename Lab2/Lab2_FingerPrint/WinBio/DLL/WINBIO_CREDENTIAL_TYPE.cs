namespace WinBioWrapper.DLL
{
    // http://msdn.microsoft.com/en-us/library/dd401654(VS.85).aspx
    public enum WINBIO_CREDENTIAL_TYPE : uint
    {
        WINBIO_CREDENTIAL_PASSWORD = 0x00000001,
        WINBIO_CREDENTIAL_ALL = 0xffffffff
    }
}
