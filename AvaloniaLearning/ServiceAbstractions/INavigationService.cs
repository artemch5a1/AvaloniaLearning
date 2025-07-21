﻿using System;
using AvaloniaApp.Services.NavService.Absract;
using AvaloniaApp.ViewModel;

namespace AvaloniaApp.ServiceAbstractions
{
    public interface INavigationService
    {
        void Navigate<TViewModel>()
            where TViewModel : ViewModelTemplate;

        void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelTemplate;

        public void DestroyAndNavigate<TViewModel>()
            where TViewModel : ViewModelTemplate;

        public void DestroyAndNavigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelTemplate;

        public void NavigateOverlay<TViewModel>(
            Action<ViewModelTemplate?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : ViewModelTemplate;

        public void NavigateOverlay<TViewModel, TParam>(
            TParam @params,
            Action<ViewModelTemplate?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : ViewModelTemplate;

        public void CloseOverlay();

        void NavigateBack();
    }
}
