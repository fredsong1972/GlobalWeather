using System;
using System.Threading.Tasks;
using Weather.Persistence.Repositories;
using GlobalWeather.Services;
using NSubstitute;
using Serilog;
using TestStack.BDDfy;
using Weather.Persistence.Models;
using Xunit;

namespace Weather.Test.Services
{
    public class CityServiceTest
    {
        private readonly ICityRepository _repository;
        private City _testData;
        private readonly CityService _subject;

        public CityServiceTest()
        {
            _repository = Substitute.For<ICityRepository>();
            _subject = new CityService(_repository, Substitute.For<ILogger>());
        }
        #region Facts
        [Fact]
        public void GetLastAccessedCityShouldBeSucceed()
        {
            this.Given(x => GivenGetLastAccessedCityReturnsCity())
                .When(x => WhenGetLastAccessedCityAsyncIsCalled())
                .Then(x => ThenItShouldCallGetLastAccessedCityAsyncMethodInDbActions())
                .BDDfy();
        }

        [Fact]
        public void UpdateLastAccessedCityShouldAddEntity()
        {
            this.Given(x => GivenHaveATestData())
                .When(x => WhenUpdateLastAccessedCityAsyncIsCalled())
                .Then(x => ThenItShouldCallInsertOrUpdateCityAsyncMethodInDbActions())
                .BDDfy();
        }

        #endregion

        #region Givens
        private void GivenGetLastAccessedCityReturnsCity()
        {
            _repository.GetLastAccessedCityAsync()
                  .Returns(new City());


        }

        private void GivenHaveATestData()
        {
            _testData = new City
            { Id = "26216", Name = "Melbourne", CountryId = "AU", AccessedDate = DateTimeOffset.UtcNow };

        }

        #endregion

        #region Whens
        private async Task WhenGetLastAccessedCityAsyncIsCalled()
        {
            await _subject.GetLastAccessedCityAsync();
        }

        private async Task WhenUpdateLastAccessedCityAsyncIsCalled()
        {
            await _subject.UpdateLastAccessedCityAsync(_testData);
        }
        #endregion

        #region Thens
        private void ThenItShouldCallGetLastAccessedCityAsyncMethodInDbActions()
        {
            _repository.Received().GetLastAccessedCityAsync();

        }

        private void ThenItShouldCallInsertOrUpdateCityAsyncMethodInDbActions()
        {
            _repository.Received().InsertOrUpdateCityAsync(_testData);

        }
        #endregion
    }
}
