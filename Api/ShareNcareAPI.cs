using Api.Entities;
using Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class ShareNcareAPI
    {
        private readonly IPaymentHandler _paymentHandler;
        private readonly IGroupHandler _groupHandler;
        private readonly IUserHandler _userHandler;

        public ShareNcareAPI()
        {
            _paymentHandler = new PaymentHandler();
            _groupHandler = new GroupHandler();
            _userHandler = new UserHandler();
        }

        //Object Creation
        public Group CreateNewGroup(string name, string description, List<User> members) => _groupHandler.CreateGroup(name, description, members);
        public User CreateUser(string name) => _userHandler.CreateUser(name);
        public Payment CreateNewPayment(decimal amount, string name) => _paymentHandler.CreatePayment(amount,name);

        // Group handling
        public void CloseGroup(Group group) => _groupHandler.CloseGroup(group);
        public void ResolveGroup(Group group) => _groupHandler.ResolveGroup(group);

        // Payment handling
        public void AddPaymentToGroup(Group group, Payment payment, User user) => _paymentHandler.AddPayment(payment, user, group);
        public List<Payment> GetGroupPayments(Group group) => _paymentHandler.GetGroupPayments(group);
        public void GetGroupPaymentsByUser(Group group, User user) => _paymentHandler.GetUserGroupPayments(group, user);
        public Dictionary<User, List<PaymentShare>> GetUserToUserOutstandingShares(Group group, User user1, User user2) => _paymentHandler.GetUserToUserOutstandingShares(group, user1, user2);
        public void PayGroupShare(Group group, PaymentShare shareToPay) => _paymentHandler.PayShare(group, shareToPay);
        public List<PaymentShare> GetOutstandingSharesForUserInGroup(Group group, User user) => _paymentHandler.GetOutstandingSharesForUserInGroup(group, user);
        public void PayAllOutstandingSharesForUser(Group group, User user) => _paymentHandler.PayAllOutstandingSharesForUser(group,user);
        
    }
}

