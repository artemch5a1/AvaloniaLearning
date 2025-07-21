using AvaloniaApp.Services.NavService.Absract;
using System.ComponentModel;

namespace AvaloniaApp.Stores.NavStore
{
    public interface INavigationStore : INotifyPropertyChanged
    {
        ViewModelTemplate? CurrentViewModel { get; set; }
    }
}