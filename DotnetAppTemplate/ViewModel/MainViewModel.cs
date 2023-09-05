using DotnetAppTemplate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using DotnetAppTemplate.Model.Interfaces;

namespace DotnetAppTemplate.ViewModel;

public class MainViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; set; }
    
    private int Counter = 0;

    private string bindingText = "Hello World!";

    public string BindingText
    {
        get { return bindingText; }
        set { 
            bindingText = value;
            NotifyPropertyChanged();
        }
    }

    public MainViewModel(INavigationService navigation)
    {
        NavigationService = navigation;
    }

    public void IncreaseCounter()
    {
        Counter++;
        BindingText = $"Counter: {Counter}";
    }
}
