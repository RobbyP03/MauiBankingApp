using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    [QueryProperty(nameof(AccountId), nameof(AccountId))] 
    public class TransactionViewModel : BaseViewModel
    {
        private BankingDatabaseService _databaseService;
        private Account? _selectedAccount;
        private int _accountId;
        private int _customerId;
        private decimal _transactionAmount;
        private string _selectedTransactionType = string.Empty;

        public ObservableCollection<string> TransactionTypes { get; set; } = new ObservableCollection<string> { "Deposit", "Withdrawal" };
        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        public int AccountId
        {
            get { return _accountId; }
            set
            {
                _accountId = value;
                OnPropertyChanged();
                LoadAccountAndTransactions();
            }
        }

        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
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
            }
        }

        public decimal TransactionAmount
        {
            get { return _transactionAmount; }
            set
            {
                _transactionAmount = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTransactionType
        {
            get { return _selectedTransactionType; }
            set
            {
                _selectedTransactionType = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTransactionCommand { get; }

        public TransactionViewModel(BankingDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddTransactionCommand = new Command(OnAddTransaction);
            LoadTransactionTypes(); 
        }

        private void LoadTransactionTypes()
        {
            try
            {
                TransactionTypes.Clear();
                var types = _databaseService.GetAllTransactionTypes();
                foreach (var type in types)
                {
                    TransactionTypes.Add(type.Name);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading transaction types: {ex.Message}");
              
                TransactionTypes.Clear();
            
            }
        }

        private void LoadAccountAndTransactions()
        {
            if (AccountId > 0 && CustomerId > 0)
            {
                try
                {
                    var accounts = _databaseService.GetAccountsByCustomerId(CustomerId);
                    SelectedAccount = accounts.FirstOrDefault(a => a.AccountId == AccountId);

                    if (SelectedAccount != null)
                    {
                        LoadTransactions();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading account: {ex.Message}");
                }
            }
        }

        private void LoadTransactions()
        {
            try
            {
                Transactions.Clear();
                if (SelectedAccount?.Transactions != null)
                {
                    var sortedTransactions = SelectedAccount.Transactions
                        .OrderByDescending(t => t.TransactionDate);

                    foreach (var transaction in sortedTransactions)
                    {
                        Transactions.Add(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        private async void OnAddTransaction()
        {
            if (SelectedAccount == null || TransactionAmount <= 0 || string.IsNullOrEmpty(SelectedTransactionType))
            {
                // Show error message to user
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid amount and select transaction type.", "OK");
                return;
            }

            try
            {
                // Check for sufficient funds on withdrawal
                if (SelectedTransactionType == "Withdrawal" && TransactionAmount > SelectedAccount.AccountBalance)
                {
                    await Application.Current.MainPage.DisplayAlert("Insufficient Funds",
                        $"Cannot withdraw ${TransactionAmount:F2}. Available balance: ${SelectedAccount.AccountBalance:F2}", "OK");
                    return;
                }

                // Get the TransactionType from database based on the name
                var transactionType = _databaseService.GetTransactionTypeByName(SelectedTransactionType);
                if (transactionType == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Transaction type not found: {SelectedTransactionType}");
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid transaction type.", "OK");
                    return;
                }

                var transaction = new Transaction
                {
                    AccountId = AccountId,
                    Amount = SelectedTransactionType == "Withdrawal" ? -TransactionAmount : TransactionAmount,
                    TransactionDate = DateTime.Now,
                    TransactionTypeId = transactionType.TransactionTypeId,
                    Description = $"{SelectedTransactionType} of R{TransactionAmount:F2}"
                };

                _databaseService.AddTransaction(transaction);

                SelectedAccount.AccountBalance += transaction.Amount;
                _databaseService.UpdateAccount(SelectedAccount);

               
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Transaction completed successfully!\nNew balance: R{SelectedAccount.AccountBalance:F2}", "OK");

                LoadAccountAndTransactions();

                
                TransactionAmount = 0;
                SelectedTransactionType = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding transaction: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to process transaction. Please try again.", "OK");
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if (AccountId > 0 && CustomerId > 0)
            {
                LoadAccountAndTransactions();
            }
        }
    }
}