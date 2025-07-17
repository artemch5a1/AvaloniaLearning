using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApp.NavigationStore;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaApp.NavService
{
    public class NavigationService : INavigationService
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

        public void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase
        {
            ViewModelBase? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            viewModel.Initialize(@params);
            _navStore.CurrentViewModel = viewModel;
        }
    }
}
