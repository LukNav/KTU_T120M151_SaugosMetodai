using ConsoleApplication.Extensions;
using ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.OptionsMenu
{
    public static class OptionsMenu
    {
        public static List<Option> menuMain { get; internal set; } = null;
        public static void StartMenu()
        {
            if (menuMain != null)
            {
                return;
            }

            menuMain = new List<Option>
                {
                    new Option("Print Internal Values", () => WinBioExtensions.PrintInternalValues()),
                    new Option("Enumerate Devices", () => WinBioExtensions.EnumerateDevices()),
                    new Option("Enumerate Databases", () => WinBioExtensions.EnumerateDatabases()),
                    new Option("Enumerate Enrollments", () => WinBioExtensions.EnumerateEnrollments()),
                    new Option("Locate Sensor", () => WinBioExtensions.LocateSensor()),
                    new Option("Select Biometric Subtype", () => WinBioExtensions.SelectBiometricSubtype()),
                    new Option("Identify", () => WinBioExtensions.Identify()),
                    new Option("Verify", () => WinBioExtensions.Verify()),
                    new Option("Enroll", () => WinBioExtensions.Enroll()),
                    new Option("Delete Template", () => WinBioExtensions.DeleteTemplate()),
                    new Option("Close Session", () => WinBioExtensions.CloseSession()),
                    new Option("Exit", () => Environment.Exit(0)),
                };


            WinBioExtensions.OpenSession();

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(menuMain, menuMain[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < menuMain.Count)
                    {
                        index++;
                        WriteMenu(menuMain, menuMain[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(menuMain, menuMain[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    menuMain[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();

        }

        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }
    }
}
