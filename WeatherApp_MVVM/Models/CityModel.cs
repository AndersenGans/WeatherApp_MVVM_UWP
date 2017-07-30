using System.Collections.Generic;

namespace WeatherApp_MVVM.Models
{
    public class CityModel
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }
        public bool AddToMainList { get; set; }
        public ICollection<Weather> Weathers { get; set; }
    }
}