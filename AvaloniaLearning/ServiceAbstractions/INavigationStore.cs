using AvaloniaApp.Services.NavService.Absract;
using System.ComponentModel;

namespace AvaloniaApp.ServiceAbstractions
{
    public interface INavigationStore : INotifyPropertyChanged
    {
        ViewModelTemplate? CurrentViewModel { get; set; }
    }
}