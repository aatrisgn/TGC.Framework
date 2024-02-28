using Microsoft.EntityFrameworkCore;

namespace TGC.Cosmos;
internal class CosmosDbContext : DbContext
{
	private readonly ICosmosConfiguration _configuration;
	public CosmosDbContext(ICosmosConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	=> optionsBuilder.UseCosmos(
		_configuration.ConnectionString!,
		databaseName: _configuration.DatabaseName);
}
