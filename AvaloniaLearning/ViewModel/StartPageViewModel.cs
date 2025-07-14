using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;

namespace AvaloniaLearning.ViewModel
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly NavStore _navStore;

        public StartPageViewModel(NavStore navStore)
        {
            _navStore = navStore;
        }

        public void NavToMain() => _navStore.Navigate<MainPageViewModel>();
    }
}
