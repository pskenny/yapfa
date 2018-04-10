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
        private decimal _balance;

        public enum AccountType
        {
            Bank,
            Cash
        }
        public AccountType Type { get; set; }
        public string Currency { get; set; }
        public decimal InitialBalance { get; set; }
        public string Name { get; set; }
        public decimal Balance {
            get { return _balance + this.InitialBalance; }
            set { this._balance = value;} }
    }
}
