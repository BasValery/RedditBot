using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            .AddSingleton<IImageProcessor, GPTProcessor>()
            .AddSingleton<IFolderWatcher, FolderWatcher>()
            .BuildServiceProvider();

        var folderWatcher = serviceProvider.GetService<IFolderWatcher>();

        if (folderWatcher == null){
                Console.WriteLine("FolderWatcher is null");
                return;
        }
        
        folderWatcher.StartWatching();
        Console.WriteLine("Waiting new files...");
        Console.ReadLine();
    }
}