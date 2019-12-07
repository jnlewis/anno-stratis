﻿using System;

namespace AnnoAPI.Models
{
    public class CancelTicketResponse
    {
        public string EventTitle { get; set; }
        public string TierTitle { get; set; }
        public long? PaidPrice { get; set; }
        public string NewTicketStatus { get; set; }
    }
}