using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotnetAppTemplate.Model;
using DotnetAppTemplate.Model.Interfaces;
using DotnetAppTemplate.ViewModel;

namespace DotnetAppTemplate.Wpf.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMyWindow
{
    private MainViewModel MainViewModel;
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        Closing += MainWindow_Closing;


        DataContext = viewModel;
        MainViewModel = viewModel;
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        RollingLogger.Trace(nameof(MainWindow_Closing), nameof(MainWindow));
        // Lets do a cleanup
        // ...

        Application.Current.Shutdown();
    }

    private void TestButton_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel.NavigationService.Confirm(MainViewModel.IncreaseCounter, "Test Message! \nIncrease the counter?");
    }
}
