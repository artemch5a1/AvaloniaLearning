using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.NavService;

namespace AvaloniaLearning.ViewModel
{
    internal partial class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;

        public MainPageViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        public void NavToBack() => _navService.Navigate<StartPageViewModel>();
    }
}
