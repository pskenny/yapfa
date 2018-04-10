using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Yapfa
{
    public class FinancialStuff
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Account> accountsList;
        ObservableCollection<Transaction> transactionsList;


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Load categories, payees, accounts and transactions
            LoadData();

            // Set item sources
            AccountTable.ItemsSource = accountsList;
            TransactionsTable.ItemsSource = transactionsList;

            LoadChartContents();
        }

        /// <summary>
        /// Load accounts, categories, payees and transactions
        /// </summary>
        private void LoadData()
        {
            accountsList = new ObservableCollection<Account>();
            transactionsList = new ObservableCollection<Transaction>();

            // Load test data if in debug mode
#if DEBUG
            // Make test accounts
            Account account1 = new Account()
            {
                Name = "Wallet",
                Type = Account.AccountType.Cash,
                InitialBalance = 3.50M,
                Currency = "Euro"
            };
            Account account2 = new Account()
            {
                Name = "Current Account",
                Type = Account.AccountType.Bank,
                InitialBalance = 200,
                Currency = "Euro"
            };

            accountsList.Add(account1);
            accountsList.Add(account2);

            // Make test transactions
            Transaction tr1 = new Transaction()
            {
                Account = account2.Name,
                Date = new DateTime(),
                Payee = "Employer Co.",
                Category = "Wage",
                Amount = (decimal) 1000,
                Memo = "Work"
            };

            AddTransaction(tr1);
#else
            // Load real data, previous instance if available
#endif
        }

        void AddTransaction(Transaction transaction)
        {
            transactionsList.Add(transaction);

            for (var i = 0; i < accountsList.Count; i++) {
                if(accountsList[i].Name == transaction.Account)
                {
                    accountsList[i].Balance += transaction.Amount;
                }
            }
        }

        private void LoadChartContents()
        {
            // TODO Generate category totals for income/expenditure from transactions
            Random rand = new Random();
            List<FinancialStuff> financialStuffList = new List<FinancialStuff>();
            financialStuffList.Add(new FinancialStuff() { Name = "Name1", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "Name2", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "Name3", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "Name4", Amount = rand.Next(0, 200) });
            (PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
        }
    }
}
