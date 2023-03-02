using H1_BankSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H1_BankSystem.Classes
{
    public class UiClass
    {
        public UserService User { get; set; }
        public MenuClass Menu { get; set; }

        public void InsertHorizontalLine()
        {
            Console.WriteLine();

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();

        }

        public void ShowCustomerList(List<UserService.UserClass> users, int cursorPosTop, int startIndex)
        {
            Console.CursorTop = cursorPosTop;

            Console.Write(
                "Id".PadRight(6) +
                "Account Type".PadRight(16) +
                "Firstname".PadRight(12) +
                "Lastname".PadRight(12) +
                "Email".PadRight(26) +
                "Created".PadRight(16) +
                "Account Value"
            );

            Console.WriteLine();

            int endIndex = Math.Min(startIndex + 15, users.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                string output =
                    users[i].Id.ToString().PadRight(6) +
                    users[i].UserType.ToString().PadRight(16) +
                    users[i].FirstName.PadRight(12) +
                    users[i].LastName.PadRight(12) +
                    users[i].Email.PadRight(26) +
                    users[i].CreationDate.ToShortDateString().PadRight(16) +
                    User.GetTotalBalance(users[i]).ToString("0.00") +
                    " DKK";

                if (i == Menu.HoveredItem)
                {
                    ApplyEffect(output, Effect.ActiveMenuItem);
                }
                else
                {
                    Console.Write(output);
                }

                Console.WriteLine();
            }
        }

        public void ShowCustomerDetails(UserService.UserClass customer, int cursorPosTop)
        {
            Console.CursorTop = cursorPosTop;
            int padding = 16;

            Console.WriteLine("".PadRight(padding));

            Console.WriteLine("Id:".PadRight(padding) + customer.Id.ToString());
            Console.WriteLine("Account Type:".PadRight(padding) + customer.UserType.ToString());
            Console.WriteLine("Created Date:".PadRight(padding));
            Console.WriteLine("Username:".PadRight(padding) + customer.Username);
            Console.WriteLine("Email:".PadRight(padding) + customer.Email);

            Console.WriteLine();

            Console.WriteLine("Name:".PadRight(padding) + customer.FirstName + " " + customer.LastName);
            Console.WriteLine("Birthday".PadRight(padding) + customer.Birthday.ToShortDateString());
            Console.WriteLine("Account Value:".PadRight(padding) + User.GetTotalBalance(customer).ToString() + " DKK");

            Console.WriteLine();

            Console.WriteLine("Accounts:");
            ShowAccountListCustomer(customer, cursorPosTop + 12);

        }

        public void ShowAccountListCustomer(UserService.UserClass user, int cursorPosTop)
        {
            Console.Write(
                "Id".PadRight(10) +
                "Account Name".PadRight(14) +
                "Balance".PadRight(12) +
                "Account Created"
                );
        }

        public void ShowAccountListDetailed(UserService.UserClass loggedInAs, int cursorPosTop, int startIndex)
        {
            Console.CursorTop = cursorPosTop;

            Console.Write(
                "Id".PadRight(10) +
                "Account Holder".PadRight(24) +
                "Account Name".PadRight(14) +
                "Balance".PadRight(16) +
                "Account Created"
                );

            Console.WriteLine();

            List<AccountService.AccountData> AccountList = DataService.AccountList;
            int endIndex = Math.Min(startIndex + 15, AccountList.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (loggedInAs.UserType == UserService.UserClass.UserTypes.Teller)
                {
                    string output =
                        AccountList[i].Id.ToString().PadRight(10) +
                        User.GetUserFullName(AccountList[i].AccountHolderId).PadRight(24) +
                        AccountList[i].Name.PadRight(14) +
                        GetBalanceWithCurrency(AccountList[i]).PadRight(16) +
                        AccountList[i].CreationDate.ToShortDateString();

                    if (AccountList[i].Balance < 0 && i != Menu.HoveredItem)
                    {
                        ApplyEffect(output, Effect.NegativeBalance);
                        Console.WriteLine();
                    }
                    else if (i == Menu.HoveredItem)
                    {
                        ApplyEffect(output, Effect.ActiveMenuItem);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine(output);
                    }

                }
            }
        }

        public void ShowLoginForm()
        {
            int cursorTopStart = (Console.WindowHeight / 2) - 2;
            int cursorLeftStart = (Console.WindowWidth / 2) - 11;

            bool loginIsSuccesful = false;
            string username = "";
            string password = "";

            Console.CursorVisible = true;

            while (!loginIsSuccesful)
            {

                while (true)
                {
                    Console.CursorTop = cursorTopStart;
                    Console.CursorLeft = cursorLeftStart;

                    Console.WriteLine("Username: ");

                    Console.CursorLeft = cursorLeftStart;
                    Console.WriteLine("Password: ");

                    Console.CursorTop = cursorTopStart;
                    Console.CursorLeft = cursorLeftStart + 11;

                    username = Console.ReadLine();

                    Console.CursorLeft = cursorLeftStart + 11;

                    password = GetPassword();
                    if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
                    {
                        loginIsSuccesful = User.VerifyLogin(username, password);
                    }

                    if (loginIsSuccesful)
                    {
                        break;
                    }

                    Console.WriteLine("\n");
                    string errorText = "Login failed. Press a key to try again";
                    Console.CursorLeft = (Console.WindowWidth / 2) - (errorText.Length / 2);
                    ApplyEffect(errorText, Effect.Warning);
                    Console.ReadKey();
                    Console.Clear();
                };
            }
        }

        private string GetPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }

        private string GetBalanceWithCurrency(AccountService.AccountData account)
        {
            string leadingSign = account.Balance < 0 ? "" : "+";

            return ((leadingSign + account.Balance.ToString("0.00")).PadLeft(8) + " " + account.Currency.ToString());

            //return (account.Balance.ToString("0.00").PadLeft(padding) + " " + account.Currency.ToString());
        }

        public void ApplyEffect(string output, Effect effect)
        {
            ConsoleColor startingForegroundColor = Console.ForegroundColor;
            ConsoleColor startingBackgroundColor = Console.BackgroundColor;

            switch (effect)
            {
                case Effect.Warning:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case Effect.ActiveMenuItem:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case Effect.Username:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case Effect.NegativeBalance:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write(output);
            Console.ForegroundColor = startingForegroundColor;
            Console.BackgroundColor = startingBackgroundColor;
        }


        public enum Effect
        {
            Warning,
            ActiveMenuItem,
            Username,
            NegativeBalance,
        }

    }
}
