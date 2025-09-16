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
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Customers");

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
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Customers/{id}");

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
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Accounts/customer/{customerid}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    Account? oneAccount = JsonSerializer.Deserialize<Account>(content, _jsonSerializerOptions);

                    return oneAccount ?? new Account();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch account data for {customerid} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch account data for {customerid} from API.");

        }

        public async Task<Account> GetAccountById(int accountid)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Accounts/{accountid}");

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

        public async Task<Transaction> GetTransactionsByAccountId(int accountid)
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Transactions/account/{accountid}");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    Transaction? accountTransaction = JsonSerializer.Deserialize<Transaction>(content, _jsonSerializerOptions);

                    return accountTransaction ?? new Transaction();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException($"Failed to fetch customer data for {accountid} from API.");
            }
            throw new BankingApiFailedException($"Failed to fetch customer data for {accountid} from API.");

        }

        public async Task<List<Transaction>> GetAllTransactionTypes()
        {
            Uri uri = new Uri($"{_applicationSettings.ServiceUrl}/Transactions/types");

            try
            {
                HttpResponseMessage response = await _apiClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize directly to AgeRestriction objects since API now returns them
                    List<Transaction>? allTransTypes = JsonSerializer.Deserialize<List<Transaction>>(content, _jsonSerializerOptions);

                    return allTransTypes ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw new BankingApiFailedException("Failed to fetch transaction types from API.");
            }

            return new List<Transaction>();
        }

    }
}
