namespace DotnetAppTemplate.Model.Interfaces;

/// <summary>
/// Use this for every View. Or else you won't be allowed to use it with the <seealso cref="INavigationService"/>.
/// </summary>
public interface IMyWindow
{
    bool? ShowDialog();
    void Show();
}
