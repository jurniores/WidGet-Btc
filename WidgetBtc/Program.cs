using Avalonia;
using System;
using System.Threading;

namespace WidgetBtc;

class Program
{
    private static Mutex? _mutex;
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        const string mutexName = "widgetappnamebtcxpub";
        bool isNewInstance;

        _mutex = new Mutex(true, mutexName, out isNewInstance);

        if (!isNewInstance)
        {
            // Se o mutex já estiver em uso, feche a nova tentativa de abertura
            Console.WriteLine("Application is already running.");
            return;
        }

        BuildAvaloniaApp()
          .StartWithClassicDesktopLifetime(args);
        GC.KeepAlive(_mutex);
    
    }
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
