using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartClient.Models
{
    public class HomeViewModel
    {
        public string SelectedMenu { get; set; }

        public ProductRemark ProductReview { get; set; }

        public IList<ProductRemark> ProductReviewList { get; set; }

        public IList<BannerMaster> BannerList { get; set; }

        public IList<ProductMaster> FeaturedProductList { get; set; }

        public IList<ProductMaster> RecommendedProductList { get; set; }

        public IList<ProductImage> ProductImageList { get; set; }

        public IList<CategoryMaster> CategoryList { get; set; }

        public IList<ProductMaster> CategoryProductList { get; set; }

        public Dictionary<string, IList<SubCategoryMaster>> SubCategoryByCategory { get; set; }

        public ProductMaster ProductDetail { get; set; }

        public ShoppingUser LoginModel { get; set; }

        public string LoginUser { get; set; }

        public int CartCount { get; set; }

    }
}