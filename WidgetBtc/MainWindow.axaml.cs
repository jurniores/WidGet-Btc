using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Threading;
using Controller;

namespace WidgetBtc;

public partial class MainWindow : Window
{
    private int time = 60000;
    public MainWindow()
    {
        InitializeComponent();
        CanResize = false;
        Position = new PixelPoint(1020, 0);
        var coroutine = new Coroutine(time, GetMethod);
        coroutine.Start();
        //Opened += (sender, e) => HideFromAltTab();
    }
    void Refresh(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       GetMethod();  // Fecha a janela
    }
    private async void GetMethod()
    {
        var fetch = new Fetch("https://economia.awesomeapi.com.br", this);
        var btcUsd = await fetch.Get<Coin>("/json/last/BTC-USD");
        var btcBrl = await fetch.Get<Coin>("/json/last/BTC-BRL");
        var usdBrl = await fetch.Get<Coin>("/json/last/USD-BRL");
    
        
        btc_dolar.Text = $"$ {btcUsd?.BTCUSD?.bid}";
        btc_real.Text = $"$ {btcBrl?.BTCBRL?.bid}";
        dolar_real.Text = $"$ {usdBrl?.USDBRL?.bid}";
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



