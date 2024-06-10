namespace Presentation.ViewModel;

public interface IErrorInformer
{
    public void CallMessageBox(string message);
    void InformError(string message);

    void InformSuccess(string message);

    string GetRecentMessage();
}
