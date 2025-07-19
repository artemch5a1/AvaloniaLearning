using System;
using System.Collections.Generic;
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
    /// <para>
    /// История навигации реализована на основе стека, в котором хранятся ранее открытые ViewModel.
    /// По умолчанию размер истории неограничен, но может быть задан вручную через параметр конструктора.
    /// </para>
    /// <para>
    /// Если количество сохранённых переходов превысит заданный лимит, самая ранняя запись будет удалена.
    /// Это поведение может использоваться для ограничения потребления памяти в приложениях с большим количеством навигаций.
    /// </para>
    /// <para>
    /// Важно: чтобы избежать неожиданной потери навигационной истории, рекомендуется использовать <see cref="NavigateBack"/>
    /// только если история не пуста. Проверить это можно с помощью <see cref="HistoryIsNotEmpty"/>
    /// </para>
    /// </remarks>
    public class NavigationService : INavigationService
    {
        private readonly NavStore _navStore;
        private readonly IServiceProvider _serviceProvider;

        private Stack<ViewModelBase> _historyNavigation = new Stack<ViewModelBase>();
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
            int maxSizeHistory = int.MaxValue
        )
        {
            _navStore = navStore;
            _serviceProvider = serviceProvider;
            _maxSizeHistory = maxSizeHistory;
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
            ViewModelBase? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
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
        /// Текущая ViewModel не добавляется в историю при возврате назад.
        /// </para>
        /// Если будет превышен лимит истории, самое раннее использование <see cref="NavigateBack"/>
        /// перестанет работать из-за очистки последнего элемента истории навигации
        /// </remarks>
        public void NavigateBack()
        {
            if (!HistoryIsNotEmpty)
            {
                return;
            }

            ViewModelBase viewModel = _historyNavigation.Pop();

            _navStore.CurrentViewModel = viewModel;
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
                    _historyNavigation = new Stack<ViewModelBase>(_historyNavigation);
                    _historyNavigation.Pop();
                    _historyNavigation = new Stack<ViewModelBase>(_historyNavigation);
                }
                _historyNavigation.Push(_navStore.CurrentViewModel);
            }
            _navStore.CurrentViewModel = viewModel;
        }
    }
}
