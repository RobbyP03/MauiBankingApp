using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiBankingExercise.Exceptions
{
    public class BankingApiFailedException: Exception
    {
        public BankingApiFailedException(string message) : base(message) { }
    }
}
