using MauiBankingExercise.Models;
using MauiBankingExercise.Interface;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls; // for Application.Current.MainPage

namespace MauiBankingExercise.ViewModels
{
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    public class TransactionViewModel : BaseViewModel
    {
        private readonly IBankingService _bankingService;

        private Account? _selectedAccount;
        private int _accountId;
        private int _customerId;
        private decimal _transactionAmount;
        private string _selectedTransactionType = string.Empty;

        public ObservableCollection<string> TransactionTypes { get; set; } = new();
        public ObservableCollection<Transaction> Transactions { get; set; } = new();

        public int AccountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                OnPropertyChanged();
                _ = LoadAccountAndTransactionsAsync();
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

        public TransactionViewModel(IBankingService bankingService)
        {
            _bankingService = bankingService;
            AddTransactionCommand = new Command(async () => await OnAddTransactionAsync());
            _ = LoadTransactionTypesAsync();
        }

        // Load transaction types from API
        private async Task LoadTransactionTypesAsync()
        {
            try
            {
                TransactionTypes.Clear();
                var types = await _bankingService.GetAllTransactionTypes(); // returns TransactionType
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

        // Load selected account and its transactions
        private async Task LoadAccountAndTransactionsAsync()
        {
            if (AccountId > 0 && CustomerId > 0)
            {
                try
                {
                    SelectedAccount = await _bankingService.GetAccountById(AccountId);
                    if (SelectedAccount != null)
                    {
                        await LoadTransactionsAsync();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading account: {ex.Message}");
                }
            }
        }

        // Load transactions for the selected account
        private async Task LoadTransactionsAsync()
        {
            try
            {
                Transactions.Clear();
                var accountTransactions = await _bankingService.GetTransactionsByAccountId(AccountId);

                var sortedTransactions = accountTransactions
                    .OrderByDescending(t => t.TransactionDate);

                foreach (var transaction in sortedTransactions)
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        // Add a transaction
        private async Task OnAddTransactionAsync()
        {
            if (SelectedAccount == null || TransactionAmount <= 0 || string.IsNullOrEmpty(SelectedTransactionType))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Please enter a valid amount and select transaction type.",
                    "OK");
                return;
            }

            try
            {
                // Check withdrawal balance
                if (SelectedTransactionType == "Withdrawal" && TransactionAmount > SelectedAccount.AccountBalance)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Insufficient Funds",
                        $"Cannot withdraw R{TransactionAmount:F2}. Available balance: R{SelectedAccount.AccountBalance:F2}",
                        "OK");
                    return;
                }

                // Get TransactionType object
                var transactionType = await _bankingService.GetTransactionTypeByName(SelectedTransactionType);
                if (transactionType == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Invalid transaction type.",
                        "OK");
                    return;
                }

                // Create transaction
                var transaction = new Transaction
                {
                    AccountId = AccountId,
                    Amount = SelectedTransactionType == "Withdrawal" ? -TransactionAmount : TransactionAmount,
                    TransactionDate = DateTime.Now,
                    TransactionTypeId = transactionType.TransactionTypeId,
                    Description = $"{SelectedTransactionType} of R{TransactionAmount:F2}"
                };

                await _bankingService.AddTransaction(transaction);

                // Update account balance
                SelectedAccount.AccountBalance += transaction.Amount;
                await _bankingService.UpdateAccount(SelectedAccount);

                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    $"Transaction completed successfully!\nNew balance: R{SelectedAccount.AccountBalance:F2}",
                    "OK");

                await LoadAccountAndTransactionsAsync();

                TransactionAmount = 0;
                SelectedTransactionType = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding transaction: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Failed to process transaction. Please try again.",
                    "OK");
            }
        }


        public override async Task OnAppearingAsync()
        {
            // No need to call base.OnAppearing() since BaseViewModel now uses Task
            if (AccountId > 0 && CustomerId > 0)
            {
                await LoadAccountAndTransactionsAsync();
            }
            await LoadTransactionTypesAsync();
        }
    }
}
