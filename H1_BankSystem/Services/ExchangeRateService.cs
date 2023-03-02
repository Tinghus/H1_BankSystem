using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_BankSystem.Services
{
    public static class ExchangeRateService
    {
        private static readonly Dictionary<string, decimal> ExchangeRates = new Dictionary<string, decimal>()
        {
            { "DKK_EUR", 7.5m },
            { "DKK_USD", 6.0m },
            { "EUR_DKK", 7.5m / 1.0m },
            { "EUR_USD", 0.85m },
            { "USD_DKK", /1.0m / 6.0m },
            { "USD_EUR", 1.0m / 0.85m },
        };

        public static decimal ConvertCurrency(AccountService.AccountData.CurrencyType fromCurrency, AccountService.AccountData.CurrencyType toCurrency, decimal amount)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            string key = $"{fromCurrency}_{toCurrency}";

            decimal rate = ExchangeRates[key];
            return amount * rate;
        }
    }
}
