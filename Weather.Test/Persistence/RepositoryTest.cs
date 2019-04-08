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
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Weather.Test.Persistence
{
    public class RepositoryTest
    {
        private DbContextOptions<WeatherDbContext> _contextOptions;
        private City _testData;
        private WeatherDbContext _appContext;
        private IOptions<DbContextSettings> _settings;
        private IDbContextFactory _dbContextFactory;
        private Repository<City> _subject;
        private City _result;

        public RepositoryTest()
        {
            _testData = new City { Id = "26216", Name = "Melbourne", CountryId = "AU", AccessedDate = new DateTime(2018, 12, 29, 10, 1, 2) };

        }

        #region Facts
        [Fact]
        public void CreateCityShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .And(x => GivenTheDatabaseHasCities(1))
                .When(x => WhenCreateIsCalledWithTheCityAsync(_testData))
                .Then(x => ThenItShouldReturnTheCity(_testData))
                .BDDfy();
        }

        [Fact]
        public void CreateCityShouldThrowException()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACity(_testData))
                .When(x => WhenCreateSameIdIsCalledWithTheCityAsync(_testData))
                .Then(x => ThenItShouldBeSuccessful())
                .BDDfy();
        }

        [Fact]
        public void GetCityShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACity(_testData))
                .When(x => WhenGetCalledWithTheCityIdAsync(_testData.Id))
                .Then(x => ThenItShouldReturnTheCity(_testData))
                .BDDfy();

        }

        [Fact]
        public void UpdateCityShouldSucceed()
        {
            var city = new City
            {
                Id = _testData.Id,
                Name = "Melbourne",
                CountryId = "AU",
                AccessedDate = new DateTime(2018, 12, 30, 10, 1, 2)
            };
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACity(_testData))
                .When(x => WhenUpdateCalledWithTheCityAsync(city))
                .Then(x => ThenItShouldReturnTheCity(city))
                .BDDfy();
        }

        [Fact]
        public void DeleteCityShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACity(_testData))
                .When(x => WhenDeleteCalledWithTheCityIdAsync(_testData.Id))
                .Then(x => ThenItShouldBeNoExistCity())
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
            _subject = new Repository<City>(_dbContextFactory, Substitute.For<ILogger>());
        }

        private void GivenTheDatabaseHasCities(int numberOfCities)
        {
            var cities = new List<City>();
            for (var item = 0; item < numberOfCities; item++)
            {
                cities.Add(
                    new City()
                    {
                        Id = (item + 1).ToString(),
                        Name = $"City{item}",
                        CountryId = "AU",
                        AccessedDate = DateTimeOffset.UtcNow,
                    }
                );
            }

            _appContext.Cities.AddRange(cities);
            _appContext.SaveChanges();
        }

        private void GivenTheDatabaseHasACity(City city)
        {
            _appContext.Cities.Add(city);
            _appContext.SaveChanges();
        }
        #endregion

        #region Whens
        private async Task<bool> WhenCreateIsCalledWithTheCityAsync(City city)
        {
            _result = await _subject.AddEntity(city);
            return true;
        }

        private async Task WhenCreateSameIdIsCalledWithTheCityAsync(City city)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _subject.AddEntity(city));
        }

        private async Task<bool> WhenGetCalledWithTheCityIdAsync(string id)
        {
            _result = await _subject.GetEntity(id);
            return true;
        }

        private async Task<bool> WhenUpdateCalledWithTheCityAsync(City city)
        {
            var entity = await _subject.GetEntity(city.Id);
            entity.Name = city.Name;
            entity.CountryId = city.CountryId;
            entity.AccessedDate = city.AccessedDate;
            _result = await _subject.UpdateEntity(entity);
            return true;
        }

        private async Task<bool> WhenDeleteCalledWithTheCityIdAsync(string id)
        {
            await _subject.DeleteEntity(id);
            return true;
        }
        #endregion

        #region Then
        private void ThenItShouldReturnTheCity(City city)
        {
            _result.Id.ShouldBe(city.Id);
        }

        private void ThenItShouldBeSuccessful()
        { }

        private void ThenItShouldBeNoExistCity()
        {
            _appContext.Cities.Count().ShouldBe(0);
        }

        #endregion
    }
}
