using Microsoft.Extensions.Configuration;

public class FolderWatcher : IFolderWatcher
{
    private readonly string? inputFolder;
    private readonly string? outputFolder;
    private readonly IImageProcessor imageProcessor;

    public FolderWatcher(IConfiguration configuration, IImageProcessor imageProcessor)
    {
        inputFolder = configuration["Folders:Input"];
        outputFolder = configuration["Folders:Output"];
        this.imageProcessor = imageProcessor;

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
    }

    public void StartWatching()
    {
        FileSystemWatcher watcher = new FileSystemWatcher(inputFolder)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            Filter = "*.*",
            EnableRaisingEvents = true
        };
        watcher.Created += async (s, e) => await ProcessNewFile(e.FullPath);
    }

    private async Task ProcessNewFile(string filePath)
    {
        string caption = await imageProcessor.ProcessImageAsync(filePath);
        string newFileName = $"{caption}{Path.GetExtension(filePath)}";
        string newPath = Path.Combine(outputFolder, newFileName);
        File.Copy(filePath, newPath);
        Console.WriteLine($"File saved: {newPath}");
    }
}