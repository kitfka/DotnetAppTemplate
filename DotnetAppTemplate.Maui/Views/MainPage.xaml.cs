using DotnetAppTemplate.ViewModel;

namespace DotnetAppTemplate.Maui.Views;

public partial class MainPage : ContentPage
{
    MainViewModel viewModel => BindingContext as MainViewModel;

    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        viewModel.IncreaseCounter();


        //count++;

        //if (count == 1)
        //    CounterBtn.Text = $"Clicked {count} time";
        //else
        //    CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

