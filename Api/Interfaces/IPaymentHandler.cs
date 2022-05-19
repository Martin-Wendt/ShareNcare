using Api.Entities;
using Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IPaymentHandler
    {
        Payment CreatePayment(decimal amount, string name);
        void AddPayment(Payment payment, User user, Group group);
        void PayShare(Group group, PaymentShare share);
        void PayAllOutstandingSharesForUser(Group group, User user);
        List<Payment> GetGroupPayments(Group group);
        List<GroupPayment> GetUserGroupPayments(Group group, User user);
        Dictionary<User, List<PaymentShare>> GetUserToUserOutstandingShares(Group group, User user1, User user2);
        List<PaymentShare> GetOutstandingSharesBetweenUsersInGroup(Group group, User payee, User userToCompareWith);
        List<PaymentShare> GetOutstandingSharesForUserInGroup(Group group, User user);
    }

    public class PaymentHandler : IPaymentHandler
    {
        public void AddPayment(Payment payment, User user, Group group)
        {
            if (group.State != State.Active) throw new Exception("Unable to add payment, group is no longer active");

            var member = group.Members.SingleOrDefault(x => x.User == user);

            if (member is null) throw new Exception("User not a group member");

            var grpPayment = new GroupPayment(user, payment);

            var paymentShares = CreatePaymentShares(payment, user, group.Members.Select(x => x.User).ToList());

            grpPayment.Shares = paymentShares;


            member.Payments.Add(grpPayment);

        }

        public void PayAllOutstandingSharesForUser(Group group, User user)
        {
            if (group.Members.SingleOrDefault(x => x.User == user) is null) throw new Exception("user not member of group");

            var shares = group.GetOutstandingPaymentShares().Where(x => x.User == user);

            foreach (var share in shares)
            {
                PayShare(group, share);
            }
        }

        public List<Payment> GetGroupPayments(Group group)
        {
            List<Payment> payments = new();
            Console.WriteLine("Total group payments: ");
            foreach (var member in group.Members)
            {
                foreach (var payment in member.Payments)
                {
                    payments.Add(payment.Payment);
                }
            }
            return payments;
        }

        public List<GroupPayment> GetUserGroupPayments(Group group, User user)
        {

            var groupMember = group.Members.SingleOrDefault(x => x.User == user);
            if (groupMember is null) throw new Exception("user not member of group");

            foreach (var payment in groupMember.Payments)
            {
                Console.WriteLine(payment.Payment.ToString());
            }

            Console.WriteLine("For a total of: " + groupMember.UserTotalPayment());
            Console.WriteLine();

            return groupMember.Payments;
        }

        public Dictionary<User, List<PaymentShare>> GetUserToUserOutstandingShares(Group group, User user1, User user2)
        {
            var groupPaymentUser1 = group.Members.SingleOrDefault(x => x.User == user1);
            var groupPaymentUser2 = group.Members.SingleOrDefault(x => x.User == user2);

            if (groupPaymentUser1 is null) throw new Exception("user1 not member of group");
            if (groupPaymentUser2 is null) throw new Exception("user2 not member of group");

            var user1SharesOutstandingToUser2 = group.GetOutstandingPaymentShares().Where(x => x.Payee == user2 && x.User == user1).ToList();
            var user2SharesOutstandingToUser1 = group.GetOutstandingPaymentShares().Where(x => x.Payee == user1 && x.User == user2).ToList();


            foreach (var share in user1SharesOutstandingToUser2)
            {
                share.ToString();
            }
            foreach (var share in user2SharesOutstandingToUser1)
            {
                share.ToString();
            }

            var returnValue = new Dictionary<User, List<PaymentShare>>();
            returnValue.Add(user1, user1SharesOutstandingToUser2);
            returnValue.Add(user2, user2SharesOutstandingToUser1);

            return returnValue;

        }

        public List<PaymentShare> GetOutstandingSharesBetweenUsersInGroup(Group group, User payee, User userToCompareWith)
        {
            var groupUser1 = group.Members.SingleOrDefault(x => x.User == payee);
            var groupUser2 = group.Members.SingleOrDefault(x => x.User == userToCompareWith);

            if (groupUser1 is null) throw new Exception("payee not member of group");
            if (groupUser2 is null) throw new Exception("userToCompareWith not member of group");

            var comparerSharesOwedToPayee = group.GetOutstandingPaymentShares().Where(x => x.Payee == payee && x.User == userToCompareWith).ToList();

            foreach (var share in comparerSharesOwedToPayee)
            {
                Console.WriteLine(share.ToString());
            }

            return comparerSharesOwedToPayee;
        }

        public List<PaymentShare> GetOutstandingSharesForUserInGroup(Group group, User user)
        {
            var userToManipulate = group.Members.SingleOrDefault(x => x.User == user);
            if (userToManipulate is null) throw new Exception("user not member of group");


            var shares = group.GetOutstandingPaymentShares().Where(x => x.User == user).ToList();
            foreach (var share in shares)
            {
                Console.WriteLine(share.ToString());
            }

            return shares;

        }

        public void PayShare(Group group, PaymentShare share)
        {
            var shareToPay = group.GetOutstandingPaymentShares().Find(x => x == share);
            if (shareToPay is null) throw new Exception("paymentshare not found in group");


            shareToPay.Fulfilled = true;
        }

        public Payment CreatePayment(decimal amount, string name)
        {
            return new Payment(amount, name);
        }

        private List<PaymentShare> CreatePaymentShares(Payment payment, User payee, List<User> groupMembers)
        {
            var sharesToReturn = new List<PaymentShare>();
            var amountPerMember = payment.Amount / groupMembers.Count;

            if (groupMembers.Exists(x => x == payee)) groupMembers.Remove(payee);

            foreach (var member in groupMembers)
            {
                var paymentShare = new PaymentShare(member, amountPerMember, payment, payee);
                sharesToReturn.Add(paymentShare);
            }

            return sharesToReturn;
        }
    }
}
