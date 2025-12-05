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
                // The Main menu interface 1-To create the account, 2- login to existing account, 3- Exit the program.
                Console.Clear();
                BankAccountManagement.Line();
                Console.WriteLine("{0,27}", "BANK SIMULATOR ");
                BankAccountManagement.Line();
                Console.WriteLine();

                string[] options = ["Create an account", "Log in", "Exit"];  // My way of looping and writing the choices possible for the user.

                for (int i = 0; i < options.Length; i++)
                {
                    BankAccountManagement.ColorNumber(i + 1);
                    Console.WriteLine($") {options[i]}");
                }

                // User input handling
                int choice = BankAccountManagement.GetInt("\nChoose an option: ", 1, 3);

                if (choice == 3)
                {
                    Console.WriteLine("\nGoodBye!");
                    break;
                }
                else
                {
                    switch (choice)
                    {
                        case 1: bankaccountmanagement.CreateAccount(); break;  // First off create account
                        case 2: bankaccountmanagement.Authentication(); break; // To connect to bank account
                    }
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
        public int AccountNumber { get; }

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

        public BankAccount(string _name, string _username, string _pin, int _accountNumber)
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
            while (true)
            {
                Console.Clear();
                Line();
                Console.WriteLine("{0,30}", "CREATE NEW ACCOUNT");
                Line();

                Console.Write("\nEnter your full name: ");
                string? name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))  // FullName handling
                {
                    ColorError();
                    Console.WriteLine("Please enter a valid name please");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }

                Console.Write("Choose a username: "); // Username handling
                string? username = Console.ReadLine();

                if (name == username)  // Username cannot be the same as name
                {
                    ColorError();
                    Console.WriteLine("Name cannot be the same as username");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }

                else if (string.IsNullOrWhiteSpace(username))  // Won't accept  nullable username
                {
                    ColorError();
                    Console.WriteLine("Please enter a valid username please");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }

                else  // Username cannot be duplicate. Has to be different.
                {
                    bool usernameTaken = false;
                    foreach (BankAccount account in accounts)
                    {
                        if (account.Username == username)
                        {
                            ColorError();
                            Console.WriteLine("Username already exists. Try another one.");
                            KeyContinue(); Console.ReadKey();
                            usernameTaken = true;
                            break;
                        }
                    }
                    if (usernameTaken)
                    {
                        continue;
                    }
                }
                // Pin Handling
                Console.Write("Create a 4-digit PIN: ");
                string? pinInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(pinInput) || pinInput.Length != 4 || !int.TryParse(pinInput, out int pin)) // take the int as a string example 0000
                {
                    ColorError();
                    Console.WriteLine("PIN must be exactly 4 digits");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }

                Console.Write("Confirm PIN: ");
                string? confirmPin = Console.ReadLine();
                if (!int.TryParse(confirmPin, out int intPin) || confirmPin != pinInput)  // To see if the pins are the same
                {
                    ColorError();
                    Console.WriteLine("PINs do not match.");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }

                string accountnumber = "";
                Random rand = new Random();
                for (int i = 0; i < 9; i++)
                {
                    accountnumber += rand.Next(0, 10);  // To also accepts 0 as the first nummber possible
                }
                accounts.Add(new BankAccount(name, username, pinInput, int.Parse(accountnumber)));


                Console.WriteLine("\nAccount created successfully! 🎉");
                Console.WriteLine($"Your account number: {accountnumber}");
                Console.Write("\nPress any key to return to the main menu..");

                Console.ReadKey();
                break;
            }
        }
        public void ToDeposit(BankAccount account)
        {
            Console.Clear();
            Console.WriteLine("-------------- DEPOSIT --------------");
            Console.Write("Enter amount to deposit: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                account.Deposit(amount);
                Console.WriteLine("\nDeposit succesful!");
                ColorNew(); Console.WriteLine($"balance: ${account.Balance}\n");
                KeyContinue(); Console.ReadKey();
            }
            else
            {
                ColorError();
                Console.WriteLine("Invalid amount");
                KeyContinue(); Console.ReadKey();
            }
        }

        public void ToWithdraw(BankAccount account)
        {
            Console.Clear();
            Console.WriteLine("-------------- WITHDRAW --------------");
            Console.Write("Enter amount to withdraw: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                ColorError();
                Console.WriteLine("Invalid amount.");
                KeyContinue(); Console.ReadKey();
            }
            else if (amount > account.Balance)
            {
                ColorError();
                Console.WriteLine("Insufficient balance.");
                KeyContinue(); Console.ReadKey();
            }
            else
            {
                account.Withdraw(amount);
                Console.WriteLine("\nWithdrawal  succesful!");
                ColorNew(); Console.WriteLine($"balance: ${account.Balance}\n");
                KeyContinue(); Console.ReadKey();
            }

        }

        public void MakeTransfer(BankAccount account)
        // Code should work just fine now.
        // If wrong, goes back to dashboard.
        // 1. Handles invalid input like 'abc' or empty input.
        // 2. Verify account number existing.
        // 3. Handles amount you enter (if its possible, if balance is higher than amount)
        {
            Console.Clear();
            Console.WriteLine("------------- TRANSFER -------------");

            Console.Write("Enter recipient account number: ");
            BankAccount? otheraccount = null;

            if (!int.TryParse(Console.ReadLine(), out int otheraccountnumber)) // 1. Handling.
            {
                ColorError();
                Console.WriteLine("Invalid account number:");
                KeyContinue(); Console.ReadKey();
            }
            else
            {
                bool notFound = true;
                foreach (BankAccount bankAccount in accounts)
                {
                    if (bankAccount.AccountNumber == otheraccountnumber)  // 2. Searching for accountnumber in list
                    {
                        otheraccount = bankAccount;
                        notFound = false;
                    }
                }
                if (notFound)  // - true or false (find account number)
                {
                    ColorError();
                    Console.WriteLine("Account number not found.");
                    KeyContinue(); Console.ReadKey();
                }
                else
                {

                    Console.Write("Enter amount to transfer: "); // 3. Amount to transfer handling
                    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                    {
                        ColorError();
                        Console.WriteLine("Invalid amount:");
                        KeyContinue(); Console.ReadKey();
                    }
                    else if (amount > account.Balance)
                    {
                        ColorError();
                        Console.WriteLine("Insufficient balance.");
                        KeyContinue(); Console.ReadKey();
                    }
                    else
                    {
                        account.TransferTo(otheraccount, amount); // Successful transfer - call for the handling of transfer.
                        Console.WriteLine("\nTransfer succesful!");
                        ColorNew();
                        Console.WriteLine($"Balance: $ {account.Balance}\n");
                        KeyContinue(); Console.ReadKey();
                    }
                }
            }
        }

        public void ShowTransactionHistory(BankAccount account)
        {
            Console.Clear();
            Console.WriteLine("========== TRANSACTION HISTORY ==========\n");
            foreach (string transaction in account.TransactionHistory)
            {
                Console.WriteLine(transaction);
            }
            Console.WriteLine("\n=========================================");
            KeyContinue(); Console.ReadKey();
        }
        public void ShowAccountDetails(BankAccount account)
        {
            Console.Clear();
            Console.WriteLine("========== ACCOUNT DETAILS ==========\n");
            Console.WriteLine(account.Description);
            Console.WriteLine("\n======================================");
            KeyContinue(); Console.ReadKey();
        }
        public void DashBoard(BankAccount bankAccount)
        {
            while (true)
            {
                Console.Clear();
                Line();
                Console.WriteLine("{0,25}", "DASHBOARD");
                Line();

                string[] options = ["Deposit money", "Withdraw money", "Transfer money", "Transaction history", "Account details", "Logout"];
                Console.WriteLine($"Account Holder: {bankAccount.Name}");
                Console.WriteLine($"Account Number: {bankAccount.AccountNumber}");
                Console.WriteLine($"Balance: ${bankAccount.Balance}\n");
                line2();
                for (int i = 0; i < options.Length; i++)
                {
                    ColorNumber(i + 1);
                    Console.WriteLine($") {options[i]}");
                }
                line2();

                Console.Write("\nChoose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice) || (choice < 1 || choice > 6))
                {
                    ColorError();
                    Console.WriteLine("Invalid option.");
                    continue;
                }
                else if (choice == 6)
                {
                    Console.WriteLine("Logging out...");
                    Console.Write("Press any key to return to the main menu.");
                    Console.ReadKey();
                    return;
                }
                else
                {
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
        }

        public void Authentication() // Authentication Control system
                                     // To do:  if there are no account created in the bank flag error.
        {
            int tries = 3;
            BankAccount? currentAccount = null;

            if (accounts.Count == 0)
            {
                return;  // No account created yet
            }

            while (true)
            {
                LoginInterface();

                Console.Write("Enter username: ");
                string? username = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username))
                {
                    ColorError();
                    Console.WriteLine("Please enter a valid username");
                    KeyContinue(); Console.ReadKey();
                    continue;
                }
                else
                {
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
                        ColorError();
                        Console.WriteLine("Username not found.");
                        KeyContinue(); Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        break;

                    }
                }
            }

            while (tries > 0)
            {

                Console.Write("Enter PIN: ");
                string? pin = Console.ReadLine();

                if (!int.TryParse(pin, out int pinInput) || pin != currentAccount?.Pin)
                {
                    ColorError();
                    tries--;
                    if (tries == 0)
                    {
                        Console.Write("Account locked due to too many failed attempts.\n" +
                        "Contact support or try again later.\n");
                        KeyContinue(); Console.ReadKey(); return;

                    }
                    Console.WriteLine("Incorrect PIN.\n" +
                    $"Attempts remaining: {tries}");
                    KeyContinue(); Console.ReadLine();
                    Console.Clear();
                    LoginInterface();
                    continue;
                }
                else
                {
                    Console.WriteLine($"Login succesful! Welcome back, {currentAccount.Name}.\n");
                    KeyContinue(); Console.ReadKey();
                    DashBoard(currentAccount);
                    return;

                }
            }
        }
        public static void Line()
        {
            Console.WriteLine("=========================================");
        }
        public static void line2()
        {
            Console.WriteLine("-----------------------------------------");
        }
        public static void ColorNumber(int num)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{num}");
            Console.ResetColor();
        }
        public static void ColorError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nError: ");
            Console.ResetColor();
        }
        public static void KeyContinue()
        {
            Console.Write("\nPress any key to continue...");
        }
        public static void ColorNew()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("New ");
            Console.ResetColor();
        }
        public static void LoginInterface()
        {
            Console.Clear();
            Line();
            Console.WriteLine("{0, 23}", "LOGIN");
            Line();
        }
        public static int GetInt(string message, int min, int max)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int value) || (value < min || value > max))
                {
                    ColorError();
                    Console.WriteLine($"Please enter a number between {min} and {max}.");
                }
                else
                {
                    return value;
                }
            }
        }
    }
}
