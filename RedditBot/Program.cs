using Microsoft.Extensions.Configuration;
using RedditBot;

class Program
{
    private static IConfigurationRoot? Configuration;

    static async Task Main(string[] args)
    {
        // Retrieve the environment name from the environment variables (default to "Production" if not set)
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            // Load the base configuration file
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            // Load the environment-specific configuration file (e.g., "AppSettings.Development.json")
            .AddJsonFile($"AppSettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        var telegramConfig = Configuration.GetSection("Telegram").Get<TelegramConfig>();

        if (telegramConfig == null)
        {
            Console.WriteLine("Telegram configuration was not loaded");
            return;
        }

        var imageBotService = new ImageBotService(telegramConfig);
        await imageBotService.Run();
    }
}