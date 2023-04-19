using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public interface ICalculateRent
    {
        public void RentHasStartedWhen(Scooter scooter);
        public decimal RentCompleted(Scooter scooter);
        public decimal RentNotCompleted();
    }
}
