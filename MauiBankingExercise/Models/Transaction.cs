using SQLite;
using SQLiteNetExtensions.Attributes;

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

        // Navigation properties
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public TransactionType TransactionType { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Account Account { get; set; }
    }
}