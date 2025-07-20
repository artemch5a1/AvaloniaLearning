using System;
using System.Collections.Generic;
using AvaloniaApp.NavigationStore;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AvaloniaApp.NavService
{
    /// <summary>
    /// Сервис навигации между ViewModel в приложении.
    /// Реализует паттерн Navigation Service для MVVM.
    /// </summary>
    /// <remarks>
    /// Для работы требует предварительной регистрации ViewModel в DI-контейнере.
    /// <para>
    /// История навигации реализована на основе стека, в котором хранятся ранее открытые ViewModel.
    /// По умолчанию размер истории неограничен, но может быть задан вручную через
    /// <see cref="NavigationOptions.MaxSizeHistory"/>.
    /// </para>
    /// <para>
    /// Если количество сохранённых переходов превысит заданный лимит, самая ранняя запись будет удалена.
    /// Это поведение может использоваться для ограничения потребления памяти в приложениях с большим количеством навигаций.
    /// </para>
    /// <para>
    /// Важно: чтобы избежать неожиданной потери навигационной истории, рекомендуется использовать <see cref="NavigateBack"/>
    /// только если история не пуста. Проверить это можно с помощью <see cref="HistoryIsNotEmpty"/>
    /// </para>
    /// <para>
    /// Чтобы не сохранять ViewModel в историю используйте <see cref="DestroyAndNavigate{TViewModel}()"/>
    /// или <see cref="DestroyAndNavigate{TViewModel, TParams}(TParams)"/> с передачей параметров
    /// </para>
    /// </remarks>
    public class NavigationService : INavigationService
    {
        private readonly NavStore _navStore;
        private readonly IServiceProvider _serviceProvider;

        private Stack<ViewModelBase> _historyNavigation = new();
        private readonly int _maxSizeHistory;

        /// <summary>
        /// Возврашает true если история не пустая
        /// </summary>
        public bool HistoryIsNotEmpty => _historyNavigation.Count > 0;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса навигации.
        /// </summary>
        /// <param name="navStore">Хранилище состояния навигации</param>
        /// <param name="serviceProvider">Провайдер сервисов (DI-контейнер)</param>
        /// <param name="maxSizeHistory">
        /// Максимальный размер истории навигации.
        /// При превышении этого значения самая ранняя ViewModel будет удалена из истории.
        /// Значение по умолчанию — <c>int.MaxValue</c>, что означает отсутствие ограничений.
        /// </param>
        public NavigationService(
            NavStore navStore,
            IServiceProvider serviceProvider,
            IOptions<NavigationOptions> options
        )
        {
            _navStore = navStore;
            _serviceProvider = serviceProvider;
            _maxSizeHistory = options.Value.MaxSizeHistory;
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
            ViewModelBase viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            PushToHistoryAndSetViewModel(viewModel);
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
            ViewModelBase viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            viewModel.Initialize(@params);
            PushToHistoryAndSetViewModel(viewModel);
        }

        /// <summary>
        /// Выполняет переход к предыдущей ViewModel в истории навигации.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Если история пуста, метод завершается без действий.
        /// </para>
        /// <para>
        /// Чтобы актуализировать данные при возврате, переопределяейте метод
        /// <see cref="ViewModelBase.RefreshPage"/> в целевой ViewModel
        /// </para>
        /// <para>
        /// Текущая ViewModel не добавляется в историю при возврате назад.
        /// </para>
        /// <para>
        /// Если будет превышен лимит истории, самое раннее использование <see cref="NavigateBack"/>
        /// перестанет работать из-за очистки последнего элемента истории навигации
        /// </para>
        /// </remarks>
        public void NavigateBack()
        {
            if (!HistoryIsNotEmpty)
            {
                return;
            }
            _navStore.CurrentViewModel?.Dispose();
            ViewModelBase viewModel = _historyNavigation.Pop();
            viewModel.RefreshPage();
            _navStore.CurrentViewModel = viewModel;
        }

        /// <summary>
        /// Выполняет переход на указанную ViewModel с очищением текущей
        /// </summary>
        /// <typeparam name="TViewModel"> Тип ViewModel, на которую выполняется переход</typeparam>
        /// <remarks>
        /// <para>
        /// Не сохраняет текущую ViewModel в историю
        /// </para>
        /// <para>
        /// Вызывает <see cref="ViewModelBase.Dispose"/> у текущей ViewModel
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если не удалось разрешить TViewModel через DI-контейнер
        /// </exception>
        public void DestroyAndNavigate<TViewModel>()
            where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            DisposeAndSetViewModel(viewModel);
        }

        /// <summary>
        /// Выполняет переход на указанную ViewModel с передачей параметров с очищением текущей ViewModel
        /// </summary>
        /// <typeparam name="TViewModel"> Тип ViewModel, на которую выполняется переход</typeparam>
        /// <typeparam name="TParams">Тип параметров инициализации</typeparam>
        /// <param name="params">Параметры для инициализации ViewModel</param>
        /// <remarks>
        /// <para>
        /// Не сохраняет текущую ViewModel в историю
        /// </para>
        /// <para>
        /// Вызывает <see cref="ViewModelBase.Dispose"/> у текущей ViewModel
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если не удалось разрешить TViewModel через DI-контейнер
        /// </exception>
        public void DestroyAndNavigate<TViewModel, TParams>(TParams @params)
            where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            viewModel.Initialize(@params);
            DisposeAndSetViewModel(viewModel);
        }

        /// <summary>
        /// Добавляет текущую ViewModel в историю и устанавливает новую ViewModel.
        /// </summary>
        /// <param name="viewModel">Новая ViewModel для перехода</param>
        /// <remarks>
        /// <para>
        /// Если текущая ViewModel в <see cref="_navStore"/> равна null, она не добавляется в историю.
        /// </para>
        /// <para>
        /// При достижении максимального размера истории (<see cref="_maxSizeHistory"/>),
        /// самая старая запись удаляется.
        /// </para>
        /// </remarks>
        private void PushToHistoryAndSetViewModel(ViewModelBase viewModel)
        {
            if (_navStore.CurrentViewModel != null)
            {
                if (_maxSizeHistory <= _historyNavigation.Count)
                {
                    RemoveLastVM();
                }
                _historyNavigation.Push(_navStore.CurrentViewModel);
            }
            _navStore.CurrentViewModel = viewModel;
        }

        /// <summary>
        /// Функция для очищения текущей ViewModel и переходу на следующую
        /// </summary>
        /// <remarks>
        /// Вызывается базовый переопределяемый метод у текущей ViewModel
        /// <see cref="ViewModelBase.Dispose"/>
        /// </remarks>
        private void DisposeAndSetViewModel(ViewModelBase viewModel)
        {
            if (_navStore.CurrentViewModel != null)
            {
                _navStore.CurrentViewModel.Dispose();
            }
            _navStore.CurrentViewModel = viewModel;
        }

        /// <summary>
        /// Функция для удаления первой записи истории
        /// </summary>
        private void RemoveLastVM()
        {
            _historyNavigation = new Stack<ViewModelBase>(_historyNavigation);
            ViewModelBase vm = _historyNavigation.Pop();
            vm.Dispose();
            _historyNavigation = new Stack<ViewModelBase>(_historyNavigation);
        }
    }
}
