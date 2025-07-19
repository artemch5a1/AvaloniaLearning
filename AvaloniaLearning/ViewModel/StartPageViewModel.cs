using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApp.NavigationStore;
using AvaloniaApp.NavService;

namespace AvaloniaApp.ViewModel
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;

        public StartPageViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public void NavToMain() => _navService.Navigate<MainPageViewModel>();
    }
}
