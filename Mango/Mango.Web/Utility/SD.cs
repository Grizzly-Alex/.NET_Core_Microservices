namespace Mango.Web.Utility
{
    public sealed class SD
    {
        #region API bases
        public static string AuthAPIBase { get; set; }
        public static string CouponAPIBase {  get; set; }
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string OrderAPIBase { get; set; }
        #endregion

        #region Roles
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        #endregion

        public const string TokenCookie = "JWTToken";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }

        public static void Initializing(IConfiguration config)
        {
            ProductAPIBase = config["ServiceUrls:ProductAPI"];
            CouponAPIBase = config["ServiceUrls:CouponAPI"];
            AuthAPIBase = config["ServiceUrls:AuthAPI"];
            ShoppingCartAPIBase = config["ServiceUrls:ShoppingCartAPI"];
            OrderAPIBase = config["ServiceUrls:OrderAPI"];
        }
    }
}
