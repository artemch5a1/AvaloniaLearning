using AvaloniaLearning.ViewModel;

namespace AvaloniaLearning.NavService
{
    public interface INavigationService
    {
        void Navigate<TViewModel>()
            where TViewModel : ViewModelBase;

        void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase;
    }
}
