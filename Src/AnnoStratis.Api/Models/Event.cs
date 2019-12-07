﻿using System;

namespace AnnoAPI.Models
{
    public class Event
    {
        public long? EventId { get; set; }
        public string ReferenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public string EventAddress { get; set; }
        public long? WalletBalance { get; set; }
        public string WalletAddress { get; set; }
    }
}