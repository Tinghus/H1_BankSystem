using H1_BankSystem.Classes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_BankSystem.Services
{

    public class UserService
    {
        public bool UserIsLoggedIn { get; set; } = false;
        public UserClass CurrentUser { get; set; }
        public MenuClass Menu { get; set; }

        public bool VerifyLogin(string username, string password)
        {

            foreach (UserClass user in DataService.UserList)
            {
                if (user.Username.ToLower() == username.ToLower())
                {
                    if (user.IsPasswordValid(password))
                    {
                        CurrentUser = user;
                        UserIsLoggedIn = true;
                        Menu.PrepareMenuItems(CurrentUser);

                        return true;
                    }
                    return false;
                }
            }

            return false;
        }

        public string GetUserFullName(int userId)
        {
            foreach (UserClass user in DataService.UserList)
            {
                if (user.Id == userId)
                {
                    return user.FirstName + " " + user.LastName;
                }
            }

            return "Unknown user";
        }

        public decimal GetTotalBalance(UserClass user)
        {
            decimal totalBalance = 0;

            if (user.AccountList.Count == 0)
            {
                return 0.00m;
            }

            foreach (AccountService.AccountData account in user.AccountList)
            {
                totalBalance += ExchangeRateService.ConvertCurrency(account.Currency, AccountService.AccountData.CurrencyType.DKK, account.Balance);
            }

            return Math.Round(totalBalance, 2);
        }

        public class UserClass
        {
            public int Id { get; set; }
            public UserTypes UserType { get; set; } = UserClass.UserTypes.AccountHolder;
            public string FirstName { get; set; } = "";
            public string LastName { get; set; }

            public string Username { get; set; }
            private string _password { get; set; }
            public string Email { get; set; }
            public DateTime Birthday { get; set; }
            public DateTime CreationDate { get; set; }
            public List<AccountService.AccountData> AccountList { get; set; } = new List<AccountService.AccountData>();

            public bool IsPasswordValid(string password)
            {
                return _password == password;
            }

            public bool SetPassword(string password)
            {
                _password = password;
                return true; // Can be used for validation
            }

            public enum UserTypes
            {
                Admin,
                Teller,
                AccountHolder,
            }


        }

    }

}
