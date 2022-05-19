using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Extensions
{
    public static class GroupExtensions
    {
        public static List<PaymentShare> GetOutstandingPaymentShares(this Group group)
        {

            return group.Members.SelectMany(x => x.Payments.SelectMany(x => x.Shares.Where(x => x.Fulfilled == false).ToList()).ToList()).ToList();
        }
    }
}
