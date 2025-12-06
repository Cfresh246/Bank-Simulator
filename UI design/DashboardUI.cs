using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.UI_design
{
    public static class DashboardUI
    {
        public static void Header()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("               DASHBOARD                 ");
            Console.WriteLine("=========================================");

        } 
        public static void Color(int number)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(number + ". ");
            Console.ResetColor();
        }
        public static void Color2(int number)
        {
            Console.ForegroundColor= ConsoleColor.DarkRed;
            Console.Write(number + ". ");
            Console.ResetColor();
        }
        public static void Choice()
        {
            Line();
            Color(1); Console.WriteLine("Deposit money");
            Color(2); Console.WriteLine("Withdraw money");
            Color(3); Console.WriteLine("Transfer money");
            Color(4); Console.WriteLine("Transaction history");
            Color(5); Console.WriteLine("Account details");
            Color2(6); Console.WriteLine("Logout");
            Line();
        }
        public static void Line()
        {
            Console.WriteLine("-----------------------------------------");
        }
    }
}
