using Api.Entities;
using Api.Interfaces;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareNcare.Test
{
    public class PaymentHandlerShould
    {
        private readonly PaymentHandler sut;

        public PaymentHandlerShould()
        {
            sut = new PaymentHandler();
        }
        [Theory]
        [AutoData]
        public void CreatePayment(decimal amount, string name)
        {

            var payment = sut.CreatePayment(amount, name);

            Assert.NotNull(payment);
            Assert.Equal(amount, payment.Amount);
            Assert.Equal(name, payment.Name);
        }

        [Theory]
        [AutoData]
        public void AddPaymentToGroup(Payment payment, Group group)
        {
            var grpMember = group.Members.First().User;

            sut.AddPayment(payment, grpMember, group);

        }

        [Theory]
        [AutoData]
        public void CannotAddPaymentToGroupUserNotMemberOfGroup(Payment payment, User user, Group group)
        {
            var grpMember = group.Members.First().User;

            sut.Invoking(x => x.AddPayment(payment, user, group)).Should().Throw<Exception>().WithMessage("User not a group member");

        }

        [Theory]
        [AutoData]
        public void CannotAddPaymentToGroupStateIsNotActive(Payment payment, Group group)
        {
            var grpMember = group.Members.First().User;
            var grpHandler = new GroupHandler();

            grpHandler.ResolveGroup(group);

            sut.Invoking(x => x.AddPayment(payment, grpMember, group)).Should().Throw<Exception>().WithMessage("Unable to add payment, group is no longer active");

        }

        [Theory]
        [AutoData]
        public void GetAllOutstandingPaymentSharesForUser(Group group, Payment payment)
        {
            sut.AddPayment(payment, group.Members.First().User, group);

            var paymentshares = sut.GetOutstandingSharesForUserInGroup(group, group.Members.Last().User);

            paymentshares.Should().NotBeNullOrEmpty();
            paymentshares.Should().HaveCount(1);
            paymentshares.First().Payment.Should().BeSameAs(payment);

        }

        [Theory]
        [AutoData]
        public void CannotGetAllOutstandingPaymentSharesForUserNotInGroup(Group group, User user)
        {
            sut.Invoking(x => x.GetOutstandingSharesForUserInGroup(group, user)).Should().Throw<Exception>().WithMessage("user not member of group");
        }

        [Theory]
        [AutoData]
        public void PayShare(Group group, Payment payment)
        {
            sut.AddPayment(payment, group.Members.First().User, group);
            var paymentShare = sut.GetOutstandingSharesForUserInGroup(group, group.Members.Last().User);
            sut.PayShare(group, paymentShare.First());


            var paymentShareAfterPay = sut.GetOutstandingSharesForUserInGroup(group, group.Members.Last().User);

            paymentShareAfterPay.Should().BeEmpty();

        }

        [Theory]
        [AutoData]
        public void CannotPayShareNotInGroup(Group group, PaymentShare paymentShare)
        {
            sut.Invoking(x => x.PayShare(group, paymentShare)).Should().Throw<Exception>().WithMessage("paymentshare not found in group");
        }



        [Theory]
        [AutoData]
        public void PayAllOutstandingSharesForUser(Group group, List<Payment> payments)
        {
            var userThatAddPayments = group.Members.First().User;
            var userToPayAllShare = group.Members.Last().User;
            foreach (var payment in payments)
            {
                sut.AddPayment(payment, userThatAddPayments, group);
            }

            var shareToPay = sut.GetOutstandingSharesForUserInGroup(group, userToPayAllShare);
            sut.PayAllOutstandingSharesForUser(group, userToPayAllShare);
            var sharesAfterPay = sut.GetOutstandingSharesForUserInGroup(group, userToPayAllShare);

            shareToPay.Should().NotBeEmpty();
            sharesAfterPay.Should().BeEmpty();
        }
        [Theory]
        [AutoData]
        public void CannotPayAllOutstandingSharesForUserNotInGroup(Group group, User user)
        {
            sut.Invoking(x => x.PayAllOutstandingSharesForUser(group, user)).Should().Throw<Exception>().WithMessage("user not member of group");
        }

        [Theory]
        [AutoData]
        public void GetAllOutstandingShareUserToUser(Group group, List<Payment> payments)
        {
            var userThatAddPayments = group.Members.First().User;
            var userToCompareWith = group.Members.Last().User;
            foreach (var payment in payments)
            {
                sut.AddPayment(payment, userThatAddPayments, group);
            }

            var sharesOutstanding = sut.GetOutstandingSharesBetweenUsersInGroup(group, userThatAddPayments, userToCompareWith);

            sharesOutstanding.Should().HaveCount(payments.Count);
            sharesOutstanding.Select(x => x.Payment).ToList().Should().BeEquivalentTo(payments);
        }

        [Theory]
        [AutoData]
        public void GetGroupPayments(Group group, List<Payment> payments)
        {
            foreach (var payment in payments)
            {
                sut.AddPayment(payment, group.Members.First().User, group);

            }

            var grpPayments = sut.GetGroupPayments(group);

            grpPayments.Should().NotBeEmpty();
            grpPayments.Should().BeEquivalentTo(payments);
        }



        [Theory]
        [AutoData]
        public void GetGroupPaymentsForUser(Group group, List<Payment> paymentsToFind, List<Payment> paymentsNotToFind)
        {
            foreach (var payment in paymentsToFind)
            {
                sut.AddPayment(payment, group.Members.First().User, group);

            }
            foreach (var payment in paymentsNotToFind)
            {
                sut.AddPayment(payment, group.Members.Last().User, group);

            }

            var payments = sut.GetUserGroupPayments(group, group.Members.First().User);

            payments.Should().NotBeEmpty();
            payments.Select(x => x.Payment).ToList().Should().BeSubsetOf(paymentsToFind);

        }

        [Theory]
        [AutoData]
        public void CannotGetGroupPaymentsForUserNotInGRoup(Group group, User userNotInGroup)
        {          
            sut.Invoking(x => x.GetUserGroupPayments(group,userNotInGroup)).Should().Throw<Exception>().WithMessage("user not member of group");
        }

        [Theory]
        [AutoData]
        public void GetOutstandsharesBetweenUsers(Group group, List<Payment> firstUserPayments, List<Payment> lastUserPayments)
        {
            var firstUser = group.Members.First().User;
            var lastUser = group.Members.Last().User;
            foreach (var payment in firstUserPayments)
            {
                sut.AddPayment(payment, firstUser, group);

            }
            foreach (var payment in lastUserPayments)
            {
                sut.AddPayment(payment, lastUser, group);

            }

            var sharesBetweenUsers = sut.GetUserToUserOutstandingShares(group, firstUser, lastUser);
            sharesBetweenUsers.Should().NotBeEmpty();

            var firstUserShare = sharesBetweenUsers[firstUser];
            var lastUserShare = sharesBetweenUsers[lastUser];

            firstUserShare.Should().HaveCount(lastUserPayments.Count);
            firstUserShare.Select(x => x.Payment).ToList().Should().BeEquivalentTo(lastUserPayments);

            lastUserShare.Should().HaveCount(firstUserPayments.Count);
            lastUserShare.Select(x => x.Payment).ToList().Should().BeEquivalentTo(firstUserPayments);


        }

        [Theory]
        [AutoData]
        public void CannotGetUserToUserOutstandingSharesNotInGroup(Group group, User userNotInGroup)
        {
            var userInGroup = group.Members.First().User;
            sut.Invoking(x => x.GetUserToUserOutstandingShares(group, userNotInGroup, userInGroup)).Should().Throw<Exception>().WithMessage("user1 not member of group");
            sut.Invoking(x => x.GetUserToUserOutstandingShares(group, userInGroup, userNotInGroup)).Should().Throw<Exception>().WithMessage("user2 not member of group");
        }

        [Theory]
        [AutoData]
        public void CannotGetOutstandingSharesBetweenUsersInGroupNotInGroup(Group group, User userNotInGroup)
        {
            var userInGroup = group.Members.First().User;
            sut.Invoking(x => x.GetOutstandingSharesBetweenUsersInGroup(group, userNotInGroup, userInGroup)).Should().Throw<Exception>().WithMessage("payee not member of group");
            sut.Invoking(x => x.GetOutstandingSharesBetweenUsersInGroup(group, userInGroup, userNotInGroup)).Should().Throw<Exception>().WithMessage("userToCompareWith not member of group");
        }

    }
}
