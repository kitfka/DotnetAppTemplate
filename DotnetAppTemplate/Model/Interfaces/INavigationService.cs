
namespace DotnetAppTemplate.Model.Interfaces;

public interface INavigationService
{
    bool? ShowDialog<TView>() where TView : class, IMyWindow;
    void ShowView<TView>() where TView : class, IMyWindow;
#nullable enable

    /// <summary>
    /// Create a confirm window with yes & no options.
    /// </summary>
    /// <param name="action">Method to be executed when yes is pressed. If the action is null the UI will be transformed to a Ok Window</param>
    /// <param name="message">Will be displayed to the user</param>
    void Confirm(Action? action, string message);

    /// <summary>
    /// Create a confirm window with yes & no options.
    /// </summary>
    /// <param name="action">Method to be executed when yes is pressed. If the action is null the UI will be transformed to a Ok Window</param>
    /// <param name="message">Will be displayed to the user</param>
    /// <param name="options">Spacial options that you will need to implement yourself</param>
    void Confirm(Action? action, string message, ConfirmWindowOptions options);

    TView CreateWindow<TView>() where TView : class, IMyWindow;
}
