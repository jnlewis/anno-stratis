﻿using System;

namespace Anno.Api.Models
{
    public class Booking
    {
        public string UserReferenceId { get; set; }
        public string EventReferenceId { get; set; }
        public string EventTitle { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}