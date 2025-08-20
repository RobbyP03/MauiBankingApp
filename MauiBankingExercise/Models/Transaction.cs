using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace MauiBankingExercise.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int TransactionId { get; set; }

        [ForeignKey(typeof(TransactionType))]
        public int TransactionTypeId { get; set; }

        [ForeignKey(typeof(Account))]
        public int AccountId { get; set; }

        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        [ManyToOne]
        public Account Account { get; set; }

        [ManyToOne]
        public TransactionType TransactionType { get; set; }
    }
}
