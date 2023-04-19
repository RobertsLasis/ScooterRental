using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _scooters;
        private string scooterID = "first";
        private decimal pricePerMinute = 1.5m;

        [SetUp]
        public void Setup()
        {
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
        }

        [Test]
        public void AddScooter_AddValidScooter_ScooterAdded()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _scooters.Count.Should().Be(1);
        }

        [Test]
        public void AddScooter_AddScooterWithoutID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.AddScooter("", pricePerMinute); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void AddScooter_AddScooterWithNullID_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.AddScooter(null, pricePerMinute); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void AddScooter_AddScooterWithInvalidPrice_ThrowsInvalidPriceException()
        {
            Action act = () => { _scooterService.AddScooter(scooterID, -0.1m); };
            act.Should().Throw<InvalidPriceException>();
        }

        [Test]
        public void RemoveScooter_ValidIdProvided_ScooterRemoved()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _scooterService.RemoveScooter(scooterID);
            _scooters.Any(x => x.Id == scooterID).Should().BeFalse();
        }

        [Test]
        public void RemoveScooter_NullIdProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.RemoveScooter(null); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void RemoveScooter_WithoutIDProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.RemoveScooter(""); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void RemoveScooter_WrongIDProvided_ThrowsWrongScooterIdProvidedException()
        {
            Action act = () => { _scooterService.RemoveScooter("second"); };
            act.Should().Throw<WrongScooterIdProvidedException>();
        }

        [Test]
        public void GetScooters_ReturnsAllScooters()
        {
            _scooterService.AddScooter(scooterID, pricePerMinute);
            _scooterService.GetScooters().Count.Should().Be(1);
        }

        [Test]
        public void GetScooterById_ValidIdProvided_ReturnScooter()
        {
            var scooter = new Scooter(scooterID, pricePerMinute);
            _scooters.Add(scooter);
            _scooterService.GetScooterById(scooterID).Should().Be(scooter);
        }

        [Test]
        public void GetScooterById_NullIdProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.GetScooterById(null); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void GetScooterById_WithoutIDProvided_ThrowsScooterIdNotProvidedException()
        {
            Action act = () => { _scooterService.GetScooterById(""); };
            act.Should().Throw<ScooterIdNotProvidedException>();
        }

        [Test]
        public void GetScooterById_WrongIDProvided_ThrowsWrongScooterIdProvidedException()
        {
            Action act = () => { _scooterService.GetScooterById("second"); };
            act.Should().Throw<WrongScooterIdProvidedException>();
        }
    }
}