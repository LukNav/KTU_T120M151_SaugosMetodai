using System;

namespace ConsoleApplication.OptionsMenu.Models
{
    public class Option
    {
        public string Name { get; set; }
        public Action Selected { get; set; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
