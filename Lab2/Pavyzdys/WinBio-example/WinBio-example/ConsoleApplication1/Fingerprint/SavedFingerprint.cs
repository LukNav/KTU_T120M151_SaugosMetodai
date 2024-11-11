using System;
using WinBioWrapper.Types;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class SavedFingerprint
    {
        public WINBIO_IDENTITY Id { get; set; }
        public string Name { get; set; }
        public FingerprintFunction Function { get; set; }

        public SavedFingerprint(WINBIO_IDENTITY id, string name)
        {
            Id = id;
            Name = name;
            Function = null;
        }

        public override string ToString()
        {
            var nameString = Name == string.Empty ? Id.ToString() : Name;
            var functionString = Function == null ? "" : Function.Name == "Empty" ? "" : ". Assigned function: " + Function.Name;

            return nameString + functionString;
        }
    }
}