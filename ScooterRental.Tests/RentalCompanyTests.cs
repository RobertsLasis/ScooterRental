using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    public class RentalCompanyTests
    {
        private IRentalCompany _company;
        private IScooterService _scooterService;
        private ICalculateRent _calculateRent;
        private List<Scooter> _scooters;
        private string scooterID = "first";
        private decimal pricePerMinute = 1.5m;
        private Dictionary<int, decimal> _incomeByYears;

        [SetUp]
        public void Setup()
        {
            _incomeByYears = new Dictionary<int, decimal>();
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
            _calculateRent = new CalculateRent(new Dictionary<Scooter, DateTime>());
            _company = new RentalCompany("Test", _scooterService, _incomeByYears, _calculateRent);
        }

        [Test]
        public void CreateRentalCompany_TestAsNameProvided_NameShouldBeTest()
        {
            _company.Name.Should().Be("Test");
        }

        [Test]
        public void StartRent_ValidIdProvided_IsRentedTrue()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _company.StartRent(scooterID);
            var scoot = _scooterService.GetScooterById(scooterID);
            scoot.IsRented.Should().BeTrue();
        }

        [Test]
        public void StartRent_StartRentWithoutID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _company.StartRent(""); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void StartRent_StartRentWithNullID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _company.StartRent(null); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void EndRent_ValidIdProvided_ReturnsRentAndScooterIsNotRented()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _company.StartRent(scooterID);
            var rentOne = _company.EndRent(scooterID);
            _company.StartRent(scooterID);
            var rentTwo = _company.EndRent(scooterID);
            var scooter = _scooterService.GetScooterById(scooterID);
            scooter.IsRented.Should().BeFalse();
            var rent = rentOne + rentTwo;
            rent.Should().Be(0);
        }

        [Test]
        public void EndRent_EndRentWithoutID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _company.EndRent(""); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void EndRent_EndRentWithNullID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _company.EndRent(null); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void CalculateIncome_YearProvidedIncludeNotCompletedRentalsFalse_ReturnsYearsIncome()
        {
            _incomeByYears.Add(2020, 200);
            _incomeByYears[2020] += 200;
            var income = _company.CalculateIncome(2020, false);
            income.Should().Be(400);
        }

        [Test]
        public void CalculateIncome_YearNotProvidedIncludeNotCompletedRentalsFalse_ReturnsAllIncome()
        {
            _incomeByYears.Add(2020, 200);
            _incomeByYears.Add(2022, 400);
            var income = _company.CalculateIncome(null, false);
            income.Should().Be(600);
        }

        [Test]
        public void CalculateIncome_YearProvidedIncludeNotCompletedRentalsTrue_ReturnsYearsFutureIncome()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _company.StartRent(scooterID);
            _incomeByYears.Add(2020, 200);
            _incomeByYears.Add(2022, 400);
            var income = _company.CalculateIncome(2023, true);
            income.Should().Be(0);
        }

        [Test]
        public void CalculateIncome_InvalidYearProvided_ThrowsInvalidYearProvidedException()
        {
            Action act = () => { _company.CalculateIncome(-1, true); };
            act.Should().Throw<InvalidYearProvidedException>();
        }
    }
}
