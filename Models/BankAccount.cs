using Bank_Simulator.Output;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.Models
{
    
    public class BankAccount
    {
        // Every personal bank account parameters
        public string Name { get; }
        public string Username { get; }
        public string Pin { get; }
        public string AccountNumber { get; }

        public DateOnly TimeCreated { get; }

        public DateTime LastUpdated { get; private set; }
        public decimal Balance { get; private set; }

        public List<string> TransactionHistory { get; } = new List<string>();  // list of transaction

        public string Description => $"Account Holder: {Name}\n" +
            $"Username: {Username}\n" +
            $"Account Number: {AccountNumber}\n" +
            $"Account Type: Checking\n" +
            $"Balance: ${Balance}\n" +
            $"Date Created: {TimeCreated}";

        // Deposit to the user bank account itself
        public void Deposit(decimal amount)
        {
            if (amount <= 0)  // Handling of non positive number and 0.
            {
                OutputHelpers.ColorError();
                Console.WriteLine("Please enter a valid amount.");
                return;
            }

            Balance += amount;
            LastUpdated = DateTime.UtcNow;
            TransactionHistory.Add($"[{LastUpdated}] +${amount:F2} Deposit ");

            Console.WriteLine("\nDeposit succesful!");
            OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${Balance}\n");
        }

        // user's withdrawal.
        public void Withdraw(decimal amount)
        {
            if (Balance < amount) // Insufficient funds handling.
            {
                OutputHelpers.ColorError();
                Console.WriteLine("Insufficient funds.");
                return;
            }

            if (amount <= 0)
            {
                OutputHelpers.ColorError();
                Console.WriteLine("Please enter a valid amount");
                return;
            }

            Balance -= amount;
            LastUpdated = DateTime.UtcNow;
            TransactionHistory.Add($"[{LastUpdated}] -${amount:F2} Withdrawal ");

            Console.WriteLine("\nWithdrawal succesful!");
            OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${Balance}\n");
        }
        public void TransferTo(BankAccount other, decimal amount)
        {
            if (Balance < amount)
            {
                OutputHelpers.ColorError();
                Console.WriteLine("Insufficient funds.");
                return;
            }

            if (amount <= 0)
            {
                OutputHelpers.ColorError();
                Console.WriteLine("Please enter a valid amount");
                return;
            }
            // How we handle the transfer between 2 accounts
            Balance -= amount;
            other.Balance += amount;

            LastUpdated = DateTime.UtcNow; // Time it was made
                                                                    // transaction history for both account.
            TransactionHistory.Add($"[{LastUpdated}] -${amount:F2} Transfer to {other.AccountNumber} ");
            other.TransactionHistory.Add($"[{other.LastUpdated}] +${amount:F2} Transfer from {AccountNumber} ");

            Console.WriteLine("\nTransfer succesful!");
            OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${Balance}\n");


        }
        public void LoadBalance(decimal amount)
        {
            Balance += amount;
        }
        public BankAccount(string _name, string _username, string _pin, string _accountNumber, DateOnly _timeCreated = default)
        {
            Name = _name;
            Username = _username;
            Pin = _pin;
            AccountNumber = _accountNumber;

            if (_timeCreated == default)
            {
                TimeCreated = DateOnly.FromDateTime(DateTime.UtcNow);
            }
            else
            {
                TimeCreated = _timeCreated;
            }
        }

    }
    
}
