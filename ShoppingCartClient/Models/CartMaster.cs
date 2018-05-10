using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class CartMaster
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string Pid { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}