using System;
using System.Collections.Generic;
using ConsoleApplication.FingerprintHandler.Extensions;
using ConsoleApplication.Helpers;
using ConsoleApplication.OptionsMenu.Models;

namespace ConsoleApplication.OptionsMenu
{
    public static class OptionMenu
    {
        public static void HostMenu()
        {
            List<Option> mainMenu = GetMenu();

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(mainMenu, mainMenu[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < mainMenu.Count)
                    {
                        index++;
                        WriteMenu(mainMenu, mainMenu[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(mainMenu, mainMenu[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    mainMenu[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);
        }
        
        private static List<Option> GetMenu() => new List<Option>
        {
            new Option("Change name", () => ChangeName()),
            new Option("Assign function", () => AssignFunction()),
            new Option("Execute function", () => ReadAndExecuteFingerprint()),
            new Option("Verify", () => VerifyFingerprint()),
            new Option("Enroll", () => Enroll()),
            new Option("Delete", () => DeleteFingerprint()),
            new Option("Exit", () => ExitApplication()),
                
            new Option("DEBUG - Add fingerprint", () => Debug_AddFingerprint()),
            new Option("DEBUG - Execute function", () => Debug_FingerprintFunction()),
        };

        public static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            var spacing = new string('-', 10);
            Console.WriteLine(spacing+ " Menu " + spacing);
            
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

        private static void Enroll()
        {
            Console.Clear();
            
            Console.WriteLine("Begin enroll...");
            WinBioFunctions.Enroll();
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void VerifyFingerprint()
        {
            Console.Clear();
            
            Console.WriteLine("Verifying...");
            Guid? fingerprintId = WinBioFunctions.ReadFingerprintId();
            if (fingerprintId is null)
            {
                return;
            }
            
            Console.WriteLine(FingerprintExtensions.Get(fingerprintId));
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void ReadAndExecuteFingerprint()
        {
            Console.Clear();
            
            Console.WriteLine("Reading...");
            
            Guid? fingerprintId = WinBioFunctions.ReadFingerprintId();
            if (fingerprintId is null)
            {
                return;
            }
            
            FingerprintExtensions.Execute(fingerprintId);
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void ChangeName()
        {
            Console.Clear();
            
            Console.WriteLine("Available fingerprints:");
            FingerprintExtensions.PrintAll();
            
            Console.WriteLine("Enter index to change the name");
            int index = int.Parse(Console.ReadLine());
            
           
            Console.WriteLine("Enter new name");
            string name = Console.ReadLine();
            while (FingerprintExtensions.DoesExist(name))
            {
                Console.WriteLine("\nThis name already exists, please enter new name:");
                name = Console.ReadLine();
            }
            
            FingerprintExtensions.ChangeId(index - 1, name);
            Console.WriteLine($"Name successfully changed to {name}");
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void AssignFunction()
        {
            Console.Clear();
            
            Console.WriteLine("Available functions:");
            Console.WriteLine("1. OpenCalc");
            Console.WriteLine("2. PrintHello");
            Console.WriteLine("Available fingerprints:");
            FingerprintExtensions.PrintAll();
            Console.WriteLine("Enter index to assign function to");
            int index = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter function name to assign");
            string name = Console.ReadLine();
            FingerprintExtensions.AssignFunction(index - 1, name);
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }
        
        private static void DeleteFingerprint()
        {
            Console.Clear();
            
            Console.WriteLine("Available fingerprints:");
            FingerprintExtensions.PrintAll();
            Console.WriteLine("Enter index to remove fingerprint");
            int index = int.Parse(Console.ReadLine());
            FingerprintExtensions.Remove(index - 1);
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }
        
        private static void ExitApplication()
        {
            WinBioFunctions.DeleteTemplate();
            WinBioFunctions.CloseSession();
            Environment.Exit(0);
        }
        
        #region Debug_options
        private static void Debug_FingerprintFunction()
        {
            Console.Clear();
            
            Console.WriteLine("Available fingerprints:");
            FingerprintExtensions.PrintAll();
            Console.WriteLine("Enter index to execute function");
            int index = int.Parse(Console.ReadLine());
            FingerprintExtensions.Execute(index - 1);
            
            Console.WriteLine("\nClick 'down' arrow to go back to menu");
        }

        private static void Debug_AddFingerprint()
        {
            Console.Clear();
            
            FingerprintExtensions.Add(Guid.NewGuid(), "");
            FingerprintExtensions.Add(Guid.NewGuid(), "");

            Console.WriteLine("Click 'down' arrow to go back to menu");
        }
        #endregion
    }
}