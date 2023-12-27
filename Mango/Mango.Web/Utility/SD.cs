namespace Mango.Web.Utility
{
    public sealed class SD
    {
        #region API bases
        public static string AuthAPIBase { get; set; }
        public static string CouponAPIBase {  get; set; }
        #endregion

        #region Roles
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        #endregion



        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
