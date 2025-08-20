using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;


namespace MauiBankingExercise.Models
{

    public class Bank
    {
        [PrimaryKey, AutoIncrement]
        public int BankId { get; set; }

        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BranchCode { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public string OperatingHours { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Customer> Customers { get; set; }
    }
}
