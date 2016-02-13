using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Azure.NLog.Query.UI.Commands;
using Azure.NLog.Query.UI.ViewModels;
using NLog.Extensions.AzureTableStorage;

namespace Azure.NLog.Query.UI.Controllers
{
  public class QueryController : BaseViewModel
  {
    private ConnectionViewModel connection;
    private QueryViewModel query;
    private ObservableCollection<LogEntity> results;

    public QueryController()
    {
      this.RetrieveResults = new PassThroughCommand(() => this.GetResults());
      this.Connection = new ConnectionViewModel();
      this.Query = new QueryViewModel();
    }

    public ICommand RetrieveResults { get; private set; }

    public ConnectionViewModel Connection
    {
      get { return this.connection; }
      set
      {
        if (Equals(value, this.connection)) return;
        this.connection = value;
        this.OnPropertyChanged(nameof(this.Connection));
      }
    }

    public QueryViewModel Query
    {
      get { return this.query; }
      set
      {
        if (Equals(value, this.query)) return;
        this.query = value;
        this.OnPropertyChanged(nameof(this.Query));
      }
    }

    public ObservableCollection<LogEntity> Results
    {
      get { return this.results; }
      set
      {
        if (Equals(value, this.results)) return;
        this.results = value;
        this.OnPropertyChanged(nameof(this.Results));
      }
    }

    private async Task GetResults()
    {
      var mapper = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true).CreateMapper();
      var nQuery = mapper.Map<QueryViewModel, AzureNLogQueryDefinition>(this.Query);
      var queryer = new TableQueryer(this.Connection.ConnectionString, this.Connection.TableName);
      var rSet = await queryer.GetLogsAsync(nQuery, this.Query.MaxResults);

      this.Results = new ObservableCollection<LogEntity>(rSet);
    }
  }
}