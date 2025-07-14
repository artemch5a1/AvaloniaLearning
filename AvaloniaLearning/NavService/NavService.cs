using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.ViewModel;

namespace AvaloniaLearning.NavService
{
    public class NavService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavStore _navStore;

        public NavService(NavStore navStore)
        {
            _navStore = navStore;
        }
    }
}
