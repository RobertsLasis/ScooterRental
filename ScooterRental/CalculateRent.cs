using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class CalculateRent: ICalculateRent
    {
        private Dictionary<Scooter, DateTime> _rentalStart;
        private Dictionary<Scooter, decimal> _rent = new Dictionary<Scooter, decimal>();

        public CalculateRent(Dictionary<Scooter, DateTime> rentalStart)
        {
            _rentalStart = rentalStart;
        }

        public void RentHasStartedWhen(Scooter scooter)
        {
            if (_rentalStart.ContainsKey(scooter))
            {
                _rentalStart[scooter] = DateTime.Now;
            }
            else
            {
                _rentalStart.Add(scooter, DateTime.Now);
            }
        }

        public decimal RentCompleted(Scooter scooter)
        {
            if (_rent.ContainsKey(scooter))
            {
                _rent[scooter] = 0;
            }
            else
            {
                _rent.Add(scooter, 0);
            }
            decimal minutesInADay = 1440;

            if (_rentalStart[scooter].Date == DateTime.Now.Date)
            {
                double intervalToday = Math.Floor(DateTime.Now.Subtract(_rentalStart[scooter]).TotalMinutes);
                decimal rentToday = (decimal)intervalToday * scooter.PricePerMinute;
                if (rentToday > 20)
                {
                    _rent[scooter] += 20;
                }
                else
                {
                    _rent[scooter] += rentToday;
                }

            }
            else
            {
                double intervalOnStartRentDay = Math.Ceiling(_rentalStart[scooter].Subtract(_rentalStart[scooter].Date).TotalMinutes);
                decimal rentOnStartRentDay = (minutesInADay - (decimal)intervalOnStartRentDay) * scooter.PricePerMinute;
                if (rentOnStartRentDay > 20)
                {
                    _rent[scooter] += 20;
                }
                else
                {
                    _rent[scooter] += rentOnStartRentDay;
                }

                while (_rentalStart[scooter].AddDays(1).Date != DateTime.Now.Date)
                {
                    if ((scooter.PricePerMinute * minutesInADay) > 20)
                    {
                        _rent[scooter] += 20;
                    }
                    else
                    {
                        _rent[scooter] += scooter.PricePerMinute * minutesInADay;
                    }
                    _rentalStart[scooter] = _rentalStart[scooter].AddDays(1);
                }

                double intervalOnEndRentDay = Math.Floor(DateTime.Now.Subtract(DateTime.Now.Date).TotalMinutes);
                decimal rentOnEndRentDay = (decimal)intervalOnEndRentDay * scooter.PricePerMinute;
                if (rentOnEndRentDay > 20)
                {
                    _rent[scooter] += 20;
                }
                else
                {
                    _rent[scooter] += rentOnEndRentDay;
                }
            }

            return _rent[scooter];
        }

        public decimal RentNotCompleted()
        {
            Dictionary<Scooter, DateTime> copy = new Dictionary<Scooter, DateTime>(_rentalStart);
            decimal uncompletedRent = 0;
            foreach (var item in _rentalStart)
            {
                uncompletedRent += RentCompleted(item.Key);
            }

            _rentalStart = copy;
            return uncompletedRent;
        }
    }
}
