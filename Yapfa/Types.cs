using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapfa
{
    class Category
    {
        string Name { get; set; }
    }

    class Payee
    {
         string Name { get; set; }
    }

    class Transaction
    {
        DateTime Date { get; set; }
        Payee Payee { get; set; }
        Category Category { get; set; }
        decimal Amount { get; set; }
        string Memo { get; set; }
    }
}
