using Presentation;

namespace PresentationTests;

internal class TextErrorInformer : IErrorInformer
{
    private string _recentMessage;

    public TextErrorInformer()
    {
        this._recentMessage = string.Empty;
    }

    public void InformError(string message)
    {
        this._recentMessage = message;
    }

    public void InformSuccess(string message)
    {
        this._recentMessage = message;
    }

    public string GetRecentMessage()
    {
        return this._recentMessage;
    }
}
