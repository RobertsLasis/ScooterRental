using ScooterRental.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private Dictionary<int, decimal> _incomeByYears;
        public string Name { get; }
        private readonly IScooterService _scooterService;
        private ICalculateRent _calculateRent;

        public RentalCompany(string name, IScooterService scooterService, Dictionary<int, decimal> incomeByYears, ICalculateRent calculateRent)
        {
            Name = name;
            _scooterService = scooterService;
            _calculateRent = calculateRent;
            _incomeByYears = incomeByYears;
        }
        
        public void StartRent(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ScooterIdNotProvidedException();
            var scooter = _scooterService.GetScooterById(id);
            scooter.IsRented = true;
            _calculateRent.RentHasStartedWhen(scooter);
        }

        public decimal EndRent(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ScooterIdNotProvidedException();
            var scooter = _scooterService.GetScooterById(id);
            scooter.IsRented = false;
            var rent = _calculateRent.RentCompleted(scooter);
            if (_incomeByYears.ContainsKey(DateTime.Now.Year))
            {
                _incomeByYears[DateTime.Now.Year] += rent;
            }
            else
            {
                _incomeByYears.Add(DateTime.Now.Year, rent);
            }
            return rent;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if (year.HasValue && year.Value < 0) throw new InvalidYearProvidedException();
            var notCompletedRentals = _calculateRent.RentNotCompleted();
            if (!_incomeByYears.ContainsKey(DateTime.Now.Year))
            {
                _incomeByYears.Add(DateTime.Now.Year, 0);
            }
            decimal income = 0;
            if (year.HasValue)
            {
                income = _incomeByYears[year.Value];
            }
            else
            {
                foreach (var item in _incomeByYears)
                {
                    income += _incomeByYears[item.Key];
                }
            }

            if (includeNotCompletedRentals)
            {
                var sum = notCompletedRentals + income;
                return sum;
            }
            return income;
        }
    }
}
