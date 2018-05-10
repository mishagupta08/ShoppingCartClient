using ShoppingCartClient.Models;
using ShoppingCartClient.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartClient.Controllers
{
    public class HomeController : Controller
    {
        Repository repository;

        HomeViewModel model;

        // GET: Home
        public async Task<ActionResult> Index()
        {
            repository = new Repository();
            model = new HomeViewModel();
            await LoadCategoryList();
            await LoadSubCategoryList();

            if (string.IsNullOrEmpty(model.SelectedMenu))
            {
                await LoadHomePage();
            }

            return View(model);
        }

        private async Task LoadSubCategoryList()
        {
            model.SubCategoryByCategory = new Dictionary<string, IList<SubCategoryMaster>>();
            if (model.CategoryList != null)
            {
                foreach (var category in model.CategoryList)
                {
                    var subCategory = await this.repository.GetSubCategoryByCategoryId(category.Id);
                    model.SubCategoryByCategory.Add(category.Id + " " + category.Name, subCategory);
                }
            }
        }

        public async Task<ActionResult> AddProductReview(HomeViewModel model)
        {
            if (model == null)
            {
                return Json("Please send complete detail");
            }

            repository = new Repository();
            var res = await repository.AddProductRemark(model.ProductReview);
            return Json(res);
        }

        public async Task<ActionResult> AddProductToCart(string ProductId, string Quantity)
        {
            if (string.IsNullOrEmpty(ProductId))
            {
                return Json("Incomplete detail");
            }

            if (Request.Cookies["UserSetting"] != null && !string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Name"]) && !string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Id"]))
            {
                //save product in db
                repository = new Repository();
                var cartProd = new CartMaster
                {
                    Pid = ProductId,
                    Uid = Request.Cookies["UserSetting"]["Id"],
                    Quantity = Convert.ToInt32(Quantity)
                };

                var res = await repository.AddCartProduct(cartProd);
                if (res.Status)
                {
                    return Json(res.ResponseValue);
                }
                else
                {
                    return Json("0");
                }
            }
            else
            {
                //save product in cookie
                var count = AddProductIdToCartCookie(ProductId, Quantity);
                return Json(count);
            }
        }

        private async Task LoadCategoryList()
        {
            if (Request.Cookies["UserSetting"] != null)
            {
                model.LoginUser = Request.Cookies["UserSetting"]["Name"];
            }
            model.CartCount = await GetCartProductCount();
            model.CategoryList = await this.repository.GetCategoryList();
        }

        public ActionResult Logout()
        {
            if (Response.Cookies["UserSetting"] != null)
            {
                var c = new HttpCookie("UserSetting");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> GetSelectedMenu(string SelectedMenu, string Type, string catId)
        {
            repository = new Repository();
            if (model == null)
            {
                model = new HomeViewModel();
            }
            model.SelectedMenu = SelectedMenu;

            if (string.IsNullOrEmpty(SelectedMenu))
            {
                await LoadHomePage();
                return View("homePartialView", model);
            }
            if (SelectedMenu == Resources.Login)
            {
                await LoadCategoryList();
                await LoadSubCategoryList();
                model.LoginModel = new ShoppingUser();
                return View("Login", model);
            }
            if (Type == Resources.Category || Type == Resources.SubCategory)
            {
                await LoadCategoryDetailView();
                if (Type == Resources.Category)
                {
                    model.CategoryProductList = await this.repository.GetProductList(Resources.ByCatId, catId);
                }
                if (Type == Resources.SubCategory)
                {
                    model.CategoryProductList = await this.repository.GetProductList(Resources.BySubCatId, catId);
                }

                return View("categoryDetailView", model);
            }

            return await Index();
        }

        public async Task LoadHomePage()
        {
            if (model == null)
            {
                model = new HomeViewModel();
            }

            if (model.CategoryList == null)
            {
                model.CategoryList = await this.repository.GetCategoryList();
            }

            model.BannerList = await this.repository.GetBannerList();
            model.FeaturedProductList = await this.repository.GetProductList(Resources.Featured, string.Empty);
            model.RecommendedProductList = await this.repository.GetProductList(Resources.Recommended, string.Empty);
            model.CategoryProductList = await this.repository.GetProductList(Resources.HomePage, string.Empty);
        }

        public async Task LoadCategoryDetailView()
        {
            if (model == null)
            {
                model = new HomeViewModel();
            }

            if (model.CategoryList == null)
            {
                await LoadCategoryList();
            }

            await LoadSubCategoryList();
        }

        public async Task<ActionResult> GetProductDetailView(string ProdId)
        {
            if (model == null)
            {
                model = new HomeViewModel();
            }
            this.repository = new Repository();

            await LoadCategoryList();
            await LoadSubCategoryList();

            var detail = await this.repository.GetProductList(Resources.ProductById, ProdId);
            if (detail != null || detail.Count() > 0)
            {
                model.ProductDetail = detail.FirstOrDefault();
                model.SelectedMenu = model.ProductDetail.CategoryName;
                model.ProductReview = new ProductRemark();
                model.ProductReview.Rating = 4;
                model.ProductReview.ProductId = model.ProductDetail.Id;
                model.ProductImageList = await this.repository.GetProductImagesList(model.ProductDetail.Id);
                model.ProductReviewList = await this.repository.GetProductReviewsList(model.ProductDetail.Id);
                if (model.ProductReviewList == null || model.ProductReviewList.Count == 0)
                {
                    model.ProductReviewList = new List<ProductRemark>();
                    model.ProductDetail.Rating = 0;
                }
                else
                {
                    model.ProductDetail.Rating = (model.ProductReviewList.Sum(re => re.Rating) / model.ProductReviewList.Count()) ?? 0;
                }
            }

            return View("productDetailPage", model);
        }

        /// <summary>
        /// validate user detail
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>validation result</returns>
        public async Task<ActionResult> ValidateUser(HomeViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.LoginModel.Email))
                {
                    return Json("Username is empty.");
                }

                if (string.IsNullOrEmpty(model.LoginModel.Password))
                {
                    return Json("Password is empty.");
                }

                this.repository = new Repository();
                var user = await this.repository.LoginShoppingUser(model.LoginModel);
                if (user == null)
                {
                    return Json("Invalid Username and Password.");
                }
                else
                {
                    HttpCookie myCookie = new HttpCookie("UserSetting");
                    myCookie["Id"] = user.Id;
                    myCookie["Name"] = user.Name;
                    Response.Cookies.Add(myCookie);
                }

                // Session["LoginUserName"] = user.UserName;
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException);
            }

            return Json(string.Empty);
        }

        public async Task<ActionResult> GetCartDetailView()
        {
            model = new HomeViewModel();
            model.CategoryProductList = new List<ProductMaster>();
            this.repository = new Repository();

            if (Request.Cookies["UserSetting"] == null || string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Name"]) || string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Id"]))
            {
                //Not-Login Get count from cookie
                var list = GetCartProductIdsFromCookie();
                if (list == null)
                {
                    return null;
                }
                else
                {
                    foreach (var prod in list)
                    {
                        var detail = prod.Split(':');
                        var prodDetail = await this.repository.GetProductList(Resources.ProductById, detail[0]);
                        if (prodDetail != null && prodDetail.Count > 0)
                        {
                            model.CategoryProductList.Add(prodDetail.FirstOrDefault());
                        }
                    }
                }
            }
            else
            {
                //Login-Get count from database
                repository = new Repository();
                model.CategoryProductList = await repository.GetCartProductList(Request.Cookies["UserSetting"]["Id"], Resources.ByUserId);
            }

            return View("cartView", model);
        }

        public async Task<ActionResult> RegisterUserDetail(HomeViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json("User detail is empty.");
                }

                this.repository = new Repository();
                var response = await this.repository.RegisterShoppingUser(model.LoginModel);
                if (response == null)
                {
                    return Json("Something went wrong. Please try again later.");
                }
                else
                {
                    if (response.Status)
                    {
                        HttpCookie myCookie = new HttpCookie("UserSetting");
                        myCookie["Id"] = response.ResponseValue;
                        myCookie["Name"] = model.LoginModel.Name;
                        Response.Cookies.Add(myCookie);
                        return Json(string.Empty);
                    }
                    else
                    {
                        return Json(response.ResponseValue);
                    }
                }

                // Session["LoginUserName"] = user.UserName;
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException);
            }

            //  return Json(string.Empty);
        }

        public async Task<int> GetCartProductCount()
        {
            if (Request.Cookies["UserSetting"] == null || string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Name"]) || string.IsNullOrEmpty(Request.Cookies["UserSetting"]["Id"]))
            {
                //Not-Login Get count from cookie
                var list = GetCartProductIdsFromCookie();
                if (list == null)
                {
                    return 0;
                }
                else
                {
                    return list.Count;
                }
            }
            else
            {
                //Login-Get count from database
                repository = new Repository();
                var count = await repository.GetCartCount(Request.Cookies["UserSetting"]["Id"], Resources.CartCount);
                return count;
            }
        }

        public int AddProductIdToCartCookie(string prodId, string qty)
        {
            // create our list
            var data = new List<string>();
            data = GetCartProductIdsFromCookie();
            if (data == null)
            {
                data = new List<string>();
            }

            data.Add(prodId + ":" + qty);
            var count = data.Count;

            // flatten it into a single string for storing a cookie
            string dataAsString = data.Aggregate((a, b) => a = a + "," + b);

            // store cookie and redirect
            Response.Cookies.Add(new HttpCookie("cartProduct", dataAsString));
            return count;
        }

        public List<string> GetCartProductIdsFromCookie()
        {
            if (Request.Cookies["cartProduct"] == null)
            {
                return null;
            }
            // get string from cookie
            string dataAsString = Request.Cookies["cartProduct"].Value;

            if (string.IsNullOrEmpty(dataAsString))
            {
                return null;
            }
            // create List to load data into
            List<string> data = new List<string>();

            // Split cookie data into an array and add the elements to the list
            data.AddRange(dataAsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            return data;
        }
    }
}