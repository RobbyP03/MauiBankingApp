using MauiBankingExercise.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MauiBankingExercise.Services
{
    public class BankingDatabaseService
    {
        private SQLiteConnection _dbConnection;

        public string GetDatabasePath()
        {
            string filename = "banking.db";
            string pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(pathToDb, filename);
        }

        public BankingDatabaseService()
        {
            _dbConnection = new SQLiteConnection(GetDatabasePath());
            BankingSeeder.Seed(_dbConnection);
        }

        public List<Customer> GetAllCustomers()
        {
            return _dbConnection.Table<Customer>().ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            var customer = _dbConnection.Table<Customer>().FirstOrDefault(c => c.CustomerId == id);
            if (customer != null)
            {
                _dbConnection.GetChildren(customer); // Load related data
            }
            return customer;
        }

        public List<Account> GetAccountsByCustomerId(int customerId)
        {
            var accounts = _dbConnection.Table<Account>()
                               .Where(a => a.CustomerId == customerId)
                               .ToList();
            // Load related data for each account
            foreach (var account in accounts)
            {
                _dbConnection.GetChildren(account);
            }
            return accounts;
        }

        public Account? GetAccountById(int accountId)
        {
            var account = _dbConnection.Table<Account>()
                            .FirstOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                _dbConnection.GetChildren(account);
            }
            return account;
        }

        public void AddTransaction(Transaction transaction)
        {
            _dbConnection.Insert(transaction);
        }

        public void UpdateAccount(Account account)
        {
            _dbConnection.Update(account);
        }

        public List<Transaction> GetTransactionsByAccountId(int accountId)
        {
            var transactions = _dbConnection.Table<Transaction>()
                               .Where(t => t.AccountId == accountId)
                               .OrderByDescending(t => t.TransactionDate)
                               .ToList();

            // Load related data for each transaction (TransactionType, etc.)
            foreach (var transaction in transactions)
            {
                _dbConnection.GetChildren(transaction);
            }

            return transactions;
        }

        // NEW METHOD: Get all transactions for a customer across all their accounts
        public List<Transaction> GetAllTransactionsByCustomerId(int customerId)
        {
            // Get all accounts for the customer first
            var customerAccounts = GetAccountsByCustomerId(customerId);

            var allTransactions = new List<Transaction>();

            foreach (var account in customerAccounts)
            {
                var accountTransactions = GetTransactionsByAccountId(account.AccountId);
                foreach (var transaction in accountTransactions)
                {
                    // Add account reference for display purposes
                    transaction.Account = account;
                    allTransactions.Add(transaction);
                }
            }

            return allTransactions;
        }

        public List<TransactionType> GetAllTransactionTypes()
        {
            return _dbConnection.Table<TransactionType>().ToList();
        }

        public TransactionType GetTransactionTypeByName(string name)
        {
            return _dbConnection.Table<TransactionType>()
                .FirstOrDefault(tt => tt.Name == name);
        }
      
    }
}