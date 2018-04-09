using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapfa
{
    public class Account
    {
        public enum AccountType
        {
            Bank,
            Cash
        }
       public AccountType Type { get; set; }
        public string Currency { get; set; }
        public decimal InitialBalance { get; set; }
        public string Name { get; set; }

        public decimal Amount
        {
            get
            {
                return InitialBalance;
            }
        }

        ObservableCollection<Transaction> transactions;
    }
}
