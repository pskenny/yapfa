using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapfa
{
    class Transaction
    {
        public string Account { get; set; }
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
    }
}
