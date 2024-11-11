using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using ConsoleApplication.FingerprintHandler.Models;
using WinBioWrapper.Types;

namespace ConsoleApplication.FingerprintHandler.Extensions
{
    public static class FingerprintExtensions
    {
        public static PrintFunction PrintLabaFunc = new PrintFunction("Print Laba", "Laba");
        public static PrintFunction PrintDienaFunc = new PrintFunction("Print Diena", "Diena");

        private static Dictionary<WINBIO_BIOMETRIC_SUBTYPE, SavedFingerprint> functionFingerprints = new Dictionary<WINBIO_BIOMETRIC_SUBTYPE, SavedFingerprint>();

        public static IEnumerable<SavedFingerprint> GetAll()
        {
            return functionFingerprints.Values;
        }

        public static SavedFingerprint Get(WINBIO_BIOMETRIC_SUBTYPE type)
        {
            return functionFingerprints[type];
        }

        public static void Add(WINBIO_BIOMETRIC_SUBTYPE type, string name)
        {
            functionFingerprints.Add(type, new SavedFingerprint(Guid.NewGuid(), name, type));
        }

        public static void Add(SavedFingerprint[] fingerprints)
        {
            foreach(var fingerprint in fingerprints)
            {
                functionFingerprints.Add(fingerprint.Type, fingerprint);
            }
        }

        public static void Remove(WINBIO_BIOMETRIC_SUBTYPE type)
        {
            functionFingerprints.Remove(type);
        }

        public static void RemoveAll()
        {
            functionFingerprints.Clear();
        }

        public static bool DoesExist(string name)
        {
            return functionFingerprints.Any(fingerprint =>
                fingerprint.Value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void SetName(WINBIO_BIOMETRIC_SUBTYPE type, string name)
        {
            functionFingerprints[type].Name = name;
        }

        public static void AssignFunction(WINBIO_BIOMETRIC_SUBTYPE index, string functionName)
        {
            var function = functionFingerprints[index];

            switch (functionName)
            {
                case "PrintLaba":
                    foreach (var item in functionFingerprints.Where(x => x.Value.Function != null && x.Value.Function.Name == "Open calculator"))
                    {
                        item.Value.Function = null;
                    }
                    function.Function = PrintLabaFunc;
                    Console.WriteLine("Function successfully assigned");
                    break;
                case "PrintDiena":
                    foreach (var item in functionFingerprints.Where(x => x.Value.Function != null && x.Value.Function.Name == "Print hello"))
                    {
                        item.Value.Function = null;
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
                return;
            }

            int i = 1;
            foreach(var fingerprint in functionFingerprints.Values)
            {
                Console.WriteLine("({0}) {1}", i++, fingerprint);
            }
        }

        public static void Execute(WINBIO_BIOMETRIC_SUBTYPE? type)
        {
            var function = functionFingerprints.First(x => x.Value.Type == type).Value.Function;

            if (function?.Name == null)
            {
                Console.WriteLine("Fingerprint does not have function assigned");
                return;
            }

            Console.Clear();
            function.Print();
        }

        public static void Execute(WINBIO_BIOMETRIC_SUBTYPE index)
        {
            var function = functionFingerprints[index].Function;

            if (function == null)
            {
                Console.WriteLine("Fingerprint does not have function assigned");
                return;
            }

            function.Print();
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
