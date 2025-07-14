using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;

namespace AvaloniaLearning.ViewModel
{
    internal partial class MainPageViewModel : ViewModelBase
    {
        private readonly NavStore _navStore;

        public MainPageViewModel(NavStore navStore)
        {
            _navStore = navStore;
        }

        public void NavToBack() => _navStore.Navigate<StartPageViewModel>();
    }
}
