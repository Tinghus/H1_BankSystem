using H1_BankSystem.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static H1_BankSystem.Services.AccountService;
using static H1_BankSystem.Services.UserService;

namespace H1_BankSystem.Services
{
    static class DataService
    {
        //References   

        private const string ConnectionString = "";

        public static List<UserClass> UserList = new List<UserClass>();
        public static List<AccountData> AccountList = new List<AccountData>();

        static DataService()
        {
            if (ConnectionString != "" && IsConnected(ConnectionString))
            {
                LoadData();
                return;
            }

            CreateDummyData();
            UserList = UserList.OrderBy(u => u.FirstName).ToList();
            AccountList = AccountList.OrderBy(a => a.Id).ToList(); // TODO remove if still uneeded later

        }

        private static bool IsConnected(string connectionString)
        {

            return false;
        }

        private static void LoadData()
        {

        }

        private static void BuildAccountReference(int userId, AccountService.AccountData accountToAdd)
        {
            foreach (UserClass user in UserList)
            {
                if (user.Id == userId)
                {
                    user.AccountList.Add(accountToAdd);
                }
            }
        }


        private static void CreateDummyData()
        {
            CreateDummyUsers();
            CreateDummyAccounts();
            CreateDummyTransfers();
        }


        private static void CreateDummyUsers()
        {
            UserList.Add(new UserClass
            {
                Id = 0,
                UserType = UserClass.UserTypes.AccountHolder,
                FirstName = "John",
                LastName = "Doe",
                Username = "cus1",
                Email = "johndoe@example.com",
                Birthday = new DateTime(1985, 6, 20),
                CreationDate = DateTime.Now
            }); ;
            UserList[0].SetPassword("pass1");


            UserList.Add(new UserClass
            {
                Id = 1,
                UserType = UserClass.UserTypes.AccountHolder,
                FirstName = "Alice",
                LastName = "Smith",
                Username = "cus2",
                Email = "alicesmith@example.com",
                Birthday = new DateTime(1990, 3, 15),
                CreationDate = DateTime.Now
            });
            UserList[1].SetPassword("pass1");

            UserList.Add(new UserClass
            {
                Id = 2,
                UserType = UserClass.UserTypes.AccountHolder,
                FirstName = "Bob",
                LastName = "Johnson",
                Username = "cos3",
                Email = "bobjohnson@example.com",
                Birthday = new DateTime(1980, 9, 5),
                CreationDate = DateTime.Now
            });
            UserList[2].SetPassword("pass1");

            UserList.Add(new UserClass
            {
                Id = 3,
                UserType = UserClass.UserTypes.Teller,
                FirstName = "Jane",
                LastName = "Teller",
                Username = "teller",
                Email = "janeteller@example.com",
                Birthday = new DateTime(1995, 2, 1),
                CreationDate = DateTime.Now
            });
            UserList[3].SetPassword("pass1");
        }

        private static void CreateDummyAccounts()
        {
            string[] accountNames = { "Budget", "Savings", "Travel", "House" };
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                AccountData account = new AccountData
                {
                    Id = i,
                    Name = accountNames[random.Next(0, accountNames.Length)],
                    AccountHolderId = random.Next(0, 3),
                    Balance = Convert.ToDecimal(random.Next(100, 1000) - 350),
                    Currency = (AccountData.CurrencyType)random.Next(0, 3)
                };

                BuildAccountReference(account.AccountHolderId, account);
                AccountList.Add(account);
            }
        }

        private static void CreateDummyTransfers()
        {

        }
    }
}
