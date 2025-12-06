using Bank_Simulator.Input;
using Bank_Simulator.Validation;
using Bank_Simulator.Output;
using Bank_Simulator.UI_design;

namespace Bank_Simulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            MainMenu();
        }
        static void MainMenu()  // Main Menu should work for 100%
        {
            BankAccountManagement bankaccountmanagement = new BankAccountManagement();  // Creating The Bank Management.

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
    class BankAccount
    {
        // Every personal bank account parameters
        public string Name { get; }
        public string Username { get; }
        public string Pin { get; private set; }
        public string AccountNumber { get; }

        public DateOnly TimeCreated { get; }

        public DateOnly LastUpdated { get; set; }
        public decimal Balance { get; private set; }

        public List<string> TransactionHistory { get; } = new List<string>();  // list of transaction

        public string Description => $"Account Holder: {Name}\n" +
            $"Username: {Username}\n" +
            $"Account Number: {AccountNumber}\n" +
            $"Account Type: Checking\n" +
            $"Balance: ${Balance}\n" +
            $"Date Created: {TimeCreated}";

        public void Deposit(decimal amount)
        {
            Balance += amount;
            LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            TransactionHistory.Add($"[{LastUpdated}] +${amount:F2} Deposit ");
        }
        public void Withdraw(decimal amount)
        {
            if (Balance < amount)
            {
                return;
            }
            Balance -= amount;
            LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            TransactionHistory.Add($"[{LastUpdated}] -${amount:F2} Withdrawal ");
        }
        public bool TransferTo(BankAccount other, decimal amount)
        {
            if (Balance < amount)
            {
                return false;
            }
            // How we handle the transfer between 2 accounts
            Balance -= amount;
            other.Balance += amount;
            LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            TransactionHistory.Add($"[{LastUpdated}] -${amount:F2} Transfer to {other.AccountNumber} ");
            other.TransactionHistory.Add($"[{other.LastUpdated}] +${amount:F2} Transfer from {AccountNumber} ");
            return true;
        }

        public BankAccount(string _name, string _username, string _pin, string _accountNumber)
        {
            Name = _name;
            Username = _username;
            Pin = _pin;
            AccountNumber = _accountNumber;
            TimeCreated = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }
    class BankAccountManagement
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

            Console.WriteLine("\nDeposit succesful!");
            OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${bankAccount.Balance}\n");
            OutputHelpers.KeyContinue();
        }
        public void ToWithdraw(BankAccount bankAccount)
        {
            Headers.WithdrawUI();
            decimal amount = InputHelper.GetDec("Enter amount to withdraw: ");
            bankAccount.Withdraw(amount);

            Console.WriteLine("\nWithdrawal succesful!");
            OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${bankAccount.Balance}\n");
            OutputHelpers.KeyContinue();
        }

        public void MakeTransfer(BankAccount bankAccount)
        {
            Headers.TransferUI();
            string otherAccount = InputHelper.GetString("Enter recipient account number: ", "Invalid account number:");
            decimal amount = InputHelper.GetDec("Enter amount to withdraw: ");

            bool notFound = true;
            foreach (BankAccount account in accounts)
            {
                if (account.Name == otherAccount)
                {
                    BankAccount other = account;
                    account.TransferTo(other, amount);
                    Console.WriteLine("\nTransfer succesful!");
                    OutputHelpers.ColorNew(); Console.WriteLine($"balance: ${bankAccount.Balance}\n");
                    OutputHelpers.KeyContinue();
                    notFound = false;
                }
            }
            if (notFound)
            {
                OutputHelpers.ColorError(); Console.WriteLine("Account number not found.");
                OutputHelpers.KeyContinue();
            }
        }

        public void ShowTransactionHistory(BankAccount bankAccount)
        {
            Headers.HistoryUI();
            Console.WriteLine(string.Join("\n",bankAccount.TransactionHistory));
            Headers.line2();
            OutputHelpers.KeyContinue();
        }

        public void ShowAccountDetails(BankAccount bankAccount)
        {

        }
    }
}
