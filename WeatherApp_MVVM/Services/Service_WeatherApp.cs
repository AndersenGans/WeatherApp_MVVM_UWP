using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp_MVVM.Models;

namespace WeatherApp_MVVM.Services
{
    public class Service_WeatherApp
    {
        public async Task<ICollection<CityModel>> GetMainCities()
        {
            string URL = "http://localhost:56070/api/DefaultCities/";
            var client = new HttpClient();
            var response = await client.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var citiesCollection = JsonConvert.DeserializeObject<ICollection<CityModel>>(result);

            return citiesCollection;
        }

        public async Task<ICollection<Weather>> GetWeathersForCity(string cityName, int countOfDays)
        {
            string URL = "http://localhost:56070/api/Weathers/" + cityName + "/" + countOfDays;
            var client = new HttpClient();
            var response = await client.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var weathersCollection = JsonConvert.DeserializeObject<ICollection<Weather>>(result);

            return weathersCollection;
        }
}
}