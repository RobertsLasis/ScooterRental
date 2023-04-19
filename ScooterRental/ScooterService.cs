using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService: IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooters)
        {
            _scooters = scooters;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (string.IsNullOrEmpty(id)) throw new ScooterIdNotProvidedException();
            if (pricePerMinute <= 0) throw new InvalidPriceException();

            var scooter = new Scooter(id, pricePerMinute);
            _scooters.Add(scooter);
        }

        public void RemoveScooter(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ScooterIdNotProvidedException();
            var scooter = _scooters.SingleOrDefault(s => s.Id == id);
            if (scooter != null)
            {
                _scooters.Remove(scooter);
            }
            else
            {
                throw new WrongScooterIdProvidedException();
            }
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (string.IsNullOrEmpty(scooterId)) throw new ScooterIdNotProvidedException();
            var scooter = _scooters.SingleOrDefault(s => s.Id == scooterId);
            if (scooter != null)
            {
                return scooter;
            }
            throw new WrongScooterIdProvidedException();
        }
    }
}
