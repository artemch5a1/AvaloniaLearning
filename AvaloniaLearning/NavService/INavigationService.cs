using AvaloniaApp.ViewModel;

namespace AvaloniaApp.NavService
{
    public interface INavigationService
    {
        void Navigate<TViewModel>()
            where TViewModel : ViewModelBase;

        void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase;

        void NavigateBack();
    }
}
