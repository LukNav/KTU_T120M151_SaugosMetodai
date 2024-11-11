﻿namespace WinBioWrapper.DLL
{
    public enum WINBIO_ERRORS :  uint
    {
        E_ACCESSDENIED = 0x80070005,
        E_HANDLE = 0x80070006,
        WINBIO_E_UNSUPPORTED_FACTOR = 0x80098001,
        WINBIO_E_INVALID_UNIT = 0x80098002,
        WINBIO_E_UNKNOWN_ID = 0x80098003,
        WINBIO_E_CANCELED = 0x80098004,
        WINBIO_E_NO_MATCH = 0x80098005,
        WINBIO_E_CAPTURE_ABORTED = 0x80098006,
        WINBIO_E_ENROLLMENT_IN_PROGRESS = 0x80098007,
        WINBIO_E_BAD_CAPTURE = 0x80098008,
        WINBIO_E_INVALID_CONTROL_CODE = 0x80098009,
        WINBIO_E_DATA_COLLECTION_IN_PROGRESS = 0x8009800B,
        WINBIO_E_UNSUPPORTED_DATA_FORMAT = 0x8009800C,
        WINBIO_E_UNSUPPORTED_DATA_TYPE = 0x8009800D,
        WINBIO_E_UNSUPPORTED_PURPOSE = 0x8009800E,
        WINBIO_E_INVALID_DEVICE_STATE = 0x8009800F,
        WINBIO_E_DEVICE_BUSY = 0x80098010,
        WINBIO_E_DATABASE_CANT_CREATE = 0x80098011,
        WINBIO_E_DATABASE_CANT_OPEN = 0x80098012,
        WINBIO_E_DATABASE_CANT_CLOSE = 0x80098013,
        WINBIO_E_DATABASE_CANT_ERASE = 0x80098014,
        WINBIO_E_DATABASE_CANT_FIND = 0x80098015,
        WINBIO_E_DATABASE_ALREADY_EXISTS = 0x80098016,
        WINBIO_E_DATABASE_FULL = 0x80098018,
        WINBIO_E_DATABASE_LOCKED = 0x80098019,
        WINBIO_E_DATABASE_CORRUPTED = 0x8009801A,
        WINBIO_E_DATABASE_NO_SUCH_RECORD = 0x8009801B,
        WINBIO_E_DUPLICATE_ENROLLMENT = 0x8009801C,
        WINBIO_E_DATABASE_READ_ERROR = 0x8009801D,
        WINBIO_E_DATABASE_WRITE_ERROR = 0x8009801E,
        WINBIO_E_DATABASE_NO_RESULTS = 0x8009801F,
        WINBIO_E_DATABASE_NO_MORE_RECORDS = 0x80098020,
        WINBIO_E_DATABASE_EOF = 0x80098021,
        WINBIO_E_DATABASE_BAD_INDEX_VECTOR = 0x80098022,
        WINBIO_E_INCORRECT_BSP = 0x80098024,
        WINBIO_E_INCORRECT_SENSOR_POOL = 0x80098025,
        WINBIO_E_NO_CAPTURE_DATA = 0x80098026,
        WINBIO_E_INVALID_SENSOR_MODE = 0x80098027,
        WINBIO_E_LOCK_VIOLATION = 0x8009802A,
        WINBIO_E_DUPLICATE_TEMPLATE = 0x8009802B,
        WINBIO_E_INVALID_OPERATION = 0x8009802C,
        WINBIO_E_SESSION_BUSY = 0x8009802D,
        WINBIO_E_CRED_PROV_DISABLED = 0x80098030,
        WINBIO_E_CRED_PROV_NO_CREDENTIAL = 0x80098031,
        WINBIO_E_DISABLED = 0x80098032,
        WINBIO_E_CONFIGURATION_FAILURE = 0x80098033,
        WINBIO_E_SENSOR_UNAVAILABLE = 0x80098034,
        WINBIO_E_SAS_ENABLED = 0x80098035,
        WINBIO_E_DEVICE_FAILURE = 0x80098036,
        WINBIO_E_FAST_USER_SWITCH_DISABLED = 0x80098037,
        WINBIO_E_NOT_ACTIVE_CONSOLE = 0x80098038,
        WINBIO_E_EVENT_MONITOR_ACTIVE = 0x80098039,
        WINBIO_E_INVALID_PROPERTY_TYPE = 0x8009803A,
        WINBIO_E_INVALID_PROPERTY_ID = 0x8009803B,
        WINBIO_E_UNSUPPORTED_PROPERTY = 0x8009803C,
        WINBIO_E_ADAPTER_INTEGRITY_FAILURE = 0x8009803D,
        WINBIO_I_MORE_DATA = 0x00090001
    }
}