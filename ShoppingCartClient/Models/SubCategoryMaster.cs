using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class SubCategoryMaster
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}