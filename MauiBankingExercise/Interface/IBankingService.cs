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
        Task<Account> GetAccountByCustomerId(int customerid);
        Task<Account> GetAccountById(int accountid);
        Task<List<Customer>> GetAllCustomers();
        Task <Customer> GetCustomersById(int id);
        Task<Transaction> GetTransactionsByAccountId(int accountid);
    }
}
