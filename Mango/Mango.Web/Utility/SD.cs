﻿namespace Mango.Web.Utility
{
    public sealed class SD
    {
        public static string AuthAPIBase { get; set; }
        public static string CouponAPIBase {  get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
