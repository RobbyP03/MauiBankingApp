using MauiBankingExercise.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace MauiBankingExercise.Services
{
    public class BankingDatabaseService
    {
        private static BankingDatabaseService _instance;

        public static BankingDatabaseService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BankingDatabaseService();
            }
            return _instance;
        }

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

        public void UpdateCustomer(Customer customer)
        {
            _dbConnection.Update(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            _dbConnection.Delete<Customer>(customerId);
        }


    }
}