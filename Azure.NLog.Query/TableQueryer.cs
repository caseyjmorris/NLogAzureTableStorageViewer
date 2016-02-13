using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NLog.Extensions.AzureTableStorage;

namespace Azure.NLog.Query
{
  public class TableQueryer
  {
    private readonly string connectionString;

    private readonly string tableName;

    public TableQueryer(string connectionString, string tableName)
    {
      this.connectionString = connectionString;
      this.tableName = tableName;
    }

    public IEnumerable<LogEntity> GetLogs(AzureNLogQueryDefinition query)
    {
      var queryBody = this.MapQuery(query);

      return this.GetTable().ExecuteQuery(queryBody);
    }

    public async Task<IEnumerable<LogEntity>> GetLogsAsync(AzureNLogQueryDefinition query, int? maxResults)
    {
      var queryBody = this.MapQuery(query);

      var table = this.GetTable();

      var results = new List<LogEntity>();

      TableContinuationToken token = null;

      do
      {
        var seg = await table.ExecuteQuerySegmentedAsync(queryBody, token);
        token = seg.ContinuationToken;
        results.AddRange(seg.Results);
      } while (token != null && results.Count < (maxResults ?? int.MaxValue));

      return results.OrderByDescending(r => r.LogTimeStamp).Take(maxResults ?? int.MaxValue);
    }

    private TableQuery<LogEntity> MapQuery(AzureNLogQueryDefinition originalQuery)
    {
      var query = new TableQuery<LogEntity>();
      var filters = new[]
      {
        this.GetPartitionKeyFilter(originalQuery), this.GetMinDateFilter(originalQuery),
        this.GetMaxDateFilter(originalQuery), this.GetLevelFilter(originalQuery)
      }
        .Where(f => !string.IsNullOrWhiteSpace(f))
        .ToArray();

      if (!filters.Any())
      {
        return query;
      }

      var querySt = filters[0];

      for (var i = 1; i < filters.Length; i++)
      {
        querySt = TableQuery.CombineFilters(querySt, TableOperators.And, filters[i]);
      }

      return query.Where(querySt);
    }

    private string GetPartitionKeyFilter(AzureNLogQueryDefinition originalQuery)
    {
      return string.IsNullOrWhiteSpace(originalQuery.PartitionKey)
        ? null
        : this.GetStringStartsWithQuery(nameof(LogEntity.PartitionKey), originalQuery.PartitionKey);
    }

    private string GetMinDateFilter(AzureNLogQueryDefinition originalQuery)
    {
      if (originalQuery.MinimumDate == null)
      {
        return null;
      }

      return TableQuery.GenerateFilterCondition(nameof(LogEntity.RowKey), QueryComparisons.GreaterThanOrEqual,
        this.StringifyDate(originalQuery.MinimumDate.Value));
    }

    private string GetMaxDateFilter(AzureNLogQueryDefinition originalQuery)
    {
      return originalQuery.MaximumDate == null
        ? null
        : TableQuery.GenerateFilterCondition(nameof(LogEntity.RowKey), QueryComparisons.LessThanOrEqual,
          this.StringifyDate(originalQuery.MaximumDate.Value));
    }

    private string GetLevelFilter(AzureNLogQueryDefinition originalQuery)
    {
      return originalQuery.Level == null
        ? null
        : TableQuery.GenerateFilterCondition(nameof(LogEntity.Level), QueryComparisons.Equal, originalQuery.Level);
    }

    private string StringifyDate(DateTime date)
    {
      // Matches method used by NLog ext

      return (DateTime.MaxValue.Ticks - date.Ticks).ToString("D");
    }

    private string GetStringStartsWithQuery(string property, string startsWithValue)
    {
      // see:  https://alexandrebrisebois.wordpress.com/2014/10/30/azure-table-storage-using-startswith-to-filter-on-rowkeys/
      var strArr = startsWithValue.ToArray();

      strArr[strArr.Length - 1]++;

      var maxStr = new string(strArr);

      return
        TableQuery.CombineFilters(
          TableQuery.GenerateFilterCondition(property, QueryComparisons.GreaterThanOrEqual,
            startsWithValue), TableOperators.And,
          TableQuery.GenerateFilterCondition(property, QueryComparisons.LessThan, maxStr));
    }

    private CloudTable GetTable()
    {
      var storageAccount = CloudStorageAccount.Parse(this.connectionString);

      var tableClient = storageAccount.CreateCloudTableClient();

      return tableClient.GetTableReference(this.tableName);
    }
  }
}