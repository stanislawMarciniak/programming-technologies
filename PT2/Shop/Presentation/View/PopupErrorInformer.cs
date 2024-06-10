using System.Windows;
using Presentation.ViewModel;

namespace Presentation.View;

internal class PopupErrorInformer : IErrorInformer
{
    private string _recentMessage;

    public PopupErrorInformer()
    {
        _recentMessage = string.Empty;
    }

    public void InformError(string message)
    {
        _recentMessage = message;

        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void CallMessageBox(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void InformSuccess(string message)
    {
        _recentMessage = message;

        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public string GetRecentMessage()
    {
        return _recentMessage;
    }
}
