using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class UserShippingAddress
    {
        public string Id { get; set; }
        public string pincode { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string UserId { get; set; }
        public Nullable<bool> DefaultAddress { get; set; }
    }
}