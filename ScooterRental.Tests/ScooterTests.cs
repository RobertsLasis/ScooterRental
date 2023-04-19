using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ScooterRental.Tests
{
    public class ScooterTests
    {
        private Scooter _scooter;

        [SetUp]
        public void Setup()
        {
            _scooter = new Scooter("Scooter", 2m);
        }

        [Test]
        public void GetPricePerMinute_ReturnsPricePerMinute()
        {
            _scooter.PricePerMinute.Should().Be(2m);
        }
    }
}
