using System;

namespace ConsoleApplication.FingerprintHandler.Models
{
    public class PrintFunction
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public PrintFunction()
        {
        }

        public PrintFunction(string name, string message)
        {
            this.Name = name;
            this.Message = message;
        }

        public void Print()
        {
            Console.WriteLine(Message);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}