﻿using System;

namespace AnnoAPI.Models
{
    public class Customer
    {
        public long? CustomerId { get; set; }
        public string RefId { get; set; }
        public string CustomerAddress { get; set; }
        public long? WalletBalance { get; set; }
        public string WalletAddress { get; set; }
    }
}