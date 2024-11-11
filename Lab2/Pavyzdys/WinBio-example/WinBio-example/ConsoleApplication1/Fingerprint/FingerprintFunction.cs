using System;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class FingerprintFunction
    {
        public string Name { get; set; }
        public Action Action { get; set; }

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