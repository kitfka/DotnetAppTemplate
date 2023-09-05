using DotnetAppTemplate.Model.Config;
using DotnetAppTemplate.ViewModel;

namespace DotnetAppTemplate.Maui.Views;

public partial class SettingsPage : ContentPage
{
    ConfigViewModel viewModel => BindingContext as ConfigViewModel;


    public SettingsPage(ConfigViewModel configViewModel)
	{
		BindingContext = configViewModel;
		InitializeComponent();
	}
}