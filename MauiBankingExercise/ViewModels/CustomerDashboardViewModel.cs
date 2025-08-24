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
        private Account? _selectedAccount;

        public ICommand AccountSelectedCommand { get; }

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

        public CustomerDashboardViewModel(BankingDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AccountSelectedCommand = new Command(OnAccountSelected);
        }

        private void LoadCustomerData()
        {
            if (CustomerId > 0)
            {
                try
                {
                    Customer = _databaseService.GetCustomerById(CustomerId);
                    LoadCustomerAccounts();
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading accounts: {ex.Message}");
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

        public override void OnAppearing()
        {
            base.OnAppearing();
            SelectedAccount = null; 
            if (CustomerId > 0 && Customer == null)
            {
                LoadCustomerData();
            }
        }
    }
}