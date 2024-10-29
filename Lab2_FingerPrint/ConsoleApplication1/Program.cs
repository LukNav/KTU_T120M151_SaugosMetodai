using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static List<Option> menuMain;
        static void Main(string[] args)
        {
            // Create options that you want your menu to have
            menuMain = new List<Option>
            {
                new Option("Print Internal Values", () => WinBioFuntions.PrintInternalValues()),
                new Option("Enumerate Devices", () => WinBioFuntions.EnumerateDevices()),
                new Option("Enumerate Databases", () => WinBioFuntions.EnumerateDatabases()),
                new Option("Enumerate Enrollments", () => WinBioFuntions.EnumerateEnrollments()),
                new Option("Open Session", () => WinBioFuntions.OpenSession()),
                new Option("Locate Sensor", () => WinBioFuntions.LocateSensor()),
                new Option("Select Biometric Subtype", () => WinBioFuntions.SelectBiometricSubtype()),
                new Option("Identify", () => WinBioFuntions.Identify()),
                new Option("Verify", () => WinBioFuntions.Verify()),
                new Option("Enroll", () => WinBioFuntions.Enroll()),
                new Option("Delete Template", () => WinBioFuntions.DeleteTemplate()),
                new Option("Close Session", () => WinBioFuntions.CloseSession()),
                new Option("Exit", () => Environment.Exit(0)),
            };

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
