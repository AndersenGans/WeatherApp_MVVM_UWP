using System;

namespace WeatherApp_MVVM.Models
{
    public class Weather
    {
        public int WeatherId { get; set; }
        public double DayAvgTemp { get; set; }
        public double DayMinTemp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public string MainWeather { get; set; }
        public string DescWeather { get; set; }
        public double WindSpeed { get; set; }
        public double Cloudiness { get; set; }
        public string IconId { get; set; }
        public DateTime Day { get; set; }
        public int CityId { get; set; }

        public string City { get; set; }
    }
}