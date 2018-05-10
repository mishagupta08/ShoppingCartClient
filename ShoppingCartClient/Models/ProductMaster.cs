using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class ProductMaster
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string SubCategoryId { get; set; }
        public string BrandId { get; set; }
        public Nullable<decimal> Dp { get; set; }
        public Nullable<decimal> Bv { get; set; }
        public Nullable<bool> HomePage { get; set; }
        public Nullable<bool> Recommended { get; set; }
        public Nullable<bool> Featured { get; set; }
        public string DetailDescription { get; set; }
        public string ProductImage { get; set; }
        public string BrandName { get; set; }
        public int Rating { get; set; }
        public string SubCategoryName { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}