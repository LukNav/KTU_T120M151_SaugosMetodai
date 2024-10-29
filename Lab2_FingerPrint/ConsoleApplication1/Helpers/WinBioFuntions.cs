using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using WinBioWrapper.Types;

namespace ConsoleApplication
{
    public class WinBioFuntions
    {
        static IntPtr _sessionHandle = IntPtr.Zero;
        static uint _unitId = 0;
        static WINBIO_BIOMETRIC_SUBTYPE _subtype = WINBIO_BIOMETRIC_SUBTYPE.WINBIO_ANSI_381_POS_LH_INDEX_FINGER;
        static WINBIO_IDENTITY _identity = new WINBIO_IDENTITY();

        public static void PrintInternalValues()
        {
            Console.WriteLine("_sessionHandle" + " : " + _sessionHandle.ToString());
            Console.WriteLine("_unitId" + " : " + _unitId.ToString());
            Console.WriteLine("_subtype" + " : " + _subtype.ToString());
            Console.WriteLine("_identity" + " : " + JsonConvert.SerializeObject(_identity, Formatting.Indented));
        }

        public static void EnumerateDevices()
        {
            Console.WriteLine("Enumerating devices...");
            int n_units = 0;
            IntPtr units_ptr = IntPtr.Zero;
            WINBIO_UNIT_SCHEMA[] units;
            uint ret = WinBio.Native.WinBioEnumBiometricUnits(WINBIO_BIOMETRIC_TYPE.WINBIO_TYPE_FINGERPRINT, ref units_ptr, ref n_units);
            if (WinBioHelpers.CheckForErrVal(ret)) { return; }
            WinBioHelpers.MarshalUnmananagedArray2Struct<WINBIO_UNIT_SCHEMA>(units_ptr, n_units, out units);
            Console.WriteLine("Found devices: " + n_units);
            Console.WriteLine(JsonConvert.SerializeObject(units, Formatting.Indented));
        }

        public static void EnumerateDatabases()
        {
            Console.WriteLine("Enumerating databases...");
            int n_databases = 0;
            IntPtr databases_ptr = IntPtr.Zero;
            WINBIO_STORAGE_SCHEMA[] databases;
            uint ret = WinBio.Native.WinBioEnumDatabases(WINBIO_BIOMETRIC_TYPE.WINBIO_TYPE_FINGERPRINT, ref databases_ptr, ref n_databases);
            if (WinBioHelpers.CheckForErrVal(ret)) { return; }
            WinBioHelpers.MarshalUnmananagedArray2Struct<WINBIO_STORAGE_SCHEMA>(databases_ptr, n_databases, out databases);
            Console.WriteLine("Found databases: " + n_databases);
            Console.WriteLine(JsonConvert.SerializeObject(databases, Formatting.Indented));
        }

        public static void EnumerateEnrollments()
        {
            Console.WriteLine("Enumerating Enrollments...");
            int n_subfactors = 0;
            IntPtr subfactors_ptr = IntPtr.Zero;
            byte[] subfactors;
            uint ret = WinBio.Native.WinBioEnumEnrollments(
                _sessionHandle,
                _unitId,
                _identity,
                ref subfactors_ptr,
                ref n_subfactors);
            if (WinBioHelpers.CheckForErrVal(ret)) { return; }
            Console.WriteLine("Found enrollments: " + n_subfactors);
            subfactors = new byte[n_subfactors];
            Marshal.Copy(subfactors_ptr, subfactors, 0, n_subfactors);
            foreach (byte sf in subfactors)
            {
                Console.WriteLine(((WINBIO_BIOMETRIC_SUBTYPE)sf).ToString());
            }
        }

        public static void OpenSession()
        {
            Console.WriteLine("Open session...");
            uint ret = WinBio.Native.WinBioOpenSession(
                WINBIO_BIOMETRIC_TYPE.WINBIO_TYPE_FINGERPRINT,
                WINBIO_POOL_TYPE.WINBIO_POOL_SYSTEM,
                WINBIO_SESSION_FLAGS.WINBIO_FLAG_DEFAULT,
                IntPtr.Zero,
                0,
                /*DatabaseID*/1,
                ref _sessionHandle);
            WinBioHelpers.CheckForErrVal(ret);
        }

        public static void CloseSession()
        {
            Console.WriteLine("Closing session...");
            if (_sessionHandle != IntPtr.Zero)
            {
                uint ret = WinBio.Native.WinBioCloseSession(_sessionHandle);
                _sessionHandle = IntPtr.Zero;
                WinBioHelpers.CheckForErrVal(ret);
            }
            else
            {
                Console.WriteLine("Session is not open!");
            }
        }

        public static void LocateSensor()
        {
            Console.WriteLine("Locating sensor - tap the sensor once...");
            WinBioHelpers.BringConsoleToFront();
            uint ret = WinBio.Native.WinBioLocateSensor(_sessionHandle, ref _unitId);
            if (!WinBioHelpers.CheckForErrVal(ret))
            {
                Console.WriteLine("Found unit ID = " + _unitId);
            }
        }

        public static void SelectBiometricSubtype()
        {
            Console.WriteLine("Available biometric subtypes:");
            foreach (byte value in Enum.GetValues(typeof(WINBIO_BIOMETRIC_SUBTYPE)))
            {
                Console.WriteLine(value + "\t: " + (WINBIO_BIOMETRIC_SUBTYPE)value);
            }
            Console.Write("Enter number and press Enter: ");
            string val = Console.ReadLine();
            try
            {
                if (Enum.TryParse(val, out _subtype))
                {
                    Console.Write("Selected: " + _subtype.ToString());
                }
                else
                {
                    Console.Write("ERR - Could not parse entered value");
                }
            }
            catch (Exception e)
            {
                Console.Write("ERR - " + e.ToString());
            }

        }

        public static void Enroll()
        {
            //  Begin enroll
            Console.WriteLine("Begin enroll...");
            Console.WriteLine("Selected biometric subtype: " + _subtype.ToString());
            WinBioHelpers.BringConsoleToFront();
            uint ret = WinBio.Native.WinBioEnrollBegin(_sessionHandle, _subtype, _unitId);
            if (WinBioHelpers.CheckForErrVal(ret)) { return; }
            // Enroll capture
            WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_SUCCESS;
            for (int swipeCount = 0; ; ++swipeCount)
            {
                Console.WriteLine("Swipe the sensor to capture a sample...");
                ret = WinBio.Native.WinBioEnrollCapture(_sessionHandle, ref rejectDetail);
                if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_I_MORE_DATA)
                {
                    Console.WriteLine("More data required");
                    continue;
                }
                if (ret != 0)
                {
                    if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_E_BAD_CAPTURE)
                    {
                        Console.WriteLine("Bad capture, reason: " + rejectDetail.ToString());
                    }
                    else
                    {
                        WinBioHelpers.CheckForErrVal(ret);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Template completed");
                    break;
                }
            }

            Console.WriteLine("Do you want to commit? Press Y for yes");
            string ans = Console.ReadLine();
            if (ans.ToUpper() == "Y")
            {
                // Commmit enrollment
                Console.WriteLine("Commit template...");
                bool isNew = false;
                ret = WinBio.Native.WinBioEnrollCommit(_sessionHandle, ref _identity, ref isNew);
                if (WinBioHelpers.CheckForErrVal(ret)) { return; }
                Console.WriteLine("Template is new: " + isNew);
                Console.WriteLine("Template identity:");
                Console.WriteLine(JsonConvert.SerializeObject(_identity, Formatting.Indented));
            }
            else
            {
                // Discard enrollment
                Console.WriteLine("Discarding enrollment...");
                WinBio.Native.WinBioEnrollDiscard(_sessionHandle);
                if (WinBioHelpers.CheckForErrVal(ret)) { return; }
            }
        }

        public static void Identify()
        {
            Console.WriteLine("Identifying...");
            Console.WriteLine("Swipe finger on a sensor");
            WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_SUCCESS;
            WinBioHelpers.BringConsoleToFront();
            uint ret = WinBio.Native.WinBioIdentify(
                _sessionHandle,
                ref _unitId,
                ref _identity,
                ref _subtype,
                ref rejectDetail);
            if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_E_BAD_CAPTURE)
            {
                Console.WriteLine("Bad capture, reason: " + rejectDetail.ToString());
            }
            else if (!WinBioHelpers.CheckForErrVal(ret))
            {
                PrintInternalValues();
            }
        }

        public static void Verify()
        {
            Console.WriteLine("Verifying...");
            Console.WriteLine("Swipe finger on a sensor");
            WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_SUCCESS;
            bool match = false;
            WinBioHelpers.BringConsoleToFront();
            uint ret = WinBio.Native.WinBioVerify(
                _sessionHandle,
                _identity,
                _subtype,
                ref _unitId,
                ref match,
                ref rejectDetail);
            if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_E_BAD_CAPTURE)
            {
                Console.WriteLine("Bad capture, reason: " + rejectDetail.ToString());
            }
            else if (!WinBioHelpers.CheckForErrVal(ret))
            {
                Console.WriteLine("Match: " + match.ToString());
                PrintInternalValues();
            }
        }

        public static void DeleteTemplate()
        {
            Console.WriteLine("Deleting template...");
            uint ret = WinBio.Native.WinBioDeleteTemplate(_sessionHandle, _unitId, _identity, _subtype);
            WinBioHelpers.CheckForErrVal(ret);
        }
    }
}
