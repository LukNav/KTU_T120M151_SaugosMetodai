using System;



// Modified and adapted from
// https://github.com/mjanbazi/WinBio

namespace ConsoleApplication
{
    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
