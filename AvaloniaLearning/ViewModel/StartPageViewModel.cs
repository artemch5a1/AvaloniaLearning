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

        public void NavToMain() => _navService.DestroyAndNavigate<MainPageViewModel>();
    }
}
