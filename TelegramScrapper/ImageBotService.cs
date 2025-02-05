using TL;
using WTelegram;

namespace TelegramScrapper
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
            var messages = await client.Messages_GetHistory(peer, limit: 20);
            foreach (var message in messages.Messages)
            {
                Console.WriteLine(message);
                await SaveImageFromMessageAsync(message, "images", client);
            }
        }

        private async Task SaveImageFromMessageAsync(MessageBase messageBase, string folderPath, Client client)
        {
            if (messageBase is Message message && message.media is MessageMediaPhoto photoMedia)
            {
                var photo = photoMedia.photo as Photo;
                var photoSize = photo?.sizes.OfType<PhotoSize>().LastOrDefault(); // Выбираем наибольший размер
                if (photoSize == null) return;

                var filePath = Path.Combine(folderPath, $"{photo.id}.jpg");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    try {
                        var fileType = await client.DownloadFileAsync(photo, fileStream, photoSize);
                    }

                    catch(Exception ex) {
                        Console.WriteLine(ex.ToString());
                    }
                }
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
