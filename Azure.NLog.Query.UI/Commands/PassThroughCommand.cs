using System;
using System.Windows.Input;

namespace Azure.NLog.Query.UI.Commands
{
  public class PassThroughCommand : ICommand
  {
    private readonly Func<bool> canEdit;

    private readonly Action execute;

    public PassThroughCommand(Action execute)
    {
      this.execute = execute;
    }

    public PassThroughCommand(Action execute, Func<bool> canEdit)
    {
      this.execute = execute;
      this.canEdit = canEdit;
    }

    public bool CanExecute(object parameter)
    {
      return this.canEdit?.Invoke() ?? true;
    }

    public void Execute(object parameter)
    {
      this.execute();
    }

    public event EventHandler CanExecuteChanged;
  }
}