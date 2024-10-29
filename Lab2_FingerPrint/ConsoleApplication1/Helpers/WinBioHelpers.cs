using System;
using System.Runtime.InteropServices;
using WinBioWrapper.Types;

namespace ConsoleApplication
{
    public class WinBioHelpers
    {
        // Functions to get window focus
        // https://learn.microsoft.com/en-us/windows/win32/api/winbio/nf-winbio-winbioenrollcapture
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
       
        [DllImport("user32.dll")]
        //[return: MarshalAs(IntPtr)]
        static extern IntPtr SetActiveWindow(IntPtr hWnd);

        public static void BringConsoleToFront()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

        public static void SetActiveWindow()
        {
            SetActiveWindow(GetConsoleWindow());
        }

        // Check return Value and print error message if there's an error
        public static bool CheckForErrVal(uint retVal)
        {
            if (retVal != 0)
            {
                Console.WriteLine("Error: " + ((WINBIO_ERRORS)retVal).ToString());
                return true;
            }
            else
            {
                Console.WriteLine("OK");
                return false;
            }
        }

        // Convert unmanaged pointer to an array of structs
        public static void MarshalUnmananagedArray2Struct<T>(IntPtr unmanagedArray, int length, out T[] mangagedArray)
        {
            var size = Marshal.SizeOf(typeof(T));
            mangagedArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                IntPtr ins = new IntPtr(unmanagedArray.ToInt64() + i * size);
                mangagedArray[i] = Marshal.PtrToStructure<T>(ins);
            }
        }
    }
}
