using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using WeatherApp_MVVM.ViewModels;

namespace WeatherApp_MVVM.Infrastructure
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new NavigationService();
            navigationService.Configure(nameof(WeatherViewModel), typeof(WeatherViewModel));

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<WeatherViewModel>();
        }

        public WeatherViewModel WeatherInstance => ServiceLocator.Current.GetInstance<WeatherViewModel>();
    }
}