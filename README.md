# 🧭 Avalonia Navigation Template

> Cистема навигации для приложений на Avalonia с поддержкой расширенных сценариев и следованием приницпу внедрения зависимостей и SOLID.

---

## 📌 Цель проекта

Этот проект представляет собой шаблон навигации для приложений на Avalonia UI, спроектированный с учётом чистой архитектуры, внедрения зависимостей, а также поддержки многоуровневой и параметризованной навигации.

Он предназначен для:
- Быстрого старта новых Avalonia-проектов.
- Переиспользуемой и тестируемой архитектуры.
- Гибкой и масштабируемой системы навигации.

---

## 🛠️ Основные возможности

- ✅ Обычная навигация между экранами (через ViewModel).
- ✅ Оверлейная (вложенная) навигация, например для модальных окон или панелей.
- ✅ Поддержка возврата назад к предыдущей ViewModel (история в стеке).
- ✅ Навигация назад из оверлея к родительской ViewModel.
- ✅ Передача параметров в конструктор ViewModel.
- ✅ Внедрение зависимостей через Microsoft.Extensions.DependencyInjection и Splat.
- ✅ Соблюдение SOLID-принципов:
  - S — Single Responsibility Principle: каждый компонент (NavStore, NavigationService и т.д.) отвечает за одну задачу.
  - D — Dependency Inversion Principle: все зависимости инъецируются через IoC-контейнер.

---

## 🧽 Инструкция по использованию MVVMNavigationKit

### 📌 1. Создание базовой `ViewModel`

Создайте базовый класс `ViewModel`, унаследованный от `ViewModelTemplate`, и реализуйте его методы:

```csharp
public class ViewModelBase : ViewModelTemplate
{
    protected bool IsDisposed { get; set; } = false;

    public override void Dispose()
    {
        if (IsDisposed) return;
        GC.SuppressFinalize(this);
        IsDisposed = true;
    }

    public override void RefreshPage() { }

    protected override void InitializeParams<T>(T @params) { }
}
```


### 🗄️ 2. Создание `MainWindowViewModel`

`MainWindowViewModel` управляет текущей отображаемой `ViewModel`:

```csharp
public partial class MainWindowViewModel : ViewModelBase
{
    public ViewModelTemplate? CurrentViewModel => _navStore.CurrentViewModel;

    private readonly INavigationStore _navStore;

    public MainWindowViewModel(INavigationStore navStore)
    {
        _navStore = navStore;
        _navStore.PropertyChanged += OnViewModelChanged;
    }

    private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_navStore.CurrentViewModel))
            OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        if (IsDisposed) return;
        _navStore.PropertyChanged -= OnViewModelChanged;
        CurrentViewModel?.Dispose();
        base.Dispose();
    }
}
```

#### 📌 Привязка в XAML

```xml
<ContentControl Content="{Binding CurrentViewModel}" />
```

### 🧰 3. Регистрация зависимостей (DI)

#### ✅ Быстрая настройка:

```csharp
private void ConfigureNavigationServices(IServiceCollection services)
{
    NavigationServicesHelper.CreateServiceCollections(services);
}
```

#### ⚙️ Ручная настройка:

```csharp
services.AddLogging(config => {
    config.SetMinimumLevel(LogLevel.Information);
});

services.AddSingleton<INavigationStore, NavigationStore>();
services.Configure<NavigationOptions>(opt => { });
services.AddSingleton<INavigationService, NavigationService>();
```

### 🪟 4. Настройка `MainWindow` и стартовой страницы

#### 🧹 Привязка `ViewModel` к `MainWindow`:

```csharp
if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
{
    desktop.MainWindow = new MainWindow();
    desktop.MainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
}
```

#### 🌟 Стартовая `ViewModel`:

```csharp
public class StartPageViewModel : ViewModelBase
{
    private readonly INavigationService _navService;

    public StartPageViewModel(INavigationService navService)
    {
        _navService = navService;
    }
}
```

#### 📦 Регистрация соответствия `View` и `ViewModel` в `MainWindow.xaml`:

```xml
<Window.DataTemplates>
    <DataTemplate DataType="vm:StartPageViewModel">
        <view:StartPage />
    </DataTemplate>
</Window.DataTemplates>
```

#### 🧽 Навигация на стартовую страницу:

```csharp
INavigationService navigationService = ServiceProvider.GetRequiredService<INavigationService>();
navigationService.Navigate<StartPageViewModel>();
```

### ➕ 5. Добавление новых `ViewModel`

#### 🔹 Пример новой `ViewModel`:

```csharp
public class MainPageViewModel : ViewModelBase
{
    private readonly INavigationService _navService;

    public MainPageViewModel(INavigationService navService)
    {
        _navService = navService;
    }
}
```

#### 🔹 Регистрация в DI:

```csharp
services.AddTransient<MainPageViewModel>();
```

#### 🔹 Добавление в `DataTemplates`:

```xml
<Window.DataTemplates>
    <DataTemplate DataType="vm:StartPageViewModel">
        <view:StartPage />
    </DataTemplate>
    <DataTemplate DataType="vm:MainPageViewModel">
        <view:MainPage />
    </DataTemplate>
</Window.DataTemplates>
```

#### 🔹 Переход между страницами:

```csharp
public void NavToMain() => _navService.DestroyAndNavigate<MainPageViewModel>();
```

---


## 🔧 Используемые технологии

- Avalonia UI (https://avaloniaui.net/) — кросс-платформенный UI-фреймворк.
- Microsoft.Extensions.DependencyInjection (https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) — встроенный DI-контейнер .NET.
- Splat.Microsoft.Extensions.DependencyInjection (https://github.com/reactiveui/splat) — интеграция Splat с Microsoft DI.
- CommunityToolkit.Mvvm (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) — лёгкая и мощная MVVM-библиотека.
