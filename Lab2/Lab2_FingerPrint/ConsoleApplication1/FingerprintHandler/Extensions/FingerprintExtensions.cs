using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConsoleApplication.FingerprintHandler.Models;

namespace ConsoleApplication.FingerprintHandler.Extensions
{
    public static class FingerprintExtensions
    {
        public static FingerprintFunction PrintLabaFunc = new FingerprintFunction("Print Laba", PrintLaba);
        public static FingerprintFunction PrintDienaFunc = new FingerprintFunction("Print Diena", PrintDiena);

        private static List<SavedFingerprint> functionFingerprints = new List<SavedFingerprint>();

        public static SavedFingerprint Get(Guid? id)
        {
            return functionFingerprints.FirstOrDefault(x => x.Id == id);
        }

        public static void Add(Guid id, string name)
        {
            functionFingerprints.Add(new SavedFingerprint(id, name));
        }

        public static void Remove(int index)
        {
            functionFingerprints.RemoveAt(index);
        }

        
        public static bool DoesExist(string name)
        {
            return functionFingerprints.Any(fingerprint =>
                fingerprint.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void ChangeId(int index, string name)
        {
            functionFingerprints[index].Name = name;
        }
        
        public static void AssignFunction(int index, string functionName)
        {
            var function = functionFingerprints[index];

            switch (functionName)
            {
                case "PrintLaba":
                    foreach (var item in functionFingerprints.Where(x => x.Function != null && x.Function.Name == "Open calculator"))
                    {
                        item.Function = null;
                    }
                    function.Function = PrintLabaFunc;
                    Console.WriteLine("Function successfully assigned");
                    break;
                case "PrintDiena":
                    foreach (var item in functionFingerprints.Where(x => x.Function != null && x.Function.Name == "Print hello"))
                    {
                        item.Function = null;
                    }
                    function.Function = PrintDienaFunc;
                    Console.WriteLine("Function successfully assigned");
                    break;
                default:
                    Console.WriteLine("Function does not exist");
                    break;
            }
        }

        public static void PrintAll()
        {
            if (functionFingerprints.Count == 0)
            {
                Console.WriteLine("No fingerprints are registered");
            }
            
            for(int i = 0; i < functionFingerprints.Count; i++)
            {
                var fingerprintNames = functionFingerprints[i];

                Console.WriteLine("({0}) {1}", i + 1, fingerprintNames);
            }
        }

        public static void Execute(Guid? id)
        {
            var function = functionFingerprints.First(x => x.Id == id).Function;

            if (function.Name == null)
            {
                Console.WriteLine("Fingerprint does not have function assigned");
                return;
            }

            function.Action.Invoke();
        }

        public static void Execute(int index)
        {
            var function = functionFingerprints[index].Function;

            if (function == null)
            {
                Console.WriteLine("Fingerprint does not have function assigned");
                return;
            }

            function.Action.Invoke();
        }

        private static void PrintLaba()
        {
            Console.WriteLine("Laba");
        }
        
        private static void PrintDiena()
        {
            Console.WriteLine("Diena");
        }
    }
}
