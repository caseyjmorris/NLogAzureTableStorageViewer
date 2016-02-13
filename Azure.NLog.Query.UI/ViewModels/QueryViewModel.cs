using System;

namespace Azure.NLog.Query.UI.ViewModels
{
  public class QueryViewModel : BaseViewModel
  {
    private string partitionKey;
    private DateTime? minimumDate;
    private DateTime? maximumDate;
    private string level;
    private int? maxResults;

    public string PartitionKey
    {
      get { return this.partitionKey; }
      set
      {
        if (value == this.partitionKey) return;
        this.partitionKey = value;
        this.OnPropertyChanged(nameof(this.PartitionKey));
      }
    }

    public DateTime? MinimumDate
    {
      get { return this.minimumDate; }
      set
      {
        if (value.Equals(this.minimumDate)) return;
        this.minimumDate = value;
        this.OnPropertyChanged(nameof(this.MinimumDate));
      }
    }

    public DateTime? MaximumDate
    {
      get { return this.maximumDate; }
      set
      {
        if (value.Equals(this.maximumDate)) return;
        this.maximumDate = value;
        this.OnPropertyChanged(nameof(this.MaximumDate));
      }
    }

    public string Level
    {
      get { return this.level; }
      set
      {
        if (value == this.level) return;
        this.level = value;
        this.OnPropertyChanged(nameof(this.Level));
      }
    }

    public int? MaxResults
    {
      get { return this.maxResults; }
      set
      {
        if (value == this.maxResults) return;
        this.maxResults = value;
        this.OnPropertyChanged(nameof(this.MaxResults));
      }
    }
  }
}