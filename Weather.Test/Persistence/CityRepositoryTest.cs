using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Persistence.Config;
using Weather.Persistence.Models;
using Weather.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using Serilog;
using TestStack.BDDfy;
using Xunit;

namespace Weather.Test.Persistence
{
    public class CityRepositoryTest
    {
        private DbContextOptions<WeatherDbContext> _contextOptions;
        private WeatherDbContext _appContext;
        private IOptions<DbContextSettings> _settings;
        private IDbContextFactory _dbContextFactory;
        private CityRepository _subject;
        private IList<City> _testData;
        private City _result;

        #region Facts
        [Fact]
        public void GetLastAccessedCityShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasCities(10))
                .When(x => WhenGetLastAccessedCityCalledAsync())
                .Then(x => ThenItShouldBeLastAccessedCityAsExpected())
                .BDDfy();
        }
        [Fact]
        public void InsertOrUpdateCityShouldSucceed()
        {
            var city = new City() { Id = "1", CountryId = "AU", Name = "Melbourne", AccessedDate = DateTimeOffset.Now };
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasCities(10))
                .When(x => WhenInsertOrUpdateCityCalledAsync(city))
                .Then(x => ThenTheCityShouldBeUpdatedAsExpected(city))
                .BDDfy();
        }

        #endregion
        #region Givens
        private void GivenADatabase(string context)
        {
            _contextOptions = MockDatabaseHelper.CreateNewContextOptions(context);
            _appContext = new WeatherDbContext(_contextOptions);
            _settings = Substitute.For<IOptions<DbContextSettings>>();

            _settings.Value.Returns(new DbContextSettings { DbConnectionString = "test" });
            _dbContextFactory = Substitute.For<IDbContextFactory>();
            _dbContextFactory.DbContext.Returns(_appContext);
            _subject = new CityRepository(_dbContextFactory, Substitute.For<ILogger>());
        }

        private void GivenTheDatabaseHasCities(int numberOfCities)
        {
            _testData = new List<City>();
            for (var item = 0; item < numberOfCities; item++)
            {
                _testData.Add(
                    new City()
                    {
                        Id = (item + 1).ToString(),
                        Name = $"Client{item}",
                        CountryId = "AU",
                        AccessedDate = new DateTime(2018, 12, 16, 10 + item, 0, 0),
                    }
                );
            }

            _appContext.Cities.AddRange(_testData);
            _appContext.SaveChanges();
        }
        #endregion

        #region Whens
        private async Task WhenGetLastAccessedCityCalledAsync()
        {
            _result = await _subject.GetLastAccessedCityAsync();
        }

        private async Task WhenInsertOrUpdateCityCalledAsync(City city)
        {
            await _subject.InsertOrUpdateCityAsync(city);
        }
        #endregion
        private void ThenItShouldBeLastAccessedCityAsExpected()
        {
            var city = _testData.OrderByDescending(x => x.AccessedDate).FirstOrDefault();
            Assert.Equal(_result, city);
        }

        private void ThenTheCityShouldBeUpdatedAsExpected(City city)
        {
            var result = _testData[0];
            Assert.Equal(result.Id, city.Id);
            Assert.Equal(result.AccessedDate, city.AccessedDate);
            Assert.Equal(result.Name, city.Name);
            Assert.Equal(result.CountryId, city.CountryId);
        }
        #region Thens
        #endregion
    }
}
