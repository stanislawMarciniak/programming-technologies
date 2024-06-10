using Presentation.View;
using System.ComponentModel;

namespace Presentation.ViewModel;

public abstract class IViewModel : INotifyPropertyChanged
{
    public IViewModel SelectedViewModel;

    public static IErrorInformer Informer;
    public IViewModel Parent { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
