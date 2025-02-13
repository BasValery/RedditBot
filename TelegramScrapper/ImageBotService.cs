using TL;
using WTelegram;

namespace TelegramScrapper
{
    public class ImageBotService
    {
        private readonly TelegramConfig telegramConfig;
        private readonly ScrapperConfig scrapperConfig;

        public ImageBotService(TelegramConfig? telegramConfig, ScrapperConfig? scrapperConfig)
        {
            this.telegramConfig = telegramConfig ?? throw new ArgumentNullException(nameof(telegramConfig));
            this.scrapperConfig = scrapperConfig ?? throw new ArgumentNullException(nameof(scrapperConfig));
        }

        public async Task Run()
        {
            using var client = new WTelegram.Client(Config);

            Console.WriteLine("Connecting to Telegram...");
            // Login the user if it's not done already
            await client.LoginUserIfNeeded();

            Console.WriteLine($"Accessing the channel: {telegramConfig.ChannelName}");

            var channel = await client.Contacts_ResolveUsername(telegramConfig.ChannelName);
            var channelPeer = channel.Channel; // An object of type Channel

            Console.WriteLine("Start listening for new messages...");

            client.OnUpdate += async update => await Client_OnUpdate(update, channelPeer, client);

            // Keep the task alive indefinitely, so the app doesn't exit
            await Task.Delay(-1);
        }

        /// <summary>
        /// This method will be called on each new update from Telegram.
        /// </summary>
        private async Task Client_OnUpdate(UpdatesBase update, Channel channelPeer, Client client)
        {
            foreach (var updateMsg in update.UpdateList)
            {
                
                if (updateMsg is UpdateNewChannelMessage newMessage)
                {
                   // Select only new messages without a caption to avoid advertisements
                    if( newMessage.message.Peer.ID == channelPeer.ID
                        && newMessage.message is Message messageInfo
                        && string.IsNullOrEmpty(messageInfo.message)
                        && messageInfo.media is MessageMediaPhoto messageMediaPhoto
                        && messageMediaPhoto is not null
                    )
                    {
                        await SaveImageFromMessageAsync(messageInfo, client);
                    }
                }
            }
        }

        private async Task SaveImageFromMessageAsync(Message message, Client client)
        {
            if (message.media is MessageMediaPhoto photoMedia)
            {
                // Extract the photo object
                var photo = photoMedia.photo as Photo;
                if (photo == null) return;

                // Typically, the largest photo is the last in the sizes array
                var photoSize = photo.sizes.OfType<PhotoSize>().LastOrDefault();
                if (photoSize == null) return;

                // Use the photo.id as the file name + ".jpg"
                // (you can detect the actual format if needed)
                if (!Directory.Exists(scrapperConfig.ImageFolder))
                {
                    Directory.CreateDirectory(scrapperConfig.ImageFolder);
                }

                var filePath = Path.Combine(scrapperConfig.ImageFolder, $"{photo.id}.jpg");

                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                try
                {
                    // DownloadFileAsync can return the file type, but we'll just store everything as .jpg
                    var fileType = await client.DownloadFileAsync(photo, fileStream, photoSize);
                    Console.WriteLine($"Photo saved: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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
