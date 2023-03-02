using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_BankSystem.Services
{
    public class AccountService
    {
        public class AccountData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AccountHolderId { get; set; }

            public decimal Balance { get; set; }
            public CurrencyType Currency { get; set; } = CurrencyType.DKK;
            public DateTime CreationDate { get; set; } = DateTime.Now;

            public enum CurrencyType
            {
                USD,
                DKK,
                EUR,
            }

        }
    }
}

