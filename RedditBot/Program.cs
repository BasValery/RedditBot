using Microsoft.Extensions.Configuration;
using RedditBot;

class Program
{

    private static IConfigurationRoot? Configuration;
    static async Task Main(string[] args)
    {
        Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .Build();

        var telegramConfig = Configuration.GetSection("Telegram").Get<TelegramConfig>();

        if(telegramConfig == null)
        {
            Console.WriteLine("Telegram configuration was not loaded"); 
            return;
        }

        var imageBotService = new ImageBotService(telegramConfig);
        await imageBotService.Run();
    }
}
