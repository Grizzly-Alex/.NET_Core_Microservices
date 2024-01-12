﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public sealed class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
