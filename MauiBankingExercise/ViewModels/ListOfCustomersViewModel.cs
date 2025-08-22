using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiBankingExercise.Services;
using MauiBankingExercise.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{
    public class ListOfCustomersViewModel : BaseViewModel
    {
        private BankingDatabaseService _databaseService;
        public ICommand CustomerSelectedCommand { get; set; }
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        private Customer? _selectedCustomer;

        public Customer? SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        public ObservableCollection<Customer> TheCustomers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public ListOfCustomersViewModel(BankingDatabaseService service)
        {
            _databaseService = service;

            //CustomerSelectedCommand = new Command(CustomerSelected);
        }

        //private async void CustomerSelected(object obj)
        //{
        //    if (SelectedCustomer != null)
        //    {
        //        var param = new ShellNavigationQueryParameters()
        //        {
        //            { "CustomerId", SelectedCustomer.CustomerId }
        //        };
        //        await AppShell.Current.GoToAsync("CustomerDashboardroute", param);
            //}

        //}
        public override void OnAppearing()
        {
            base.OnAppearing();

            SelectedCustomer = null;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _databaseService.GetAllCustomers();
                TheCustomers.Clear();
                foreach (var customer in customers)
                {
                    TheCustomers.Add(customer);
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"Error loading customers: {ex.Message}");
            }

        }
    }
}