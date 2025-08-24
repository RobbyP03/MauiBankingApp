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
            return _dbConnection.Table<Customer>().FirstOrDefault(c => c.CustomerId == id);
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
            return _dbConnection.Table<Transaction>()
                               .Where(t => t.AccountId == accountId)
                               .OrderByDescending(t => t.TransactionDate)
                               .ToList();
        }
    }
}