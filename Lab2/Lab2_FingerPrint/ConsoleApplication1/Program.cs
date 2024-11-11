using ConsoleApplication.Extensions;
using ConsoleApplication.Options;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WinBioExtensions.OpenSession();
            OptionMenu.Init();
        }
    }
}
