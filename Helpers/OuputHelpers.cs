using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.Output
{
    public static class OutputHelpers
    {
<<<<<<< HEAD
        public static void KeyContinue()
        {
            Console.Write("Press any key to continue..."); Console.ReadKey();
=======
        public static void KeyContinue(string message = "Press any key to continue...")
        {
            Console.Write(message); Console.ReadKey();
>>>>>>> main-menu-helper
        }
        public static void ColorError()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Error: ");
            Console.ResetColor();
        }
<<<<<<< HEAD
        
=======
        public static void ColorNew()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("New ");
            Console.ResetColor();
        }
>>>>>>> main-menu-helper
    }
}
