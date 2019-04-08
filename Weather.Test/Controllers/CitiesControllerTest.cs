using System;
using System.Threading.Tasks;
using GlobalWeather.Controllers;
using GlobalWeather.Services;
using NSubstitute;
using Serilog;
using TestStack.BDDfy;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Weather.Persistence.Models;


namespace Weather.Test.Controllers
{
    public class CitiesControllerTest
    {
        private ICityService _service;
        private CitiesController _controller;
        private City _testData;
        private ActionResult<City> _result;

        #region Facts
        [Fact]
        public void GetReturnsExpectedResult()
        {
            this.Given(x => GivenCitiesControllerSetup())
                .And(x => GivenGeLastAccessedCityReturnsExpected())
                .When(x => WhenGetCalledAsync())
                .Then(x => ThenResultShouldBeOk())
                .BDDfy();
        }

        [Fact]
        public void PostCallService()
        {
            this.Given(x => GivenCitiesControllerSetup())
                .When(x => WhenPostCalledAsync())
                .Then(x => ThenItShouldCallUpdateAccessedCityInService())
                .BDDfy();
        }
        #endregion

        #region Gievns

        private void GivenCitiesControllerSetup()
        {
            _testData = new City
            { Id = "26216", Name = "Melbourne", CountryId = "AU", AccessedDate = DateTimeOffset.UtcNow };
            _service = Substitute.For<ICityService>();
            _controller = new CitiesController(_service, Substitute.For<ILogger>());
        }

        private void GivenGeLastAccessedCityReturnsExpected()
        {
            _service.GetLastAccessedCityAsync().Returns(new City());
        }


        #endregion

        #region Whens
        private async Task WhenGetCalledAsync()
        {
            _result = await _controller.Get();
        }

        private async Task WhenPostCalledAsync()
        {
            await _controller.Post(_testData);
        }
        #endregion

        #region Thens
        private void ThenResultShouldBeOk()
        {
            Assert.NotNull(_result);
            Assert.IsType<City>(_result.Value);
        }

        private void ThenItShouldCallUpdateAccessedCityInService()
        {
            _service.Received().UpdateLastAccessedCityAsync(_testData);
        }
        #endregion

    }
}
