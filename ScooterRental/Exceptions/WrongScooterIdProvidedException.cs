using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental.Exceptions
{
    public class WrongScooterIdProvidedException: Exception
    {
        public WrongScooterIdProvidedException(): base ("Wrong scooter ID provided.")
        {
            
        }
    }
}
