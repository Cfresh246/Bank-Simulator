using Bank_Simulator.Input;
using Bank_Simulator.UI_design;
using Bank_Simulator.Services;

namespace Bank_Simulator
{
    internal class Program
    {
       public static string path = "C://Users//Cfresh//OneDrive//Desktop//C#//Bank Simulator//Informations.csv.txt";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            
            MainMenu();
        }
        static void MainMenu()  // Main Menu should work for 100%
        {
            BankAccountServices bankaccountmanagement = new BankAccountServices();  // Creating The Bank Management.

            bankaccountmanagement.LoadAccount(); // To load existing account. 

            while (true)
            {
                MainMenuDesign.HeaderMenu();

                // User input handling
                int choice = InputHelper.GetInt("\nChoose an option: ", 1, 3);

                if (choice == 3)
                {
                    Console.WriteLine("\nGoodBye!");
                    break;
                }
                switch (choice)
                {
                    case 1: bankaccountmanagement.CreateAccount(); break;  // First off create account
                    case 2: bankaccountmanagement.Authentication(); break; // To connect to bank account
                }
            }
        }
    }

}
