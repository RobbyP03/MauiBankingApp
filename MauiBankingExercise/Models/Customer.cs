using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{

    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhysicalAddress { get; set; }
        public string IdentityNumber { get; set; }

        [ForeignKey(typeof(CustomerType))]
        public int CustomerTypeId { get; set; }
        public int GenderTypeId { get; set; }
        public int RaceTypeId { get; set; }
        public string Nationality { get; set; }
        public int MaritalStatusId { get; set; }
        public int EmploymentStatusId { get; set; }

        [ForeignKey(typeof(Bank))]
        public int BankId { get; set; }

        [ManyToOne]
        public Bank Bank { get; set; }

        [ManyToOne]
        public CustomerType CustomerType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Account> Accounts { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Auth> Auths { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Asset> Assets { get; set; }
    }
}
