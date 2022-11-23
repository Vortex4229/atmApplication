using System.ComponentModel;
using System.Numerics;

namespace atmApplication
{
    internal class Primary
    {
        public static List<BankAccount> bankAccounts = new List<BankAccount>();

        static void Main(string[] args)
        {
            bankAccounts.Add(new BankAccount("4229", "1234-5678-9123-4567", "422", "04/09", "Paulo Bello", 1000000));

            Console.WriteLine("Welcome to the Vortexian ATM Software, partnered with the HNT8 Banking System.");
            bool stop = false;
            while (!stop)
            {
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("Welcome to the Vortexian ATM Software, partnered with the HNT8 Banking System. Press 1 to access account or press 9 to terminate program.");
                ConsoleKey userinput = Console.ReadKey().Key;

                if (userinput == ConsoleKey.D1 || userinput == ConsoleKey.NumPad1)
                {
                    // Will find account based off card number and ask for bank account info. Runs through the accountAuth method (see line 39).
                    Console.Clear();
                    Console.WriteLine("Please enter your card number. (Format: XXXX-XXXX-XXXX-XXXX)");
                    string enteredCardNumber = readLineHidden();

                    BankAccount bankAccount = bankAccounts.Where(ba => ba.checkNumber(enteredCardNumber)).FirstOrDefault();

                    if (bankAccount == null)
                    {
                        Console.WriteLine("That card number does not exist.");
                        continue;
                    }
                    
                    Console.Clear();
                    Console.WriteLine("Please enter your legal first and last name.");
                    string enteredName = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Please enter your experation date. (Format: MM/YY)");
                    string enteredExperation = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Please enter your CVV number.");
                    string enteredCVV = readLineHidden();
                    Console.Clear();
                    Console.WriteLine("Finally, please enter your pin number.");
                    string enteredPin = readLineHidden();
                    Console.Clear();

                    bool accountAuthorization = bankAccount.accountAuth(enteredPin, enteredCardNumber, enteredCVV, enteredExperation, enteredName);

                    if (accountAuthorization == false)
                    {
                        Console.WriteLine("That information is invalid.");
                        continue;
                    }

                    bool stop2 = false;
                    while (!stop2)
                    {
                        Console.Clear();
                        Console.WriteLine("Welcome to your account, " + enteredName + ".");
                        Thread.Sleep(2000);
                        Console.WriteLine("To check your balence, press 1. To make a withdraw, press 2. To change pin number, press 3. To go back to home, press 4.");
                        userinput = Console.ReadKey().Key;

                        if (userinput == ConsoleKey.D1 || userinput == ConsoleKey.NumPad1)
                        {
                            Console.Clear();
                            Console.WriteLine("Current balence: $" + bankAccount.getBal());
                            continue;
                        }
                        
                        if (userinput == ConsoleKey.D2 || userinput == ConsoleKey.NumPad2)
                        {
                            // Needs finishing, ask Hunter for help as he made the god forsaken system.
                            Console.Write("Please enter the amount you would like to withdraw: ");
                            string enteredWithdraw = Console.ReadLine();
                        }
                        
                        if (userinput == ConsoleKey.D3 || userinput == ConsoleKey.NumPad3)
                        {
                            Console.Clear();
                            string numberPin = bankAccount.pinNumber;
                            Console.Write("Please enter original pin number: ");
                            string originalPin = readLineHidden();

                            if (originalPin != numberPin)
                            {
                                Console.Clear();   
                                Console.WriteLine("That is the incorrect pin.");
                                continue;
                            }

                            Console.Clear();
                            Console.Write("Please enter your desired pin number: ");
                            string newPin = readLineHidden();
                            Console.Clear();
                            Console.Write("Please confirm your new pin number by entering it again: ");
                            string newPin2 = readLineHidden();
                            
                            if (newPin != newPin2)
                            {
                                Console.Clear();
                                Console.WriteLine("The pin number you entered is not the same as your desired pin.");
                                continue;
                            }

                            bankAccount.changePin(numberPin, newPin);
                            Console.Clear();
                            Console.WriteLine("Your new pin has been set.");
                            Thread.Sleep(2000);
                        }
                        
                        if (userinput == ConsoleKey.D4 || userinput == ConsoleKey.NumPad4)
                        {
                            Console.Clear();
                            Console.Write("Returning to home screen");
                            Console.Write(" .");
                            Thread.Sleep(1000);
                            Console.Write(" .");
                            Thread.Sleep(1000);
                            Console.Write(" .");
                            Thread.Sleep(1000);
                            break;
                        }            
                    }
                }
                else if (userinput == ConsoleKey.D9 || userinput == ConsoleKey.NumPad9)
                {
                    Console.Clear();
                    Console.Write("Terminating program");
                    Console.Write(" .");
                    Thread.Sleep(1000);
                    Console.Write(" .");
                    Thread.Sleep(1000);
                    Console.Write(" .");
                    Thread.Sleep(1000);
                    stop = true;
                }
            }
        }
        //Peter's Code 
        public static string readLineHidden()
        {
            bool cont = true;
            Stack<string> stack = new Stack<string>();
            while (cont)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                ConsoleKey ck = cki.Key;
                if (ck == ConsoleKey.Enter)
                {
                    cont = false;
                    if (stack.Count > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write("*");
                    }
                    Console.WriteLine("");
                }
                else if (ck == ConsoleKey.Backspace)
                {
                    if (stack.Count == 0)
                    {
                        continue;
                    }
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    stack.Pop();
                }
                else
                {
                    char input = cki.KeyChar;
                    if (input < 32)
                    {
                        continue;
                    }
                    if (stack.Count > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write("*");
                    }
                    Console.Write(input);
                    stack.Push(cki.KeyChar.ToString());
                }
            }
            string[] arr = stack.ToArray();
            Array.Reverse(arr);
            return string.Join(string.Empty, arr);
        }
    }

    internal class BankAccount
    {
        public string pinNumber { get; set; }
        private string cardNumber { get; }
        private string cvvNumber { get; }
        private string experationDate { get; }
        private string fullName { get; }
        private float accountBalance { get; set; }

        public bool accountAuth(string givenPin, string givenCardNumber, string givenCVV, string givenExperation, string givenName)
        {
            if (givenPin != pinNumber) return false;
            if (givenCardNumber != cardNumber) return false;
            if (givenCVV != cvvNumber) return false;
            if (givenExperation != experationDate) return false;
            if (givenName != fullName) return false;
            return true;
        }

        public float getBal() { return accountBalance; }

        public Withdraw withdrawMoney(float withdrawAmount)
        {
            Withdraw withdraw = new Withdraw(withdrawAmount, getBal());
            if (!withdraw.invalid)
            {
                accountBalance -= withdrawAmount;
                return withdraw;
            }
            else
            {
                return new Withdraw(0, accountBalance);
            }
        }

        public void changePin(string pinNumber, string newPinNumber)
        {
            pinNumber = newPinNumber;
        }

        public bool checkNumber(string number)
        {
            return cardNumber.Equals(number);
        }

        public BankAccount(string pinNumber, string cardNumber, string cvvNumber, string experationDate, string fullName, float accountBalance)
        {
            this.pinNumber = pinNumber;
            this.cardNumber = cardNumber;
            this.cvvNumber = cvvNumber;
            this.experationDate = experationDate;
            this.fullName = fullName;
            this.accountBalance = accountBalance;
        }
    }

    internal class Withdraw
    {
        public float withdrawAmount { get; }
        public bool invalid { get; } = true;

        public Withdraw(float withdrawAmount, float balance)
        {
            if (withdrawAmount <= balance && withdrawAmount > 0 && withdrawAmount <= 1000) { invalid = false; }
        }
    }
}