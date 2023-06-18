namespace TGC.AzureStorage.TestConsole;

internal class TestService
{
	private readonly IAzureStorageContainer _azureStorageContainer1;
	private readonly IAzureStorageContainer _azureStorageContainer2;

	public TestService(IAzureStorageContainerRegistry azureStorageContainerRegistry)
	{
		_azureStorageContainer1 = azureStorageContainerRegistry.GetContainerContext(Program.ContainerName1);
		_azureStorageContainer2 = azureStorageContainerRegistry.GetContainerContext(Program.ContainerName2);
	}

	public void Run()
	{
		_azureStorageContainer1.UploadAsync("Some/path", AccessTier.Cold); //Void - Maybe result?
		_azureStorageContainer1.UploadAsync("Some/path", AccessTier.Archive); //Void - Maybe result?
		_azureStorageContainer1.UploadAsync("Some/path", AccessTier.Hot); //Void - Maybe result?

		var contentAndName = _azureStorageContainer1.DownloadAsync("ID??"); //Void - Maybe result?
		var contentAndName2 = _azureStorageContainer1.DownloadAsync("PathInContainer"); //Void - Maybe result?

		_azureStorageContainer1.DownloadAsync("ID??", "ToPath"); //Void
		_azureStorageContainer1.DownloadAsync("PathInContainer", "ToPath"); //Void

		var fileInfo = _azureStorageContainer1.GetAsync("PathInContainer"); //Returns meta - not data
		var fileInfo2 = _azureStorageContainer1.GetAsync("ID??"); //Returns meta - not data

		var listOfFileInfo = _azureStorageContainer1.ListAsync();
		var listOfFileInfo2 = _azureStorageContainer1.ListAsync(cs => cs == "asdasd");
	}
}
