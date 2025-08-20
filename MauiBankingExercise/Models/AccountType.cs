using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{


    public class AccountType
    {
        [PrimaryKey, AutoIncrement]
        public int AccountTypeId { get; set; }

        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Account> Accounts { get; set; }
    }
}
