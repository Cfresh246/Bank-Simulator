using Bank_Simulator.Input;
using Bank_Simulator.Output;
using Bank_Simulator.UI_design;
using Bank_Simulator.Validation;
using Bank_Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.Services
{
    public class BankAccountServices
    {

        private List<BankAccount> accounts = new List<BankAccount>();

        // Code to create bank account
        // This part of code works
        public void CreateAccount()
        {
            CreatingAccountDesign.Header();

            string name = InputHelper.GetString("Enter your full name: ", "Please enter a valid name please");
            string username;

            while (true)
            {
                CreatingAccountDesign.Header();
                Console.WriteLine($"Enter your full name: {name}"); // If username is incorrect 

                username = InputHelper.GetString("Choose a username: ", "Please enter a valid username please");

                if (name.ToLower() == username.ToLower())  // Username cannot be the same as name
                {
                    OutputHelpers.ColorError();
                    Console.WriteLine("Name cannot be the same as username");
                    OutputHelpers.KeyContinue();
                    continue;
                }

                bool usernameTaken = false;
                foreach (BankAccount account in accounts)
                {
                    if (account.Username.ToLower() == username.ToLower())
                    {
                        OutputHelpers.ColorError();
                        Console.WriteLine("Username already exists. Try another one.");
                        OutputHelpers.KeyContinue();
                        usernameTaken = true;
                        break;
                    }
                }
                if (usernameTaken)
                {
                    continue;
                }
                break;
            }
            string pinInput;

            while (true)  // Initialization of input
            {
                pinInput = InputHelper.GetString("Create a 4-digit PIN: ", "PIN must be exactly 4 digits.");
                if (InputHelper.StringLengthVerification(pinInput, 4, "Pin must be exactly 4 digits."))
                {
                    break;
                }
            }

            string confirmPin;
            string accountNumber;

            while (true)  // Confirmation of pin number
            {
                confirmPin = InputHelper.GetString("Confirm PIN: ", "PINs do not match.");
                if (ValidationHelpers.PinValidation(pinInput, confirmPin))
                {
                    accountNumber = InputHelper.GetAccountNumber();
                    accounts.Add(new BankAccount(name, username, pinInput, accountNumber));
                    break;
                }
            }

            Console.WriteLine("\nAccount created successfully! 🎉");
            Console.WriteLine($"Your account number: {accountNumber}");

            OutputHelpers.KeyContinue("\nPress any key to return to the main menu..");
        }

        public void Authentication() // Authentication Control system
                                     // To do:  if there are no account created in the bank flag error.
        {
            if (accounts.Count == 0)
            {
                OutputHelpers.ColorError();
                Console.WriteLine("No accounts created yet.");
                OutputHelpers.KeyContinue("\nPress any key to return to the main menu..");
                return;
            }

            int tries = 3;
            BankAccount? currentAccount = null;
            string username;

            while (true)
            {
                LoginInterface.Header();

                username = InputHelper.GetString("Enter username: ", "Please enter a valid username");

                bool notFound = true;
                foreach (BankAccount account in accounts)
                {
                    if (username == account.Username)
                    {
                        currentAccount = account;
                        notFound = false;
                        break;
                    }
                }

                if (notFound)
                {
                    OutputHelpers.ColorError();
                    Console.WriteLine("Username not found.");
                    OutputHelpers.KeyContinue();
                    continue;
                }
                break;
            }

            while (tries > 0)
            {
                LoginInterface.Header();
                Console.WriteLine($"Enter username: {username}");

                string pin = InputHelper.GetString("Enter PIN: ", "Incorrect PIN.\n" +
                    $"Attempts remaining: {tries}");

                if (!int.TryParse(pin, out int pinInput) || pin != currentAccount?.Pin)
                {
                    tries--;
                    if (tries == 0)
                    {
                        Console.Write("Account locked due to too many failed attempts.\n" +
                        "Contact support or try again later.\n");
                        OutputHelpers.KeyContinue("\nPress any key to return to the main menu..");

                    }
                    Console.WriteLine("Incorrect PIN.\n" +
                    $"Attempts remaining: {tries}");
                    OutputHelpers.KeyContinue();
                    continue;
                }
                break;

            }

            Console.WriteLine($"Login succesful! Welcome back, {currentAccount.Name}.\n");
            OutputHelpers.KeyContinue();
            DashBoard(currentAccount);
            return;
        }

        public void DashBoard(BankAccount bankAccount)
        {
            while (true)
            {
                DashboardUI.Header();
                Console.WriteLine($"Account Holder: {bankAccount.Name}");
                Console.WriteLine($"Account Number: {bankAccount.AccountNumber}");
                Console.WriteLine($"Balance: ${bankAccount.Balance}");
                DashboardUI.Choice();

                int choice = InputHelper.GetInt("Choose an option: ", 1, 6);

                if (choice == 6)
                {
                    Console.WriteLine("Logging out...");
                    OutputHelpers.KeyContinue("Press any key to return to the main menu.");
                    return;
                }

                switch (choice)
                {
                    case 1: ToDeposit(bankAccount); break;
                    case 2: ToWithdraw(bankAccount); break;
                    case 3: MakeTransfer(bankAccount); break;
                    case 4: ShowTransactionHistory(bankAccount); break;
                    case 5: ShowAccountDetails(bankAccount); break;
                }
            }
        }
        public void ToDeposit(BankAccount bankAccount)
        {
            Headers.DepositUI();
            decimal amount = InputHelper.GetDec("Enter amount to deposit: ");
            bankAccount.Deposit(amount);
            OutputHelpers.KeyContinue();
        }
        public void ToWithdraw(BankAccount bankAccount)
        {
            Headers.WithdrawUI();
            decimal amount = InputHelper.GetDec("Enter amount to withdraw: ");
            bankAccount.Withdraw(amount);
            OutputHelpers.KeyContinue();
        }

        public void MakeTransfer(BankAccount bankAccount)
        {
            Headers.TransferUI();
            string otherAccount = InputHelper.GetString("Enter recipient account number: ", "Invalid account number:");

            bool notFound = true;
            foreach (BankAccount account in accounts)
            {
                if (account.AccountNumber == otherAccount)
                {
                    // if it works
                    notFound = false;

                    BankAccount other = account;
                    decimal amount = InputHelper.GetDec($"Enter amount to transfer to {other.Name}: ");
                    bankAccount.TransferTo(other, amount);  // user bank account transfer to other account.

                    OutputHelpers.KeyContinue();
                    break;
                }
            }
            if (notFound)
            {
                OutputHelpers.ColorError(); Console.WriteLine("Account number not found.");
                OutputHelpers.KeyContinue();
                return;
            }
        }

        public void ShowTransactionHistory(BankAccount bankAccount)
        {
            Headers.HistoryUI();
            Console.WriteLine(string.Join("\n", bankAccount.TransactionHistory));
            Headers.lineUI();
            OutputHelpers.KeyContinue();
        }
        // Account description shown
        /*
            * ========== ACCOUNT DETAILS ==========
            * Account Holder: 
            * Username: 
            * Account Number: 
            * Account Type: 
            * Balance: 
            * Date Created: 
            * =====================================
            */
        public void ShowAccountDetails(BankAccount bankAccount)
        {
            Headers.DetailsUI();
            Console.WriteLine(bankAccount.Description);
            Headers.lineUI();
            OutputHelpers.KeyContinue();
        }
    }   
}
