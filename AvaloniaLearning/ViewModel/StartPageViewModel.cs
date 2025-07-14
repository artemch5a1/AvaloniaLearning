using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.NavService;

namespace AvaloniaLearning.ViewModel
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
