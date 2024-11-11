using System;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class SavedFingerprint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FingerprintFunction Function { get; set; }

        public SavedFingerprint(Guid id, string name)
        {
            Id = id;
            Name = name;
            Function = null;
        }

        public override string ToString()
        {
            var nameString = Name == string.Empty ? Id.ToString() : Name;
            var functionString = Function == null ? "" : Function.Name == "Empty" ? "" : ". Assigned function: "+Function.Name;

            return nameString+functionString;
        }
    }
}