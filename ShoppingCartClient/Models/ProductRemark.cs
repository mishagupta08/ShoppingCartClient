using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class ProductRemark
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<short> Rating { get; set; }
        public string Review { get; set; }
        public string ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}