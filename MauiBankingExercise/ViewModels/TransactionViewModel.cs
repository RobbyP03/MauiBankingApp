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

       
    }
}