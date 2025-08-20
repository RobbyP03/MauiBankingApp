using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiBankingExercise.Services;
using MauiBankingExercise.Models;
using System.Collections.ObjectModel;

namespace MauiBankingExercise.ViewModels
{
    public class ListOfCustomersViewModel : BaseViewModel
    {
        private BankingSeeder _databaseSeeder;

        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        private Customer? _selectedCustomer;

        public Customer? SelectedCusomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCusomer));
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

        public ListOfCustomersViewModel(BankingSeeder service)
        { 
            _
        }

    }
}
