using MauiBankingExercise.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using MauiBankingExercise.Models;
using MauiBankingExercise.Exceptions;
using MauiBankingExercise.Interface;

namespace MauiBankingExercise.Services
{
    public class BankingDataApiService : IBankingService
    {
        private HttpClient _apiClient;
        private ApplicationSettings _applicationSettings;

        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public BankingDataApiService(ApplicationSettings applicationSettings)
        {

#if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            _apiClient = new HttpClient(insecureHandler);

#else
            _apiClient = new HttpClient();

#endif
            _applicationSettings = applicationSettings;


        }

        private HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Customers");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    List<Customer>? allCustomers = JsonSerializer.Deserialize<List<Customer>>(content, _jsonSerializerOptions);

                    return allCustomers ?? new List<Customer>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException("Failed to fetch customer from API.");
            }

            return new List<Customer>();
        }


        public async Task<Customer> GetCustomersById(int id)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Customers/{id}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    Customer? oneCustomer = JsonSerializer.Deserialize<Customer>(content, _jsonSerializerOptions);

                    return oneCustomer ?? new Customer();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch customer data for {id} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch customer data for {id} from API.");


        }


        public async Task<Account> GetAccountByCustomerId(int customerid)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Accounts/customer/{customerid}");

            Debug.WriteLine($"Requesting account for customer ID: {customerid}");
            Debug.WriteLine($"Request URI: {uri}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                Debug.WriteLine($"Response status: {response.StatusCode}");
                Debug.WriteLine($"Response success: {response.IsSuccessStatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response content: {content}");

                    Account? oneAccount = JsonSerializer.Deserialize<Account>(content, _jsonSerializerOptions);

                    if (oneAccount != null)
                    {
                        Debug.WriteLine($"Deserialized account - ID: {oneAccount.AccountId}, Balance: {oneAccount.AccountBalance}");
                    }
                    else
                    {
                        Debug.WriteLine("Deserialized account is null");
                    }

                    return oneAccount ?? new Account();
                }
                else
                {
                    Debug.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error content: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetAccountByCustomerId: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new BankingApiFailedException($"Failed to fetch account data for {customerid} from API.");
            }

            throw new BankingApiFailedException($"Failed to fetch account data for {customerid} from API.");
        }

        public async Task<Account> GetAccountById(int accountid)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Accounts/{accountid}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    Account? accountsid = JsonSerializer.Deserialize<Account>(content, _jsonSerializerOptions);

                    return accountsid ?? new Account();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch account data for {accountid} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch account data for {accountid} from API.");


        }

        public async Task<List<Transaction>> GetTransactionsByAccountId(int accountid)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Transactions/account/{accountid}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Transaction>? accountTransactions = JsonSerializer.Deserialize<List<Transaction>>(content, _jsonSerializerOptions);

                    return accountTransactions ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch account transactions for {accountid} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch account transactions for {accountid} from API.");
        }

        public async Task<List<Transaction>> GetTransactionsByCustomerId(int customerId)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Transactions/customer/{customerId}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Transaction>? customerTransactions = JsonSerializer.Deserialize<List<Transaction>>(content, _jsonSerializerOptions);

                    return customerTransactions ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch customer transactions for {customerId} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch customer transactions for {customerId} from API.");
        }


        public async Task<List<TransactionType>> GetAllTransactionTypes()
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Transactions/types");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    //  Deserialize into TransactionType (not Transaction)
                    List<TransactionType>? allTransTypes =
                        JsonSerializer.Deserialize<List<TransactionType>>(content, _jsonSerializerOptions);

                    return allTransTypes ?? new List<TransactionType>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException("Failed to fetch transaction types from API.");
            }

            return new List<TransactionType>();
        }

        public async Task AddTransaction(Transaction transaction)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Transactions/{transaction.AccountId}");

            try
            {
                string jsonContent = JsonSerializer.Serialize(transaction, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _apiClient.PutAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Update failed with status: {response.StatusCode}");
                    throw new BankingApiFailedException($"Failed to update transaction with ID {transaction.AccountId}. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to update transaction data for ID {transaction.AccountId}.");
            }
        }
        public async Task UpdateAccount(Account account)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/api/Accounts/{account.AccountId}");

            try
            {
                string jsonContent = JsonSerializer.Serialize(account, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _apiClient.PutAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Update failed with status: {response.StatusCode}");
                    throw new BankingApiFailedException($"Failed to update account with ID {account.AccountId}. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to update account data for ID {account.AccountId}.");
            }
        }
        public async Task<TransactionType?> GetTransactionTypeByName(string name)
        {
            try
            {
                var types = await GetAllTransactionTypes();
                return types.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching transaction type '{name}': {ex.Message}");
                return null;
            }
        }
    }
}