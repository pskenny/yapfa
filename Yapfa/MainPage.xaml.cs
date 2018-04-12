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
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Account> AccountsList = new ObservableCollection<Account>();
        private ObservableCollection<Transaction> TransactionsList = new ObservableCollection<Transaction>();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            TransactionsList.CollectionChanged += TransactionListChange;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayStartupDialog();

            // Set item sources
            AccountTable.ItemsSource = AccountsList;
            TransactionsTable.ItemsSource = TransactionsList;

            // Update chart initially
            UpdateChart();
        }

        private async void DisplayStartupDialog()
        {
            ContentDialog subscribeDialog = new ContentDialog
            {
                Title = "Hello",
                Content = "What do you want to do?",
                CloseButtonText = "Empty",
                PrimaryButtonText = "Load Sample Data",
                DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await subscribeDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Load categories, payees, accounts and transactions from sample data
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

            // Add test accounts
            AccountsList.Add(account1);
            AccountsList.Add(account2);

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

            // Add transactions
            AddTransaction(tr1);
            AddTransaction(tr2);
            AddTransaction(tr3);
            AddTransaction(tr4);
            AddTransaction(tr5);
        }

        void AddTransaction(Transaction transaction)
        {
            TransactionsList.Add(transaction);

            for (var i = 0; i < AccountsList.Count; i++)
            {
                // Add transaction amount to account with same name
                if (AccountsList[i].Name == transaction.Account)
                {
                    AccountsList[i].Balance += transaction.Amount;
                }
            }
        }

        private void UpdateChart()
        {
            List<PieSegment> financialStuffList = new List<PieSegment>();

            foreach (Transaction transaction in TransactionsList)
            {
                if (transaction.Amount < 0 && transaction.Category.Length > 0)
                {
                    // Add new pie segment for every encountered expense in category
                    financialStuffList.Add(new PieSegment() { Name = transaction.Category, Amount = (int)transaction.Amount });
                }
            }

            (PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
        }

        private void TransactionListChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update chart on transaction list change
            UpdateChart();
        }

        private void AddAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;
            // Show flyout
            AddAccountFlyout.ShowAt(button);
        }

        private void RemoveAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            // Make sure an account is selected
            if (AccountTable.SelectedItem != null)
            {
                // Show flyout
                RemoveAccountFlyout.ShowAt(button);
            }
        }

        private void RemoveAccountConfirmation_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selected = AccountTable.SelectedItem;
            // Remove flyout
            RemoveAccountFlyout.Hide();
        }

        private void AddTransaction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;
            // Show flyout
            AddTransactionFlyout.ShowAt(button);
        }

        private void RemoveTransaction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;

            // Make sure a transaction is selected
            if (TransactionsTable.SelectedItem != null)
            {
                // Show flyout
                RemoveTransactionFlyout.ShowAt(button);
            }
        }

        private void RemoveTransactionConfirmation_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selected = TransactionsTable.SelectedItem;
            // Hide flyout
            RemoveTransactionFlyout.Hide();
        }
        
        private class PieSegment
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }
    }
}
