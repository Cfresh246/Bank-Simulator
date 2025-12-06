using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.UI_design
{
    public static class Headers
    {
        public static void DepositUI()
        {
            Console.Clear();
            Console.WriteLine("-------------- DEPOSIT --------------");
        }
        public static void WithdrawUI()
        {
            Console.Clear();
            Console.WriteLine("-------------- WITHDRAW --------------");
        }
        public static void TransferUI()
        {
            Console.Clear();
            Console.WriteLine("------------- TRANSFER -------------");
        }
        public static void HistoryUI()
        {
            Console.Clear();
            Console.WriteLine("========== TRANSACTION HISTORY ==========");
        }
        public static void DetailsUI()
        {
            Console.Clear();
            Console.WriteLine("========== ACCOUNT DETAILS ==========");
        }
        public static void lineUI()
        {
            Console.WriteLine("=========================================\n");
        }
        public static void line2()
        {
            Console.WriteLine("-------------------------------------------\n");
        }
    }
}
