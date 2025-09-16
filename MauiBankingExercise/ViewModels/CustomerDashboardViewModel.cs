using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiBankingExercise.Services;
using MauiBankingExercise.Models;

namespace MauiBankingExercise.ViewModels
{
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    public class CustomerDashboardViewModel : BaseViewModel
    {
        private BankingDatabaseService _databaseService;
        private Customer? _customer;
        private int _customerId;
        private ObservableCollection<Account> _customerAccounts = new ObservableCollection<Account>();
        private ObservableCollection<Transaction> _recentTransactions = new ObservableCollection<Transaction>();
        private Account? _selectedAccount;
        private bool _showAllTransactions = false;

        public ICommand AccountSelectedCommand { get; }
        public ICommand ToggleTransactionsCommand { get; }
        public ICommand RefreshCommand { get; }

        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged();
                LoadCustomerData();
            }
        }

        public Customer? Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Account> CustomerAccounts
        {
            get { return _customerAccounts; }
            set
            {
                _customerAccounts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Transaction> RecentTransactions
        {
            get { return _recentTransactions; }
            set
            {
                _recentTransactions = value;
                OnPropertyChanged();
            }
        }

        public Account? SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
                if (_selectedAccount != null)
                {
                    NavigateToTransactionView();
                }
            }
        }

        public bool ShowAllTransactions
        {
            get { return _showAllTransactions; }
            set
            {
                _showAllTransactions = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransactionsDisplayText));
                LoadRecentTransactions();
            }
        }

        public string TransactionsDisplayText => ShowAllTransactions ? "Show Recent (Last 10)" : "Show All Transactions";

        public decimal TotalBalance
        {
            get
            {
                return CustomerAccounts?.Sum(a => a.AccountBalance) ?? 0;
            }
        }

        public CustomerDashboardViewModel(BankingDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AccountSelectedCommand = new Command(OnAccountSelected);
            ToggleTransactionsCommand = new Command(OnToggleTransactions);
            RefreshCommand = new Command(OnRefresh);
        }

        private void LoadCustomerData()
        {
            if (CustomerId > 0)
            {
                try
                {
                    Customer = _databaseService.GetCustomerById(CustomerId);
                    LoadCustomerAccounts();
                    LoadRecentTransactions();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading customer: {ex.Message}");
                }
            }
        }

        private void LoadCustomerAccounts()
        {
            try
            {
                var accounts = _databaseService.GetAccountsByCustomerId(CustomerId);
                CustomerAccounts.Clear();
                foreach (var account in accounts)
                {
                    CustomerAccounts.Add(account);
                }
                OnPropertyChanged(nameof(TotalBalance));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading accounts: {ex.Message}");
            }
        }

        private void LoadRecentTransactions()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading transactions for customer ID: {CustomerId}");

                // Use the new method to get all transactions for this customer
                var allTransactions = _databaseService.GetAllTransactionsByCustomerId(CustomerId);
                System.Diagnostics.Debug.WriteLine($"Found {allTransactions.Count} total transactions");

                // Sort by date (most recent first) and take appropriate number
                var sortedTransactions = allTransactions
                    .OrderByDescending(t => t.TransactionDate);

                var transactionsToShow = ShowAllTransactions ?
                    sortedTransactions :
                    sortedTransactions.Take(10);

                // Clear and rebuild the collection on the UI thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RecentTransactions.Clear();

                    foreach (var transaction in transactionsToShow)
                    {
                        System.Diagnostics.Debug.WriteLine($"Adding transaction: {transaction.Amount} on {transaction.TransactionDate} for account {transaction.AccountId}");
                        RecentTransactions.Add(transaction);
                    }

                    System.Diagnostics.Debug.WriteLine($"UI updated with {RecentTransactions.Count} transactions");
                });

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading transactions: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private async void OnAccountSelected(object parameter)
        {
            await NavigateToTransactionView();
        }

        private async Task NavigateToTransactionView()
        {
            if (SelectedAccount != null)
            {
                var parameters = new ShellNavigationQueryParameters()
                {
                    { "AccountId", SelectedAccount.AccountId },
                    { "CustomerId", CustomerId }
                };
                await AppShell.Current.GoToAsync("updateTransactionroute", parameters);
            }
        }

        private void OnToggleTransactions()
        {
            ShowAllTransactions = !ShowAllTransactions;
        }

        private void OnRefresh()
        {
            System.Diagnostics.Debug.WriteLine("Manual refresh triggered");
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadCustomerData();
            });
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            SelectedAccount = null;

            System.Diagnostics.Debug.WriteLine($"CustomerDashboard appearing for customer {CustomerId}");

            if (CustomerId > 0)
            {
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LoadCustomerData();
                });
            }
        }
    }
}