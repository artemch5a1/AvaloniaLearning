using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaLearning.NavService
{
    public class NavigationService
    {
        private readonly NavStore _navStore;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(NavStore navStore, IServiceProvider serviceProvider)
        {
            _navStore = navStore;
            _serviceProvider = serviceProvider;
        }

        public void Navigate<TViewModel>()
            where TViewModel : ViewModelBase
        {
            ViewModelBase? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            _navStore.CurrentViewModel = viewModel;
        }
    }
}
