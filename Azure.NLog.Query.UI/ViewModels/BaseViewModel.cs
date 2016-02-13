using System.ComponentModel;
using System.Runtime.CompilerServices;
using Azure.NLog.Query.UI.Annotations;

namespace Azure.NLog.Query.UI.ViewModels
{
  public class BaseViewModel : INotifyPropertyChanged
  {
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}