using System.Threading.Tasks;
using Weather.Persistence.Models;

namespace GlobalWeather.Services
{
    public interface ICityService
    {
        Task<City> GetLastAccessedCityAsync();
        Task UpdateLastAccessedCityAsync(City city);
    }
}
