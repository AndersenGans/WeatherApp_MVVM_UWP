using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WeatherApp_MVVM.Models;
using WeatherApp_MVVM.Services;

namespace WeatherApp_MVVM.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private Service_WeatherApp _service;
        public ObservableCollection<Weather> _weathers { get; set; }
        public ObservableCollection<string> _cities { get; set; }

        public ICommand SearchCommand { get; set; }

        public WeatherViewModel()
        {
            _service = new Service_WeatherApp();

            SearchCommand = new RelayCommand(FindWeathers);

            NameFilter = "";
            CountOfDaysFilter = "";
            SelectedIndexFilter = -1;

            GetMainCities();
        }

        private string _nameFilter;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                RaisePropertyChanged(() => NameFilter);
                if (SelectedIndexFilter != -1)
                {
                    _selectedIndexFilter = -1;
                    RaisePropertyChanged(() => SelectedIndexFilter);
                }      
            }
        }

        private string _countOfDaysFilter;
        public string CountOfDaysFilter
        {
            get => _countOfDaysFilter;
            set
            {
                _countOfDaysFilter = value;
                RaisePropertyChanged(() => CountOfDaysFilter);
                if (SelectedIndexFilter != -1)
                {
                    _selectedIndexFilter = -1;
                    RaisePropertyChanged(() => SelectedIndexFilter);
                }                
            }
        }

        private int _selectedIndexFilter;
        public int SelectedIndexFilter
        {
            get => _selectedIndexFilter;
            set
            {
                _selectedIndexFilter = value;
                RaisePropertyChanged(() => SelectedIndexFilter);
                if (NameFilter != "")
                {
                    _nameFilter = "";
                    RaisePropertyChanged(() => NameFilter);
                }
                if (CountOfDaysFilter != "")
                {
                    _countOfDaysFilter = "";
                    RaisePropertyChanged(() => CountOfDaysFilter);
                }                
            }
        }

        public string CityName { get; set; }
        public string WaitingMessage { get; set; }

        private async void FindWeathers()
        {
            try
            {
                _weathers = new ObservableCollection<Weather>();
                _weathers.Clear();

                WaitingMessage = "Searching! Wait...";
                RaisePropertyChanged(() => WaitingMessage);


                int _days;
                if (!string.IsNullOrWhiteSpace(NameFilter))
                {
                    if (Int32.TryParse(CountOfDaysFilter, out _days))
                    {
                        var weathers = (ICollection) await _service.GetWeathersForCity(NameFilter, _days);

                        foreach (Weather weather in weathers)
                        {
                            weather.IconId = "/Assets/icons/" + weather.IconId;
                            _weathers.Add(weather);
                        }
                        if(_weathers.Any())
                            CityName = _weathers[0].City;
                    }
                    else throw new FormatException("'Count of days' value must be integer!");
                    if(!_weathers.Any())
                        throw new ArgumentException("There is no city like '" + NameFilter +"'!");

                }
                else if (SelectedIndexFilter != -1)
                {
                    _days = 3; //default
                    var nameCity = _cities[SelectedIndexFilter];
                    CityName = nameCity;
                    var weathers = (ICollection) await _service.GetWeathersForCity(nameCity, _days);

                    foreach (Weather weather in weathers)
                    {
                        weather.IconId = "/Assets/icons/" + weather.IconId;
                        _weathers.Add(weather);
                    }
                }
                else
                {
                    WaitingMessage = "Fields empty!";
                    RaisePropertyChanged(() => WaitingMessage);
                    return;
                }
                RaisePropertyChanged(() => _weathers);
                RaisePropertyChanged(() => CityName);

                WaitingMessage = "";
                RaisePropertyChanged(() => WaitingMessage);
            }
            catch (HttpRequestException)
            {
                WaitingMessage = "Connection error!";
                RaisePropertyChanged(() => WaitingMessage);
            }
            catch (FormatException ex)
            {
                WaitingMessage = ex.Message;
                RaisePropertyChanged(() => WaitingMessage);
            }
            catch (ArgumentException ex)
            {
                WaitingMessage = ex.Message;
                RaisePropertyChanged(() => WaitingMessage);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                WaitingMessage = "Not so fast!";
                RaisePropertyChanged(() => WaitingMessage);
            }
        }

        private async void GetMainCities()
        {
            try
            {
                WaitingMessage = "Loading! Wait...";
                RaisePropertyChanged(() => WaitingMessage);

                _cities = new ObservableCollection<string>();
                var cities = (ICollection) await _service.GetMainCities();
                foreach (CityModel city in cities)
                {
                    _cities.Add(city.Name);
                }

                WaitingMessage = "";
                RaisePropertyChanged(() => WaitingMessage);
            }
            catch (HttpRequestException)
            {
                WaitingMessage = "Connection error!";
                RaisePropertyChanged(() => WaitingMessage);
            }
        }
    }
}