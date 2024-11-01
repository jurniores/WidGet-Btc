using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Input;
namespace EventsUI
{
    public static class EventsUI
    {
        public static void EnteredBackGroundColor(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var color = Color.Parse("#34818d");
                border.Background = new SolidColorBrush(color);
                border.Cursor = new Cursor(StandardCursorType.Hand);
            }

        }
        public static void ExitedBackGroundColor(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var color = Color.Parse("#80D3D3D3");
                border.Background = new SolidColorBrush(color);

            }

        }


        public static void RedirectFromSite(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                if (sender is Border border)
                {
                    string? nameBorder = border.Name;

                    if (nameBorder == "dolar_real_border")
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://br.tradingview.com/chart/?symbol=BITSTAMP%3ABTCUSD",
                            UseShellExecute = true // Abre a URL no navegador padrão
                        });
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://br.tradingview.com/chart/?symbol=BINANCE%3ABTCBRL",
                            UseShellExecute = true // Abre a URL no navegador padrão
                        });
                    }
                    else if (nameBorder == "dolar_border")
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://br.tradingview.com/chart/?symbol=BITSTAMP%3ABTCUSD",
                            UseShellExecute = true // Abre a URL no navegador padrão
                        });

                    }
                    else if (nameBorder == "real_border")
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://br.tradingview.com/chart/?symbol=BINANCE%3ABTCBRL",
                            UseShellExecute = true // Abre a URL no navegador padrão
                        });

                    }else if(nameBorder == "dolar_real_val"){
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://br.tradingview.com/chart/?symbol=FX_IDC%3AUSDBRL",
                            UseShellExecute = true // Abre a URL no navegador padrão
                        });
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open URL: {ex.Message}");
            }
        }

    }

}