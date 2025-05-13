﻿using System;
namespace ECommerceAPI.DTOs
{
    public class ShippingAddressDto
    {
        public Guid Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
