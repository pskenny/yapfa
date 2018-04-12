using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace Yapfa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Account> accountsList = new ObservableCollection<Account>();
        ObservableCollection<Transaction> transactionsList = new ObservableCollection<Transaction>();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            transactionsList.CollectionChanged += TransactionListChange;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO load settings
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            DisplayStartupDialog();

            // Set item sources
            AccountTable.ItemsSource = accountsList;
            TransactionsTable.ItemsSource = transactionsList;

            UpdateChart();
        }

        private async void DisplayStartupDialog()
        {
            ContentDialog subscribeDialog = new ContentDialog
            {
                Title = "Hello",
                Content = "What do you want to do?",
                CloseButtonText = "Empty",
                PrimaryButtonText = "Load File",
                SecondaryButtonText = "Load Sample Data",
                DefaultButton = ContentDialogButton.Secondary
            };

            ContentDialogResult result = await subscribeDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // TODO open dialog, open serialised data
            }
            else if (result == ContentDialogResult.Secondary)
            {
                // Load categories, payees, accounts and transactions
                LoadSampleData();
            }
        }

        /// <summary>
        /// Load accounts, categories, payees and transactions
        /// </summary>
        private void LoadSampleData()
        {
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
                InitialBalance = 200M,
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
                Amount = 1000,
                Memo = "Work"
            };
            Transaction tr2 = new Transaction()
            {
                Account = account2.Name,
                Date = new DateTime(),
                Payee = "Landlord",
                Category = "Rent",
                Amount = -450M,
                Memo = "Rent"
            };
            Transaction tr3 = new Transaction()
            {
                Account = account2.Name,
                Date = new DateTime(),
                Payee = "Eye Cinema",
                Category = "Film",
                Amount = -8M,
                Memo = "Ready Player One"
            };
            Transaction tr4 = new Transaction()
            {
                Account = account1.Name,
                Date = new DateTime(),
                Payee = "Me",
                Category = "Unexpected Income",
                Amount = 2M,
                Memo = "Found money"
            };
            Transaction tr5 = new Transaction()
            {
                Account = account1.Name,
                Date = new DateTime(),
                Payee = "Shop",
                Category = "Drink",
                Amount = -2M,
                Memo = "Water"
            };

            AddTransaction(tr1);
            AddTransaction(tr2);
            AddTransaction(tr3);
            AddTransaction(tr4);
            AddTransaction(tr5);
        }

        void AddTransaction(Transaction transaction)
        {
            transactionsList.Add(transaction);

            for (var i = 0; i < accountsList.Count; i++)
            {
                // Add transaction amount to account with same name
                if (accountsList[i].Name == transaction.Account)
                {
                    accountsList[i].Balance += transaction.Amount;
                }
            }
        }

        private void UpdateChart()
        {
            // TODO Generate category totals for income/expenditure from transactions
            List<PieSegment> financialStuffList = new List<PieSegment>();

            foreach (Transaction transaction in transactionsList)
            {
                if (transaction.Amount < 0 && transaction.Category.Length > 0)
                {
                    // TODO check if category is already present, add to value if true
                    financialStuffList.Add(new PieSegment() { Name = transaction.Category, Amount = (int)transaction.Amount });
                }
            }

            (PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
        }

        private void TransactionListChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateChart();
        }

        private void AddAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            AddAccountFlyout.ShowAt(button);
        }

        private void RemoveAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            if (AccountTable.SelectedItem != null)
            {
                RemoveAccountFlyout.ShowAt(button);
            }
        }

        private void AddTransaction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            AddTransactionFlyout.ShowAt(button);
        }

        private void RemoveTransaction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            if (TransactionsTable.SelectedItem != null)
            {
                RemoveTransactionFlyout.ShowAt(button);
            }
        }

        private class PieSegment
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }
    }
}
