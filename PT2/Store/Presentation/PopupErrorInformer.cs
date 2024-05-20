using System.Windows;

namespace Presentation;

internal class PopupErrorInformer : IErrorInformer
{
    private string _recentMessage;

    public PopupErrorInformer()
    {
        this._recentMessage = string.Empty;
    }

    public void InformError(string message)
    {
        this._recentMessage = message;

        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void InformSuccess(string message)
    {
        this._recentMessage = message;

        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public string GetRecentMessage()
    {
        return this._recentMessage;
    }
}
