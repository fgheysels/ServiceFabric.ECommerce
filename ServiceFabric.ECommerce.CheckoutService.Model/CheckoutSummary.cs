using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceFabric.ECommerce.CheckoutService.Model
{
    public class CheckoutSummary
    {
        public List<CheckoutProduct> Products { get; set; }

        public decimal TotalPrice
        {
            get { return Products.Sum(p => p.Price); }
        }

        public DateTime Date { get; set; }
    }
}