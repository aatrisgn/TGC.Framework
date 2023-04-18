using Microsoft.EntityFrameworkCore;
using TGC.EFCoreRepositories.IoC;

namespace TGC.EFCoreRepositories;
public class EFCoreContext : DbContext
{
	public EFCoreContext()
	{
	}

	public EFCoreContext(DbContextOptions<EFCoreContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		modelBuilder.RegisterAllEntities(assemblies);
	}
}
