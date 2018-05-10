using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public partial class CategoryMaster
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string BannerImage { get; set; }
    }
}