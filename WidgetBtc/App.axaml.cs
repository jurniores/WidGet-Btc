using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace WidgetBtc;

public partial class App : Application
{
    IClassicDesktopStyleApplicationLifetime? desktop;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);


    }
    public void Exit(object? sender, EventArgs e)
    {
        desktop?.Shutdown();
    }
    public void Maximize(object? sender, EventArgs e)
    {
       desktop?.MainWindow?.Activate();
    }
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktop)
        {
            desktop = classicDesktop;
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}