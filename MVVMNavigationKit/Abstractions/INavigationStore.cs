using MvvmNavigationKit.Abstractions.ViewModelBase;
using System.ComponentModel;

namespace MvvmNavigationKit.Abstractions
{
    public interface INavigationStore : INotifyPropertyChanged
    {
        ViewModelTemplate? CurrentViewModel { get; set; }
    }
}