namespace Mango.CouponAPI.Utility
{
    public sealed class SD
    {
        public static string StripeSessionKey {  get; set; }

        public static void Initializing(IConfiguration config)
        {
            StripeSessionKey = config["Stripe:SecretKey"];
        }
    }
}
