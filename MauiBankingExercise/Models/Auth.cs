using SQLite;
using SQLiteNetExtensions.Attributes;


namespace MauiBankingExercise.Models
{

    public class Auth
    {
        [PrimaryKey, AutoIncrement]
        public int AuthId { get; set; }

        [ForeignKey(typeof(Customer))]
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [ForeignKey(typeof(AuthType))]
        public int AuthTypeId { get; set; }

        [ManyToOne]
        public Customer Customer { get; set; }

        [ManyToOne]
        public AuthType AuthType { get; set; }
    }
}
