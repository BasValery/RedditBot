

using TL;

namespace RedditBot
{
    public class ImageBotService
    {
        private readonly TelegramConfig telegramConfig;
        public ImageBotService(TelegramConfig telegramConfig)
        {
           this.telegramConfig = telegramConfig ?? throw new ArgumentNullException(nameof(telegramConfig));
        }

        public async Task Run()
        {
            using var client = new WTelegram.Client(Config);

            Console.WriteLine("Connecting to Telegram...");
            await client.LoginUserIfNeeded();

            Console.WriteLine($"Accessing the channel: {telegramConfig.ChannelName}");

            var channel = await client.Contacts_ResolveUsername(telegramConfig.ChannelName);
            var peer = new InputPeerChannel(channel.Channel.ID, channel.Channel.access_hash);

            Console.WriteLine("Getting messages...");
            var messages = await client.Messages_GetHistory(peer, limit: 10);
            foreach (var message in messages.Messages)
            {
                Console.WriteLine(message);
            }
        }

        private string Config(string what)
        {
            switch (what)
            {
                case "api_id": return telegramConfig.ApiId; 
                case "api_hash": return telegramConfig.ApiHash;
                case "phone_number": return telegramConfig.PhoneNumber;
                default: return null;
            }
        }
    }
}
