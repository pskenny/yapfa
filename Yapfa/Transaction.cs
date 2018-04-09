using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapfa
{
    class Transaction
    {
        public DateTime Date { get; set; }
        public Payee Payee { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
    }
}
