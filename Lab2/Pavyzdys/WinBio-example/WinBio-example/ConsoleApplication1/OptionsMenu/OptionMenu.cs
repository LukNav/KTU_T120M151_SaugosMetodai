using ConsoleApplication.Extensions;
using ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication.FingerprintHandler.Extensions;
using WinBioWrapper.Types;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using ConsoleApplication.FingerprintHandler.Models;

namespace ConsoleApplication.Options
{
    public static class OptionMenu
    {
        public static List<Option> menuMain { get; internal set; } = null;
        public static void Init()
        {
            LoadSavedFingerprints();

            menuMain = new List<Option>
                {
                    new Option("Enroll", () => Enroll()),
                    new Option("Change name", () => ChangeName()),
                    new Option("Assign function", () => AssignFunction()),
                    new Option("Execute function", () => ReadAndExecuteFingerprint()),
                    //new Option("Print Internal Values", () => WinBioExtensions.PrintInternalValues()),
                    //new Option("Enumerate Devices", () => WinBioExtensions.EnumerateDevices()),
                    //new Option("Enumerate Databases", () => WinBioExtensions.EnumerateDatabases()),
                    //new Option("Select Biometric Subtype", () => WinBioExtensions.SelectBiometricSubtype()),
                    new Option("Verify", () => Verify()),
                    new Option("Delete fingerprint", () => ReadAndDeleteFingerprint()),
                    new Option("Delete all fingerprints", () => DeleteAllFingerprints()),

                    //new Option("Close Session", () => WinBioExtensions.CloseSession()),
                    new Option("Enumerate Enrollments", () => EnumerateEnrollments()),

                    new Option("Exit", () => ExitMenu()),
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

        private static void Verify()
        {
            Console.Clear();
            WinBioExtensions.Identify();
            Console.Clear();
            WinBioExtensions.Verify();
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

            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Registered fingerprints:");
            FingerprintExtensions.PrintAll();
        }

        private static void AssignFunction()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            Console.Clear();

            Console.WriteLine("Available functions:");
            Console.WriteLine("1. PrintLaba");
            Console.WriteLine("2. PrintDiena");
            Console.WriteLine("Scan fingerprint to assign function to");
            var fingerType = WinBioExtensions.Identify();
            while(fingerType == null)
            {
                Console.WriteLine("Try again");
                fingerType = WinBioExtensions.Identify();
            }
            Console.WriteLine("Enter function name to assign");
            string name = Console.ReadLine();
            FingerprintExtensions.AssignFunction((WINBIO_BIOMETRIC_SUBTYPE)fingerType, name);

            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void ReadAndExecuteFingerprint()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            Console.Clear();

            Console.WriteLine("Reading...");

            WINBIO_BIOMETRIC_SUBTYPE? fingerprint = WinBioExtensions.Identify();
            if (fingerprint == null)
            {
                return;
            }

            FingerprintExtensions.Execute(fingerprint);

            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void ReadAndDeleteFingerprint()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            Console.Clear();

            Console.WriteLine("Reading...");

            WINBIO_BIOMETRIC_SUBTYPE? fingerprint = WinBioExtensions.Identify();
            if (fingerprint == null)
            {
                return;
            }
            WinBioExtensions.DeleteCurrentTemplate();
            FingerprintExtensions.Remove((WINBIO_BIOMETRIC_SUBTYPE)fingerprint);

            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void ChangeName()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            Console.Clear();

            Console.WriteLine("Put finger on sensor:");
            var fingerprintType = WinBioExtensions.Identify();
            while (fingerprintType == null)
            {
                Console.WriteLine("Woops, try again:");
                fingerprintType = WinBioExtensions.Identify();
            }

            Console.WriteLine("Enter new name");
            string name = Console.ReadLine();
            while (FingerprintExtensions.DoesExist(name))
            {
                Console.WriteLine("\nThis name already exists, please enter new name:");
                name = Console.ReadLine();
            }

            FingerprintExtensions.SetName((WINBIO_BIOMETRIC_SUBTYPE)fingerprintType, name);
            Console.WriteLine("Name successfully changed to {0}", name);

            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void EnumerateEnrollments()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            if (!WinBioExtensions.IsIdentified())
            {
                WinBioExtensions.Identify();
            }
            Console.Clear();

            WinBioExtensions.EnumerateEnrollments();
        }

        private static void Enroll()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            Console.Clear();

            WinBioExtensions.Enroll();
        }

        private static void DeleteAllFingerprints()
        {
            Console.Clear();
            WinBioExtensions.LocateSensor();
            if(!WinBioExtensions.IsIdentified())
            {
                WinBioExtensions.Identify();
            }
            Console.Clear();

            foreach (WINBIO_BIOMETRIC_SUBTYPE fingerType in Enum.GetValues(typeof(WINBIO_BIOMETRIC_SUBTYPE)))
            {
                WinBioExtensions.DeleteTemplate(fingerType);
                Console.WriteLine($"Deleted template: {fingerType}");
            }
            FingerprintExtensions.RemoveAll();
        }

        private static void ExitMenu()
        {
            WinBioExtensions.CloseSession();
            SaveCurrentState();
            Environment.Exit(0);
        }

        private static void SaveCurrentState()
        {
            string serializedFingerprints = JsonSerializer.Serialize(FingerprintExtensions.GetAll().ToArray());
            string docPath = Environment.CurrentDirectory;
            docPath = Path.Combine(docPath, "SavedFingerprints.txt");

            if (File.Exists(docPath))
            {
                File.Delete(docPath);
            }
            File.WriteAllText(docPath, serializedFingerprints);
        }

        private static void LoadSavedFingerprints()
        {
            string docPath = Environment.CurrentDirectory;
            docPath = Path.Combine(docPath, "SavedFingerprints.txt");

            if (File.Exists(docPath))
            {
                string serializedFingerprints = File.ReadAllText(docPath);
                var fingerprints = JsonSerializer.Deserialize<SavedFingerprint[]> (serializedFingerprints);
                FingerprintExtensions.Add(fingerprints);
            }
        }
    }
}
