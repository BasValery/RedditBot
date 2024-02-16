# Telegram Image Downloader & Uploader Project

## Overview

This project enables users to download images from Telegram channels and, in the future, upload them to various online resources. It leverages the `WTelegramClient` library to interact with Telegram's API, allowing for automated image downloading with plans to extend its functionality for versatile uploads.

## Features

- **Download Images:** Automatically download images from specified Telegram channels.
- **Future Upload Functionality:** Planned feature to upload downloaded images to various platforms.
- **Customizable:** Configure the application to use specific Telegram channels and credentials through `AppSettings.json`.

## Getting Started

### Prerequisites

Ensure you have the following prerequisites installed on your system:

- .NET runtime or SDK compatible with the project (typically .NET 5.0 or later).

### Configuration

Before running the project, you need to configure it by editing the `AppSettings.json` file. Fill in the following fields with your Telegram API credentials and target channel information:

```json
{
  "Telegram": {
    "ApiId": "<Your_Telegram_ApiId>",
    "ApiHash": "<Your_Telegram_ApiHash>",
    "PhoneNumber": "<Your_Telegram_PhoneNumber>",
    "ChannelName": "<Target_Telegram_ChannelName>"
  }
}
```

- `ApiId` and `ApiHash`: Unique credentials provided by Telegram for accessing their API.
- `PhoneNumber`: Your Telegram account's phone number in international format.
- `ChannelName`: The name of the Telegram channel from which you wish to download images.

### Obtaining Telegram API Credentials

1. Visit [my.telegram.org](https://my.telegram.org) and log in with your Telegram phone number.
2. Go to 'API development tools' and fill out the form to get your `ApiId` and `ApiHash`.

### Running the Project

To run the project, navigate to the project directory in your terminal or command prompt and execute the following command:

```
dotnet run
```
This command compiles and runs the application, initiating the image download process based on your configuration.

## Future Plans

- **Upload Feature:** Implement functionality to upload downloaded images to various online platforms, enhancing the project's utility for content management and distribution.

## Contributing

Contributions are welcome! If you have ideas for new features or improvements, please feel free to contribute to the project by submitting a pull request.
