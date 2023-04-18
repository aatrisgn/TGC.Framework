namespace TGC.AzureTableStorage;

[AttributeUsage(AttributeTargets.Class)]
public class TableItemAttribute : Attribute
{
	public string TableName { get; set; }
	public string PartitionKey { get; set; }

	public TableItemAttribute(string tableName)
	{
		this.TableName = tableName;
	}

	public TableItemAttribute(string tableName, string partitionKey)
	{
		this.TableName = tableName;
		this.PartitionKey = partitionKey;
	}
}
