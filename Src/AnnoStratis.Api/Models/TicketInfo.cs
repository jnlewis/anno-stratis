﻿using System;

namespace Anno.Api.Models
{
    public class TicketInfo
    {
        public long? TicketId { get; set; }
        public long? BookingId { get; set; }
        public string CustomerReferenceId { get; set; }
        public string EventReferenceId { get; set; }
        public string TierReferenceId { get; set; }
        public string EventTitle { get; set; }
        public string TierTitle { get; set; }
        public string TicketNo { get; set; }
        public string BookingConfirmationNo { get; set; }
        public string Status { get; set; }
        public decimal? PaidPrice { get; set; }
        public string TicketAddress { get; set; }
    }
}