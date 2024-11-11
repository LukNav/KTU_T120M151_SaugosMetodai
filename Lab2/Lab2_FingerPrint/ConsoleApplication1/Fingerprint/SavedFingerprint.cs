using System;
using WinBioWrapper.Types;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class SavedFingerprint
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public WINBIO_BIOMETRIC_SUBTYPE Type { get; set; }
        public PrintFunction Function { get; set; }


        public SavedFingerprint()
        {
        }

        public SavedFingerprint(Guid id, string name,  WINBIO_BIOMETRIC_SUBTYPE type, PrintFunction function = null)
        {
            Id = id;
            Name = name;
            Function = function;
            Type = type;
        }

        public override string ToString()
        {
            var nameString = Name == string.Empty ? Id.ToString() : Name;
            var functionString = Function == null ? "" : Function.Name == "Empty" ? "" : ". Assigned function: " + Function.Name;

            return nameString + functionString;
        }
    }
}