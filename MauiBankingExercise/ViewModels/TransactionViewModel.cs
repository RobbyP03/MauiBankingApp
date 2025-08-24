using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{
    
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    public class TransactionViewModel : BaseViewModel
    {
        private BankingDatabaseService _databaseService;
        private Account? _selectedAccount;
        private int _accountId;
        private int _customerId;
        private decimal _transactionAmount;
        private string _selectedTransactionType;

        public ObservableCollection<string> TransactionTypes { get; set; } = new ObservableCollection<string> { "Deposit", "Withdrawal" };
        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        public int AccountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                OnPropertyChanged();
                LoadAccountAndTransactions();
            }
        }

        public int CustomerId
        {
            get => _customerId;
            set
            {
                _customerId = value;
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
            }
        }

        public decimal TransactionAmount
        {
            get => _transactionAmount;
            set
            {
                _transactionAmount = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTransactionType
        {
            get => _selectedTransactionType;
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
            //AddTransactionCommand = new Command(OnAddTransaction);
        }

        private void LoadAccountAndTransactions()
        {
            if (AccountId > 0)
            {
                try
                {
                    // Get the specific account
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
                    // Sort transactions by date (most recent first)
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

        //private async void OnAddTransaction()
        //{
        //    if (SelectedAccount == null)
        //    {
        //        await App.Current.MainPage.DisplayAlert("Error", "Account not found.", "OK");
        //        return;
        //    }

        //    if (TransactionAmount <= 0)
        //    {
        //        await App.Current.MainPage.DisplayAlert("Error", "Enter a valid amount greater than 0.", "OK");
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(SelectedTransactionType))
        //    {
        //        await App.Current.MainPage.DisplayAlert("Error", "Please select a transaction type.", "OK");
        //        return;
        //    }

        //    if (SelectedTransactionType == "Withdrawal" && TransactionAmount > SelectedAccount.AccountBalance)
        //    {
        //        await App.Current.MainPage.DisplayAlert("Error", "Insufficient balance for withdrawal.", "OK");
        //        return;
        //    }

        //    try
        //    {
        //        // Determine sign of transaction
        //        var amount = SelectedTransactionType == "Withdrawal" ? -TransactionAmount : TransactionAmount;
                
        //        var newTransaction = new Transaction
        //        {
        //            AccountId = SelectedAccount.AccountId,
        //            TransactionDate = DateTime.Now,
        //            Amount = amount,
        //            Description = SelectedTransactionType
        //        };

        //        _databaseService.AddTransaction(newTransaction);

        //        // Update account balance
        //        SelectedAccount.AccountBalance += amount;
        //        _databaseService.UpdateAccount(SelectedAccount);

        //        // Add to UI and refresh
        //        Transactions.Insert(0, newTransaction); // Add to top of list
        //        OnPropertyChanged(nameof(SelectedAccount));

        //        // Reset input
        //        TransactionAmount = 0;
        //        SelectedTransactionType = null;

        //        await App.Current.MainPage.DisplayAlert("Success", 
        //            $"{(amount > 0 ? "Deposit" : "Withdrawal")} of R{Math.Abs(amount):N2} completed successfully.", "OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        await App.Current.MainPage.DisplayAlert("Error", $"Transaction failed: {ex.Message}", "OK");
        //    }
        //}
    }
}