using AvaloniaApp.ViewModel;

namespace AvaloniaApp.ServiceAbstractions
{
    public interface INavigationService
    {
        void Navigate<TViewModel>()
            where TViewModel : ViewModelBase;

        void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase;

        public void DestroyAndNavigate<TViewModel>()
            where TViewModel : ViewModelBase;

        public void DestroyAndNavigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase;

        void NavigateBack();
    }
}
