using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MauiBankingExercise.Models
{


    public class Asset
    {
        [PrimaryKey, AutoIncrement]
        public int AssetId { get; set; }

        [ForeignKey(typeof(AssetType))]
        public int AssetTypeId { get; set; }

        [ForeignKey(typeof(Customer))]
        public int CustomerId { get; set; }

        public decimal Value { get; set; }
        public string Name { get; set; }

        [ManyToOne]
        public AssetType AssetType { get; set; }

        [ManyToOne]
        public Customer Customer { get; set; }
    }
}
