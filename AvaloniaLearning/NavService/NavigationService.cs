using System;
using AvaloniaApp.NavigationStore;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaApp.NavService
{
    /// <summary>
    /// Сервис навигации между ViewModel в приложении.
    /// Реализует паттерн Navigation Service для MVVM.
    /// </summary>
    /// <remarks>
    /// Для работы требует предварительной регистрации ViewModel в DI-контейнере.
    /// </remarks>
    public class NavigationService : INavigationService
    {
        private readonly NavStore _navStore;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса навигации.
        /// </summary>
        /// <param name="navStore">Хранилище состояния навигации</param>
        /// <param name="serviceProvider">Провайдер сервисов (DI-контейнер)</param>
        public NavigationService(NavStore navStore, IServiceProvider serviceProvider)
        {
            _navStore = navStore;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Выполняет переход на указанную ViewModel без параметров.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel, на которую выполняется переход</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если не удалось разрешить TViewModel через DI-контейнер
        /// </exception>
        public void Navigate<TViewModel>()
            where TViewModel : ViewModelBase
        {
            ViewModelBase? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            _navStore.CurrentViewModel = viewModel;
        }

        /// <summary>
        /// Выполняет переход на указанную ViewModel с передачей параметров.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel, на которую выполняется переход</typeparam>
        /// <typeparam name="TParams">Тип параметров инициализации</typeparam>
        /// <param name="params">Параметры для инициализации ViewModel</param>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если не удалось разрешить TViewModel через DI-контейнер
        /// </exception>
        /// <remarks>
        /// Ожидается, что целевая ViewModel реализует метод Initialize(TParams parameters).
        /// </remarks>
        public void Navigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase
        {
            ViewModelBase? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            viewModel.Initialize(@params);
            _navStore.CurrentViewModel = viewModel;
        }
    }
}
