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

## Инструкция по использованию

### Создание базовой ViewModel

- Создайте класс и наследуйте его от ViewModelTemplate
- Реализуйте абстрактные методы

Пример:
```csharp
  public class ViewModelBase : ViewModelTemplate
{
    protected bool IsDisposed { get; set; } = false;

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        GC.SuppressFinalize(this);
        IsDisposed = true;
    }

    public override void RefreshPage() { }

    protected override void InitializeParams<T>(T @params) { }
}
```

### Создание MainWindowViewModel

Реализуйте ViewModel для главного экрана:
- Передайте в конструктор INavigationStore
- Создайте свойство, которое будет брать из INavigationStore текущую ViewModel
- Подпишитесь на изменение INavigationStore, чтобы реагировать на навигацию

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
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        _navStore.PropertyChanged -= OnViewModelChanged;
        CurrentViewModel?.Dispose();
        base.Dispose();
    }
}

```

На главном экране привяжите CurrentViewModel в ContentControl

```xaml
<ContentControl Content="{Binding CurrentViewModel}"/>

```

### Регистрация зависимостей в DI

Чтобы пользоватья сервисом необходимо:
- Создать DI контейнер
- Сконфигурировать необходмиые зависимости

Можно воспользоваться готовым методом из пространства имен MVVMNavigationKit.ServiceBuild

```csharp
private void ConfigureNavigationServices(IServiceCollection services)
{
    NavigationServicesHelper.CreateServiceCollections(services);
}
```

Или настроить зависимости в ручную для более тонкой настройке

```csharp
services.AddLogging(config =>
{
    config.SetMinimumLevel(LogLevel.Information);
});

services.AddSingleton<INavigationStore, NavigationStore>();

services.Configure<NavigationOptions>(opt => { });

services.AddSingleton<INavigationService, NavigationService>();
```

Далее необходимо настроить MainWindow и стартовую страницу

- Получите из сервис провайдера MainWindowViewModel и установите его в качестве DataContext MainWindow:

```csharp
desktop.MainWindow = new MainWindow();
desktop.MainWindow.DataContext =
    ServiceProvider.GetRequiredService<MainWindowViewModel>();
```

- Создайте стартовую страницу:

ViewModel
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

- Установите в MainWindow соотвествие <View, ViewModel>

```xaml
<Window.DataTemplates>
	<DataTemplate DataType="vm:StartPageViewModel">
		<view:StartPage/>
	</DataTemplate>
</Window.DataTemplates>
```

- Навигируйтесь на нее в App.axaml.cs

```csharp
if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
{
    desktop.MainWindow = new MainWindow();
    desktop.MainWindow.DataContext =
        ServiceProvider.GetRequiredService<MainWindowViewModel>();
}

INavigationService navigationService =
    ServiceProvider.GetRequiredService<INavigationService>();

navigationService.Navigate<StartPageViewModel>();
```

### Регистрация ViewModel и навигация

Прежде чем использовать навигацию для новой связки <View, ViewModel>:
- Унаследуйте ваш ViewModel от ViewModelBase

```csharp
public partial class MainPageViewModel : ViewModelBase
{
    private readonly INavigationService _navService;

    public MainPageViewModel(INavigationService navService)
    {
        _navService = navService;
    }
}
```

- Зарегистрируйте в DI

```csharp
services.AddTransient<MainPageViewModel>();
```

- Установите соотвествие view и viewmodel в MainWindow

```csharp
<Window.DataTemplates>
	<DataTemplate DataType="vm:StartPageViewModel">
		<view:StartPage/>
	</DataTemplate>
	<DataTemplate DataType="vm:MainPageViewModel">
		<view:MainPage/>
	</DataTemplate>
</Window.DataTemplates>
```

- Навигируйтесь одним из нужных способов

```csharp
public void NavToMain() => _navService.DestroyAndNavigate<MainPageViewModel>();
```

---

## 🔧 Используемые технологии

- Avalonia UI (https://avaloniaui.net/) — кросс-платформенный UI-фреймворк.
- Microsoft.Extensions.DependencyInjection (https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) — встроенный DI-контейнер .NET.
- Splat.Microsoft.Extensions.DependencyInjection (https://github.com/reactiveui/splat) — интеграция Splat с Microsoft DI.
- CommunityToolkit.Mvvm (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) — лёгкая и мощная MVVM-библиотека.

<details>
  <summary>📁 Архитектура проекта</summary>

  <pre>
AvaloniaLearning
├── Dependencies
│   ├── NavigationStore
│   │   └── NavStore.cs
│   └── NavService
│       └── NavigationService.cs
|       └── INavigationService.cs
├── View
│   ├── Base
│   │   └── MainWindow.axaml
│   └── Pages
│       ├── MainPage.axaml
│       └── StartPage.axaml
├── ViewModel
│   ├── ViewModelBase
│   │   ├── MainWindowViewModel.cs
│   │   └── ViewModelBase.cs
│   ├── MainPageViewModel.cs
│   └── StartPageViewModel.cs

  </pre>
</details>
