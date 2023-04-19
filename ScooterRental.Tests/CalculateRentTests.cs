using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ScooterRental.Tests
{
    public class CalculateRentTests
    {
        private ICalculateRent _calculateRent;
        private Dictionary<Scooter, DateTime> _rentalStart;
        private Scooter _scooter;
        private Scooter _cheapScooter;

        [SetUp]
        public void Setup()
        {
            _scooter = new Scooter("Scooter", 60m);
            _cheapScooter = new Scooter("Scooter", 0.01m);
            _rentalStart = new Dictionary<Scooter, DateTime>();
            _calculateRent = new CalculateRent(_rentalStart);
        }

        [Test]
        public void RentCompleted_ScooterRentedForHour_ReturnsMaxRentOfOneDay()
        {
            _rentalStart.Add(_scooter, DateTime.Now.AddHours(-1));
            var rent = _calculateRent.RentCompleted(_scooter);
            rent.Should().Be(20);
        }

        [Test]
        public void RentCompleted_ScooterRentedForTwoDays_ReturnsRentForThreeDays()
        {
            _rentalStart.Add(_scooter, DateTime.Now.AddDays(-2));
            var rent = _calculateRent.RentCompleted(_scooter);
            rent.Should().Be(60);
        }

        [Test]
        public void RentCompleted_CheapScooterRentedForDay_ReturnsRentForThreeDays()
        {
            _rentalStart.Add(_cheapScooter, DateTime.Now.AddDays(-2));
            var rent = _calculateRent.RentCompleted(_cheapScooter);
            rent.Should().Be(28.79m);
        }
    }
}
