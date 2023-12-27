namespace Mango.Web.Utility
{
    public sealed class SD
    {
        #region API bases
        public static string AuthAPIBase { get; set; }
        public static string CouponAPIBase {  get; set; }
        #endregion

        public enum Role
        {
            Admin,
            Customer,
        }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
