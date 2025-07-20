using AvaloniaApp.ViewModel;
using System;

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

        public void NavigateOverlay<TViewModel>(
            Action<ViewModelBase?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : ViewModelBase;

        public void NavigateOverlay<TViewModel, TParam>(
            TParam @params,
            Action<ViewModelBase?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : ViewModelBase;

        public void CloseOverlay();

        void NavigateBack();
    }
}
