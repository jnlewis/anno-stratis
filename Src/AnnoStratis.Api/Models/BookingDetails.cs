﻿using System;
using System.Collections.Generic;

namespace Anno.Api.Models
{
    public class BookingDetails
    {
        public string UserReferenceId { get; set; }
        public string EventReferenceId { get; set; }
        public string ConfirmationNumber { get; set; }
        public List<TicketInfo> Tickets { get; set; }
    }
}