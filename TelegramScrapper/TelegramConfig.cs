﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramScrapper
{
    public class TelegramConfig
    {
        public string ApiId { get; set; }
        public string ApiHash { get; set; }
        public string PhoneNumber { get; set; }
        public string ChannelName { get; set; }
    }
    public class ScrapperConfig
    {
        public string ImageFolder { get; set; }
    }
}
