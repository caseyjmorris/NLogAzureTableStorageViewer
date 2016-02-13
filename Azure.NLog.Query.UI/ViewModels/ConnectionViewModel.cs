using System.ComponentModel.DataAnnotations;

namespace Azure.NLog.Query.UI.ViewModels
{
  public class ConnectionViewModel : BaseViewModel
  {
    private string tableName;
    private string connectionString;

    [Required]
    public string ConnectionString
    {
      get { return this.connectionString; }
      set
      {
        if (value == this.connectionString) return;
        this.connectionString = value;
        this.OnPropertyChanged(nameof(this.ConnectionString));
      }
    }

    [Required]
    public string TableName
    {
      get { return this.tableName; }
      set
      {
        if (value == this.tableName) return;
        this.tableName = value;
        this.OnPropertyChanged(nameof(this.TableName));
      }
    }
  }
}