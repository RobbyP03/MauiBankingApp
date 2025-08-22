//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MauiBankingExercise.Services;
//using MauiBankingExercise.Models;

//namespace MauiBankingExercise.ViewModels
//{
//    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
//    public class CustomerDashboardViewModel : BaseViewModel
//    {
//        private BankingDatabaseService _databaseService;
//        private Customer? _customer;
//        private int _customerId;

//        public int CustomerId
//        {
//            get => _customerId;
//            set
//            {
//                _customerId = value;
//                OnPropertyChanged();
//                LoadCustomerData();
//            }
//        }

//        public Customer? Customer
//        {
//            get => _customer;
//            set
//            {
//                _customer = value;
//                OnPropertyChanged();
//            }
//        }

//        public CustomerDashboardViewModel(BankingDatabaseService databaseService)
//        {
//            _databaseService = databaseService;
//        }

//        private void LoadCustomerData()
//        {
//            if (CustomerId > 0)
//            {
//                try
//                {
//                    Customer = _databaseService.GetCustomerById(CustomerId);
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error loading customer: {ex.Message}");
//                }
//            }
//        }

//        public override void OnAppearing()
//        {
//            base.OnAppearing();
//            if (CustomerId > 0 && Customer == null)
//            {
//                LoadCustomerData();
//            }
//        }
//    }
//}