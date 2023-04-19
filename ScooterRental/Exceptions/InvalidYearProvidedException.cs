using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental.Exceptions
{
    public class InvalidYearProvidedException: Exception
    {
        public InvalidYearProvidedException(): base ("Invalid year provided.")
        {
            
        }
    }
}
