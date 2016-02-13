using System;

namespace Azure.NLog.Query
{
  /// <summary>
  /// The body of a query against NLog Azure database
  /// </summary>
  public class AzureNLogQueryDefinition
  {
    public string PartitionKey { get; set; }

    public DateTime? MinimumDate { get; set; }

    public DateTime? MaximumDate { get; set; }

    public string Level { get; set; }

    public int? MaxResults { get; set; }
  }
}