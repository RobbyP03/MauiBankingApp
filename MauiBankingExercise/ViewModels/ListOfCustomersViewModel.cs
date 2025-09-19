using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiBankingExercise.Models;
using MauiBankingExercise.Interface;
using Microsoft.Maui.Dispatching; // for MainThread

namespace MauiBankingExercise.ViewModels
{
    public class ListOfCustomersViewModel : BaseViewModel
    {
        private readonly IBankingService _bankingService;

        public ICommand CustomerSelectedCommand { get; }

        private ObservableCollection<Customer> _customers = new();
        private Customer? _selectedCustomer;

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> TheCustomers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public ListOfCustomersViewModel(IBankingService bankingService)
        {
            _bankingService = bankingService;
            CustomerSelectedCommand = new Command(async obj => await CustomerSelectedAsync());
        }

        private async Task CustomerSelectedAsync()
        {
            if (SelectedCustomer != null)
            {
                var param = new ShellNavigationQueryParameters()
                {
                    { "CustomerId", SelectedCustomer.CustomerId }
                };

                // Navigate asynchronously to customer dashboard
                await AppShell.Current.GoToAsync("CustomerDashboardroute", param);
            }
        }

        public override async Task OnAppearingAsync()
        {
            SelectedCustomer = null;
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                var customers = await _bankingService.GetAllCustomers();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TheCustomers.Clear();
                    foreach (var customer in customers)
                    {
                        TheCustomers.Add(customer);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading customers: {ex.Message}");
            }
        }
    }
}
