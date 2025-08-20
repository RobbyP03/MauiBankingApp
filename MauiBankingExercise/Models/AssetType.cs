using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MauiBankingExercise.Models
{


    public class AssetType
    {
        [PrimaryKey, AutoIncrement]
        public int AssetTypeId { get; set; }
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Asset> Assets { get; set; }
    }
}
