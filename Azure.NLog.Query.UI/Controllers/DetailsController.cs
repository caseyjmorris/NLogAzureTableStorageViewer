using Azure.NLog.Query.UI.ViewModels;
using NLog.Extensions.AzureTableStorage;

namespace Azure.NLog.Query.UI.Controllers
{
  public class DetailsController : BaseViewModel
  {
    private LogEntity log;

    public LogEntity Log
    {
      get { return this.log; }
      set
      {
        if (Equals(value, this.log)) return;
        this.log = value;
        this.OnPropertyChanged(nameof(this.Log));
      }
    }
  }
}