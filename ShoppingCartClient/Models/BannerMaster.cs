using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class BannerMaster
    {
        public string BannerId { get; set; }
        public string Url { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}