using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{


    public class CustomerType
    {
        [PrimaryKey, AutoIncrement]
        public int CustomerTypeId { get; set; }

        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Customer> Customers { get; set; }
    }
}
