using Newtonsoft.Json;
using ShoppingCartClient.Models;
using ShoppingCartClient.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShoppingCartClient
{
    public class Repository
    {
        private string ApiUrl = "http://shopapi.bisplindia.in/api/Admin/";

        #region bannerAction

        private string GetBannerListAction = "ManageBanner/List";

        #endregion bannerAction

        #region CategoryAction

        private string GetCategoryListAction = "ManageCategory/ActiveList";

        #endregion CategoryAction

        #region ShoppingCartAction

        private string ShoppingCartListAction = "ManageShoppingCart/";

        private string DeleteCartProductAction = "ManageShoppingCart/Delete";

        private string AddCartProductAction = "ManageShoppingCart/Add";

        #endregion ShoppingCartAction

        #region ShoppingUserAction

        private string GetShoppingUserByIdAction = "ManageShoppingUser/UserById";

        private string LoginShoppingUserAction = "ManageShoppingUser/Login";

        private string RegisterShoppingUserAction = "ManageShoppingUser/Add";

        #endregion ShoppingUserAction

        #region SubCategoryAction

        private string GetSubCategoryByCategoryIdAction = "GetSubCategoryByCategoryId/";

        #endregion SubCategoryAction

        #region ProductAction

        private string GetProductByFilterListAction = "ManageProduct/";

        private string GetProductImagesAction = "ManageProductImages/ByProductId/";

        private string AddProductReviewsAction = "ManageProductReviews/Add/0";

        private string GetProductReviewsByIdAction = "ManageProductReviews/ByProductId/";

        private string GetProductRatingAction = "ManageProductReviews/Rating/";

        #endregion ProductAction

        #region ShippingAddressAction

        private string ManageShippingAddressByFilterAction = "ManageShippingAddress/";

        private string GetAddressByIdAction = "ManageShippingAddress/AddressById";

        private string GetAddressListByUserIdAction = "ManageShippingAddress/AddressByUserId";

        #endregion ShippingAddressAction

        #region CategoryDetailAction

        public async Task<List<CategoryMaster>> GetCategoryList()
        {
            var result = await CallPostFunction(string.Empty, GetCategoryListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<List<CategoryMaster>>(result.ResponseValue);
                return detail;
            }
        }

        #endregion CategoryDetailAction

        #region SubCategoryDetailAction

        public async Task<List<SubCategoryMaster>> GetSubCategoryByCategoryId(string id)
        {
            var result = await CallPostFunction(string.Empty, GetSubCategoryByCategoryIdAction + id);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<List<SubCategoryMaster>>(result.ResponseValue);
                return detail;
            }
        }

        #endregion SubCategoryDetailAction

        #region BannerActionDetail

        public async Task<List<BannerMaster>> GetBannerList()
        {
            var result = await CallPostFunction(string.Empty, GetBannerListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<List<BannerMaster>>(result.ResponseValue);
                return detail;
            }
        }

        #endregion BannerActionDetail

        #region ProductActionDetail

        public async Task<string> AddProductRemark(ProductRemark detail)
        {
            var data = JsonConvert.SerializeObject(detail);
            var result = await CallPostFunction(data, AddProductReviewsAction);
            if (result == null || !result.Status)
            {
                return result.ResponseValue;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<int> GetProductAverageRating(string id)
        {
            var result = await CallPostFunction(string.Empty, GetProductRatingAction + id);
            if (result == null || !result.Status)
            {
                return 0;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<int>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<List<ProductRemark>> GetProductReviewsList(string id)
        {
            var result = await CallPostFunction(string.Empty, GetProductReviewsByIdAction + id);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<List<ProductRemark>>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<List<ProductImage>> GetProductImagesList(string id)
        {
            var result = await CallPostFunction(string.Empty, GetProductImagesAction + id);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<List<ProductImage>>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<List<ProductMaster>> GetProductList(string action, string Id)
        {
            var query = string.Empty;
            if (action == Resources.ByCatId)
            {
                query = "{\"CategoryId\":\"IdValue\"}";
                query = query.Replace("IdValue", Id);
            }
            else if (action == Resources.BySubCatId)
            {
                query = "{\"SubCategoryId\":\"IdValue\"}";
                query = query.Replace("IdValue", Id);
            }
            else if (action == Resources.ProductById)
            {
                query = "{\"Id\":\"IdValue\"}";
                query = query.Replace("IdValue", Id);
            }

            var method = GetProductByFilterListAction + action;
            var result = await CallPostFunction(query, method);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = new List<ProductMaster>();
                if (action == Resources.ProductById)
                {
                    var pdetail = JsonConvert.DeserializeObject<ProductMaster>(result.ResponseValue);
                    detail.Add(pdetail);
                }
                else
                {
                    detail = JsonConvert.DeserializeObject<List<ProductMaster>>(result.ResponseValue);
                }

                return detail;
            }
        }

        #endregion ProductActionDetail


        #region ShippingAddressActionDetail

        public async Task<Response> AddEditShipping(UserShippingAddress detail, string action)
        {
            var data = JsonConvert.SerializeObject(detail);
            var result = await CallPostFunction(data, action);

            return result;
        }

        public async Task<UserShippingAddress> GetAddressById(string Id)
        {
            var query = string.Empty;
            query = "{\"Id\":\"IdValue\"}";
            query = query.Replace("IdValue", Id);
            var result = await CallPostFunction(query, GetAddressByIdAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = new UserShippingAddress();
                detail = JsonConvert.DeserializeObject<UserShippingAddress>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<List<UserShippingAddress>> GetAddressListByUserId(string Id)
        {
            var query = string.Empty;
            query = "{\"UserId\":\"IdValue\"}";
            query = query.Replace("IdValue", Id);
            var result = await CallPostFunction(query, GetAddressListByUserIdAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = new List<UserShippingAddress>();
                detail = JsonConvert.DeserializeObject<List<UserShippingAddress>>(result.ResponseValue);
                return detail;
            }
        }

        #endregion ShippingAddressActionDetail

        #region CartActionDetail

        public async Task<List<ProductMaster>> GetCartProductList(string Id, string action)
        {
            var query = string.Empty;
            query = "{\"Uid\":\"IdValue\"}";
            query = query.Replace("IdValue", Id);
            var result = await CallPostFunction(query, ShoppingCartListAction + action);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var detail = new List<ProductMaster>();
                detail = JsonConvert.DeserializeObject<List<ProductMaster>>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<int> GetCartCount(string Id, string action)
        {
            var query = string.Empty;
            query = "{\"Uid\":\"IdValue\"}";
            query = query.Replace("IdValue", Id);
            var result = await CallPostFunction(query, ShoppingCartListAction + action);
            if (result == null || !result.Status)
            {
                return 0;
            }
            else
            {
                var detail = JsonConvert.DeserializeObject<int>(result.ResponseValue);
                return detail;
            }
        }

        public async Task<Response> AddCartProduct(CartMaster detail)
        {
            var data = JsonConvert.SerializeObject(detail);
            var result = await CallPostFunction(data, AddCartProductAction);

            return result;
        }

        public async Task<string> DeleteCartProductById(string id)
        {
            var query = "{\"Id\":\"IdValue\"}";
            query = query.Replace("IdValue", id);
            var result = await CallPostFunction(query, DeleteCartProductAction);
            if (result == null || !result.Status)
            {
                return result.ResponseValue;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion CartActionDetail

        #region ShoppingUserActionDetail

        public async Task<Response> RegisterShoppingUser(ShoppingUser detail)
        {
            var data = JsonConvert.SerializeObject(detail);

            var result = await CallPostFunction(data, RegisterShoppingUserAction);
            return result;
        }

        public async Task<ShoppingUser> LoginShoppingUser(ShoppingUser detail)
        {
            var data = JsonConvert.SerializeObject(detail);

            var result = await CallPostFunction(data, LoginShoppingUserAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var obj = JsonConvert.DeserializeObject<ShoppingUser>(result.ResponseValue);
                return obj;
            }
        }

        #endregion ShoppingUserActionDetail

        private async Task<Response> CallPostFunction(string detail, string action)
        {
            using (var httpClient = new HttpClient())
            {
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(detail, Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync(ApiUrl + action, httpContent);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Response>(responseContent);

                    return result;
                }
            }

            return null;
        }
    }
}