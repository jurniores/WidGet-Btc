using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Controller;
using System.Globalization;

namespace WidgetBtc;

public partial class MainWindow : Window
{
    private int time = 60000;
    private bool bounce = false;
    Fetch fetch;
    List<Coin.Money>? listUsd, listBrl;
    public MainWindow()
    {
        InitializeComponent();
        CanResize = false;

        Position = new PixelPoint(1020, 0);

        fetch = new Fetch("https://economia.awesomeapi.com.br", this);

        var coroutine = new Coroutine(time, GetMethod);
        coroutine.Start();
        UIEvents();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Opened += (sender, e) => HideFromAltTab();

    }
    private void UIEvents()
    {
        foreach (var borders in stack_panel_borders.Children)
        {
            if (borders is Border border)
            {
                border.PointerEntered += EventsUI.EventsUI.EnteredBackGroundColor!;
                border.PointerExited += EventsUI.EventsUI.ExitedBackGroundColor!;
                border.PointerPressed += EventsUI.EventsUI.RedirectFromSite!;
            }
        }
    }
    void Refresh(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (bounce) return;
        bounce = true;
        GetMethod();  // Fecha a janela
    }
    private async void GetMethod()
    {
        try
        {
            listUsd ??= await fetch.GetList<Coin.Money>("/json/daily/BTC-USD/2");
            listBrl ??= await fetch.GetList<Coin.Money>("/json/daily/BTC-BRL/2");

            listUsd?.ForEach(e =>
            {
                Console.WriteLine(listUsd?[1].bid);
            });

            string dolarLastDay = listUsd?[1].bid ?? "0";
            string realLastDay = listBrl?[1].bid ?? "0";



            var btcUsd = await fetch.Get<Coin>("/json/last/BTC-USD");
            var btcBrl = await fetch.Get<Coin>("/json/last/BTC-BRL");
            var usdBrl = await fetch.Get<Coin>("/json/last/USD-BRL");

            string? valUsdMaxLastDay = listUsd?.Count > 0 ? listUsd[1].bid : "0";

            string valBtcDolar = btcUsd?.BTCUSD?.bid ?? "0";
            string valBtcDolarMin = btcUsd?.BTCUSD?.low ?? "0";
            string valBtcDolarMax = btcUsd?.BTCUSD?.high ?? "0";

            string valBtcReal = btcBrl?.BTCBRL?.bid ?? "0";
            string valBtcRealMin = btcBrl?.BTCBRL?.low ?? "0";
            string valBtcRealMax = btcBrl?.BTCBRL?.high ?? "0";

            string valDolarReal = usdBrl?.USDBRL?.bid ?? "";

            if (float.Parse(valBtcDolar) > float.Parse(valUsdMaxLastDay!))
            {
                var color = Color.Parse("#76fb8d");
                btc_dolar.Foreground = new SolidColorBrush(color);
                btc_real.Foreground = new SolidColorBrush(color);
            }
            else
            {
                btc_dolar.Foreground = new SolidColorBrush(Colors.Red);
                btc_real.Foreground = new SolidColorBrush(Colors.Red);
            }

            btc_dolar_max_day_last.Text = $"{dolarLastDay[..valBtcDolarMax.IndexOf('.')]}$";
            btc_real_max_day_last.Text = $"{realLastDay[..valBtcDolarMax.IndexOf('.')]}R$";
            btc_dolar.Text = $"$ {valBtcDolar[..valBtcDolarMax.IndexOf('.')]}";
            
            btc_dolar_max_min.Text = $"Max: {valBtcDolarMax[..valBtcDolarMax.IndexOf('.')]} Min: {valBtcDolarMin[..valBtcDolarMax.IndexOf('.')]}";

            btc_real.Text = $"R$ {valBtcReal[..valBtcDolarMax.IndexOf('.')]}";
            btc_real_max_min.Text = $"Max: {valBtcRealMax} Min: {valBtcRealMin[..valBtcDolarMax.IndexOf('.')]}";

            dolar_real.Text = $"Dolar: {valDolarReal[..valBtcDolarMax.IndexOf('.')]}R$";
            bounce = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }



    // P/Invoke para alterar o estilo da janela
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    // Constantes para alterar o estilo da janela
    private const int GWL_EXSTYLE = -20;
    private const uint WS_EX_TOOLWINDOW = 0x00000080;  // Janela de ferramenta (não aparece no Alt + Tab)
    private const uint WS_EX_APPWINDOW = 0x00040000;   // Janela de aplicativo (aparece no Alt + Tab)
    public void CloseApp_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        Close();  // Fecha a janela
    }
    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        button_close.IsVisible = true;  // Torna o botão visível
    }

    // Quando o ponteiro sai da área do Border, o botão desaparece
    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        button_close.IsVisible = false;  // Torna o botão invisível
    }
    private void HideFromAltTab()
    {
        // Obtém o handle da janela usando TryGetPlatformHandle

        var platformHandle = TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;

        if (platformHandle == IntPtr.Zero)
            return;

        // Remove o estilo "AppWindow" e adiciona o estilo "ToolWindow"
        uint style = GetWindowLong(platformHandle, GWL_EXSTYLE);
        style &= ~WS_EX_APPWINDOW;  // Remove WS_EX_APPWINDOW
        style |= WS_EX_TOOLWINDOW;  // Adiciona WS_EX_TOOLWINDOW
        SetWindowLong(platformHandle, GWL_EXSTYLE, style);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs pressed)
    {
        BeginMoveDrag(pressed);
    }


}



