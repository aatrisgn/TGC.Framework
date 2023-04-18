namespace TGC.EFCoreRepositories;
public interface IEFCoreDTO
{
	Guid Id { get; }
	DateTime Created { get; }
	DateTime LastUpdated { get; }
	bool Active { get; set; }
}
