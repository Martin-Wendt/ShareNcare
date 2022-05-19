using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Extensions
{
    public static class GroupMemberExtensions
    {
        public static decimal UserTotalPayment(this GroupMember groupMember)
        {
            return groupMember.Payments.Sum(x => x.Payment.Amount);
        }
    }
}
