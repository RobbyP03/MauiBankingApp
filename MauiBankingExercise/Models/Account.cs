using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{

    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        [ForeignKey(typeof(AccountType))]
        public int AccountTypeId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(typeof(Customer))]
        public int CustomerId { get; set; }

        public DateTime DateOpened { get; set; }
        public decimal AccountBalance { get; set; }

        [ManyToOne]
        public Customer Customer { get; set; }

        [ManyToOne]
        public AccountType AccountType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Transaction> Transactions { get; set; }
    }
}
