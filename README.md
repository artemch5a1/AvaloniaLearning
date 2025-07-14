# 🧭 Avalonia Navigation Template

> Эталонная система навигации для приложений на Avalonia с поддержкой расширенных сценариев и следованием приницпу внедрения зависимостей и SOLID.

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
