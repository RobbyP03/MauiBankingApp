using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{


    public class AuthType
    {
        [PrimaryKey, AutoIncrement]
        public int AuthTypeId { get; set; }
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Auth> Auths { get; set; }
    }
}
