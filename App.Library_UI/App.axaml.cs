﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace App.Library_UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) { }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) { }

        base.OnFrameworkInitializationCompleted();
    }
}
