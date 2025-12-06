using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.UI_design
{
    public static class MainMenuDesign
    {
        public static void HeaderMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("             BANK SIMULATOR              ");
            Console.WriteLine("=========================================\n");
            Choice();
        }
        public static void Choice()
        {
            Color(1); Console.WriteLine("Create an account");
            Color(2); Console.WriteLine("Log in");
            Color(3); Console.WriteLine("Exit");
        }
        public static void Color(int number)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(number + ". ");
            Console.ResetColor();
        }
    }
}
