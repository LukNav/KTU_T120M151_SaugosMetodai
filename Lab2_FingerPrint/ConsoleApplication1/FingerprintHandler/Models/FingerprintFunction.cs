using System;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class FingerprintFunction
    {
        public string Name { get; }
        public Action Action { get; }

        public FingerprintFunction(string name, Action action)
        {
            this.Name = name;
            this.Action = action;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}