﻿using Microsoft.Extensions.Configuration;
using TelegramScrapper;

class Program
{
    private static IConfigurationRoot? Configuration;

    static async Task Main(string[] args)
    {
        // Retrieve the environment name from the environment variables (default to "Production" if not set)
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var dirrectory = Directory.GetCurrentDirectory();
        Configuration = new ConfigurationBuilder()
            .SetBasePath(dirrectory)
            // Load the base configuration file
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            // Load the environment-specific configuration file (e.g., "AppSettings.Development.json")
            .AddJsonFile($"AppSettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        var telegramConfig = Configuration.GetSection("Telegram").Get<TelegramConfig>();
        var scrapperConfig = Configuration.GetSection("ScrapperConfig").Get<ScrapperConfig>();


        var imageBotService = new ImageBotService(telegramConfig, scrapperConfig);
        await imageBotService.Run();
    }
}