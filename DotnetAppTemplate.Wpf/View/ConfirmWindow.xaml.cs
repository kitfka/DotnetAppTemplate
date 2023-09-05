using DotnetAppTemplate.Model;
using DotnetAppTemplate.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Timer = System.Timers.Timer;
#nullable enable
namespace DotnetAppTemplate.Wpf.View;


/// <summary>
/// The only window where you can't use the <see cref="ViewModel.MainWindowViewModel.GetWindow{T}"/> method for.
/// </summary>
public partial class ConfirmWindow : Window, IMyWindow
{

    private Action? Action = null;
    Timer? myTimer;
    private readonly Dispatcher _uiDispatcher;


    private bool CloseOnEnter = false;

    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// This window is for 2 things.
    /// When we need to give a Yes, No option.
    /// And when we NEED to send a message to the user.    
    /// /// </summary>
    /// <param name="action">The action to be performed when you press yes. If null this parameter is null it will turn into an Ok dialog.</param>
    /// <param name="message">Text to be displayed to the user.</param>
    public ConfirmWindow()
    {
        InitializeComponent();
        _uiDispatcher = Dispatcher.CurrentDispatcher;
    }


    public void Init(Action? action, string message, ConfirmWindowOptions options = 0)
    {
        Action = action;
        Message.Text = message;

        if (action == null)
        {
            // OK only option.
            Button_No.Content = "Ok";
            Button_Yes.IsEnabled = false;
            Button_Yes.Visibility = Visibility.Collapsed;
        }

        if (options.HasFlag(ConfirmWindowOptions.Fullscreen))
        {

            WindowStyle = WindowStyle.None; // this will do nothing XD this is the default value!
            WindowState = WindowState.Maximized;

            Message.FontSize = 50;
        }

        if (options.HasFlag(ConfirmWindowOptions.CloseOnEnter))
        {
            CloseOnEnter = true;
        }

        if (options.HasFlag(ConfirmWindowOptions.SelfDestruct1s))
        {
            myTimer = new();

            // Tell the timer what to do when it elapses
            myTimer.Elapsed += ReadTimeout;
            myTimer.AutoReset = false;
            myTimer.Interval = 1000;
            myTimer.Enabled = true;

            myTimer.Start();
        }
    }

    private void ReadTimeout(object? source, ElapsedEventArgs e)
    {
        // We can't close windows on different threads. This will return us to Thread 1!
        _uiDispatcher.BeginInvoke(new Action(() =>
        {
            Close();
        }));
    }


    private void Button_Yes_Click(object sender, RoutedEventArgs e)
    {
        Action?.Invoke();

        Close();
    }

    private void Button_No_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        window.KeyDown += HandleKeyPress;
    }

    private void HandleKeyPress(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                if (CloseOnEnter)
                {
                    _uiDispatcher.BeginInvoke(new Action(() =>
                    {
                        Close();
                    }));
                }
                break;
            default:
                break;
        }
    }
}
