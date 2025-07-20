using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApp.ViewModel
{
    /// <summary>
    /// Базовый класс для всех ViewModel в приложении.
    /// Наследует функциональность уведомления об изменении свойств от <see cref="ObservableObject"/>.
    /// </summary>
    /// <remarks>
    /// Предоставляет базовую реализацию для:
    /// - Инициализации ViewModel с параметрами
    /// - Безопасного приведения типов параметров
    /// </remarks>
    public class ViewModelBase : ObservableObject, IDisposable
    {
        protected bool IsDisposed { get; set; } = false;

        /// <summary>
        /// Инициализирует ViewModel с указанными параметрами.
        /// </summary>
        /// <typeparam name="T">Тип параметров инициализации</typeparam>
        /// <param name="params">Параметры для инициализации ViewModel</param>
        /// <remarks>
        /// Перенаправляет вызов в защищенный виртуальный метод <see cref="InitializeParams{T}(T)"/>,
        /// который может быть переопределен в производных классах.
        /// </remarks>
        public void Initialize<T>(T @params)
        {
            InitializeParams(@params);
        }

        /// <summary>
        /// Переопределяемая функция для перезагрузки данных страницы
        /// </summary>
        /// <remarks>
        /// Вызывается каждый раз после использования 
        /// <see cref="NavService.NavigationService.NavigateBack"/>
        /// </remarks>
        public virtual void RefreshPage() { }

        /// <summary>
        /// Виртуальный метод для обработки параметров инициализации.
        /// </summary>
        /// <typeparam name="T">Тип параметров инициализации</typeparam>
        /// <param name="params">Параметры для инициализации ViewModel</param>
        /// <remarks>
        /// Базовая реализация не выполняет никаких действий.
        /// Производные классы должны переопределить этот метод для обработки конкретных параметров,
        /// если это требуется
        /// </remarks>
        protected virtual void InitializeParams<T>(T @params) { }

        /// <summary>
        /// Безопасно преобразует объект параметров к указанному типу.
        /// </summary>
        /// <typeparam name="T">Ожидаемый тип параметров</typeparam>
        /// <param name="params">Объект параметров, который нужно преобразовать</param>
        /// <returns>Параметры приведенные к типу T</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если:
        /// - params не null и не может быть приведен к типу T
        /// - params null, а T не является nullable-типом
        /// </exception>
        /// <remarks>
        /// Упрощает работу с параметрами в производных классах.
        /// Если и params и T являются nullable, возвращает default(T).
        /// </remarks>
        protected static T GetAs<T>(object? @params)
        {
            if (@params is null && default(T) is null)
                return default!;

            if (@params is T t)
                return t;

            throw new ArgumentException(
                $"Expected type {typeof(T).Name}, but got {@params?.GetType().Name ?? "null"}"
            );
        }

        /// <summary>
        /// Переопределяемый метод для освобождения ресурсов
        /// </summary>
        /// <remarks>
        /// Базовая реализация отменяет финализацию
        /// и устанавливает <see cref="IsDisposed"/> = true
        /// </remarks>
        public virtual void Dispose()
        {
            if(IsDisposed) return;
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }
    }
}
