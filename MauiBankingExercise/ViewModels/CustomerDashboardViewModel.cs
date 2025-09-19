using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiBankingExercise.Models;
using MauiBankingExercise.Interface;
using Microsoft.Maui.Dispatching;

namespace MauiBankingExercise.ViewModels
{
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    public class CustomerDashboardViewModel : BaseViewModel
    {
        private Customer? _customer;
        private int _customerId;
        private ObservableCollection<Account> _customerAccounts = new();
        private ObservableCollection<Transaction> _recentTransactions = new();
        private Account? _selectedAccount;
        private bool _showAllTransactions;

        private readonly IBankingService _bankingService;

        public ICommand AccountSelectedCommand { get; }
        public ICommand ToggleTransactionsCommand { get; }
        public ICommand RefreshCommand { get; }

        public int CustomerId
        {
            get => _customerId;
            set
            {
                _customerId = value;
                OnPropertyChanged();
                _ = LoadCustomerDataAsync(); // fire and forget, still fine here
            }
        }

        public Customer? Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Account> CustomerAccounts
        {
            get => _customerAccounts;
            set
            {
                _customerAccounts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Transaction> RecentTransactions
        {
            get => _recentTransactions;
            set
            {
                _recentTransactions = value;
                OnPropertyChanged();
            }
        }

        public Account? SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
                if (_selectedAccount != null)
                {
                    _ = NavigateToTransactionViewAsync();
                }
            }
        }

        public bool ShowAllTransactions
        {
            get => _showAllTransactions;
            set
            {
                _showAllTransactions = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransactionsDisplayText));
                _ = LoadRecentTransactionsAsync();
            }
        }

        public string TransactionsDisplayText => ShowAllTransactions ? "Show Recent (Last 10)" : "Show All Transactions";

        public decimal TotalBalance => CustomerAccounts?.Sum(a => a.AccountBalance) ?? 0;

        public CustomerDashboardViewModel(IBankingService bankingService)
        {
            _bankingService = bankingService;
            AccountSelectedCommand = new Command(async _ => await NavigateToTransactionViewAsync());
            ToggleTransactionsCommand = new Command(() => ShowAllTransactions = !ShowAllTransactions);
            RefreshCommand = new Command(async () => await LoadCustomerDataAsync());
        }

        private async Task LoadCustomerDataAsync()
        {
            if (CustomerId > 0)
            {
                try
                {
                    Customer = await _bankingService.GetCustomersById(CustomerId);
                    await LoadCustomerAccountsAsync();
                    await LoadRecentTransactionsAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading customer: {ex.Message}");
                }
            }
        }

        private async Task LoadCustomerAccountsAsync()
        {
            try
            {
                var accounts = await _bankingService.GetAccountByCustomerId(CustomerId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CustomerAccounts.Clear();

                    // Handle case where service returns a single account or a list
                    if (accounts is IEnumerable<Account> list)
                    {
                        foreach (var account in list)
                            CustomerAccounts.Add(account);
                    }
                    else if (accounts is Account singleAccount)
                    {
                        CustomerAccounts.Add(singleAccount);
                    }

                    OnPropertyChanged(nameof(TotalBalance));
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading accounts: {ex.Message}");
            }
        }

        private async Task LoadRecentTransactionsAsync()
        {
            try
            {
                var allTransactions = await _bankingService.GetTransactionsByCustomerId(CustomerId);

                var sortedTransactions = allTransactions
                    .OrderByDescending(t => t.TransactionDate);

                var transactionsToShow = ShowAllTransactions
                    ? sortedTransactions
                    : sortedTransactions.Take(10);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RecentTransactions.Clear();
                    foreach (var t in transactionsToShow)
                    {
                        RecentTransactions.Add(t);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        private async Task NavigateToTransactionViewAsync()
        {
            if (SelectedAccount != null)
            {
                var parameters = new ShellNavigationQueryParameters
                {
                    { "AccountId", SelectedAccount.AccountId },
                    { "CustomerId", CustomerId }
                };
                await AppShell.Current.GoToAsync("updateTransactionroute", parameters);
            }
        }

        public override async Task OnAppearingAsync()
        {
            SelectedAccount = null;

            if (CustomerId > 0)
            {
                await LoadCustomerDataAsync();
            }
        }
    }
}