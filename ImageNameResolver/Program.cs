using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
    .Build();

var appSettings = config.GetSection("AppSettings").Get<AppSettings>();

if (appSettings != null)
{
    Console.WriteLine($"Configured path: {appSettings.Path}");
}
else
{
    Console.WriteLine("Failed to load configuration.");
}
