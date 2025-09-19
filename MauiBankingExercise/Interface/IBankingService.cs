using MauiBankingExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MauiBankingExercise.Interface
{
    public interface IBankingService
    {
        Task AddTransaction(Transaction transaction);
        Task<Account> GetAccountByCustomerId(int customerid);
        Task<Account> GetAccountById(int accountid);
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomersById(int id);
        Task<List<Transaction>> GetTransactionsByCustomerId(int customerId);
        Task<List<Transaction>> GetTransactionsByAccountId(int accountId);
        Task<List<TransactionType>> GetAllTransactionTypes();
        Task UpdateAccount(Account account);
        Task<TransactionType?> GetTransactionTypeByName(string name);
    }
}