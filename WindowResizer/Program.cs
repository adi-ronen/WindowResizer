using System;
using System.Diagnostics;

namespace WindowResizer
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3 || args[0] == "/?" || args[0].ToLower() == "-help"
                || (args.Length == 3 && args[2].ToLower() != "all"))
            {
                printHelp();
                Environment.Exit(0);
            }

            bool All = false;
            if (args.Length == 3) All = true;

            if (args[1].ToLower() == "min" || args[1].ToLower() == "max" || args[1].ToLower() == "normal" ||
                args[1].ToLower() == "l" || args[1].ToLower() == "r" || args[1].ToLower() == "t" ||
                args[1].ToLower() == "b" || args[1].ToLower() == "tl" || args[1].ToLower() == "bl" ||
                args[1].ToLower() == "tr" || args[1].ToLower() == "br")
            {
                Class1.FindWindow(args[0], args[1], All);
            }
            else
                printHelp();

            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void printHelp()
        {
            Console.WriteLine();
            Console.WriteLine("WindowResizer - About:");
            Console.WriteLine("**********************");
            Console.WriteLine("Created by Adi Ronen for Ben-Gurion University,");
            Console.WriteLine("Guilford Glazer Faculty of Business and Managements.\n");
            Console.WriteLine("Compilation Date: 26.9.2016");
            Console.WriteLine("Compiled for: .NET version 4.6");
            Console.WriteLine("Tested on: Windows 8.1\\10 64-bit");
            Console.WriteLine();
            Console.WriteLine("Description:");
            Console.WriteLine("------------");
            Console.WriteLine("This application is used for automatically resizing and setting location for a chosen windows on the screen.");
            Console.WriteLine();
            Console.WriteLine("HOW TO USE:");
            Console.WriteLine("-----------");
            Console.WriteLine("FullPath\\WindowResizer.exe <Title or Part of title> <Position's parameters> [ALL]");
            Console.WriteLine();
            Console.WriteLine("Position's parameters:");
            Console.WriteLine();
            Console.WriteLine("     Min    - Minimize window.");
            Console.WriteLine("     Max    - Maximize window.");
            Console.WriteLine("     Normal - Normal window.");
            Console.WriteLine("     T      - Top half of the screen.");
            Console.WriteLine("     B      - Bottom half of the screen.");
            Console.WriteLine("     L      - Left half of the screen.");
            Console.WriteLine("     R      - Right half of the screen.");
            Console.WriteLine("     TL     - Top left of the screen.");
            Console.WriteLine("     TR     - Top right of the screen.");
            Console.WriteLine("     BL     - Bottom left of the screen.");
            Console.WriteLine("     BR     - Bottom right of the screen.");
            Console.WriteLine();
            Console.WriteLine("ALL :    Use ALL if you would like to include all the windows that contains the given title.");
            Console.WriteLine("         If you will not add ALL and there is more then one window open that contains the same title,");
            Console.WriteLine("         the program will choose randomly one of the windows that contains the given window title.");
            Console.WriteLine("         Its recomended to write exactly the Title to allow the program to work precisely on the specific window");
            Console.WriteLine("         or just use parameter ALL to include all relevant windows in action.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("---------");
            Console.WriteLine("c:\\Windows\\WindowResizer.exe Outlook max ALL");
            Console.WriteLine();
            Console.WriteLine("c:\\Windows\\WindowResizer.exe \"Untitled - Notepad\" L");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
