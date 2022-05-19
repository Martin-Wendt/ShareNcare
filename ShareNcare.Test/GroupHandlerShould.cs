using Api.Entities;
using Api.Interfaces;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareNcare.Test
{
    public class GroupHandlerShould
    {
        [Theory]
        [AutoData]
        public void CreateGroup(string name, string description, List<User> users)
        {

            var sut = new GroupHandler();
            var grp = sut.CreateGroup(name, description, users);

            Assert.NotNull(grp);
            grp.Should().GetType().As<Group>();
            grp.Members.Should().HaveCount(users.Count);

            grp.Name.Should().Be(name);
            grp.Description.Should().Be(description);
        }
        

        [Theory]
        [AutoData]
        public void ResolveGroup(Group group)
        {
            var sut = new GroupHandler();

            sut.ResolveGroup(group);

            group.State.Should().Be(State.Resolving);
        }


        [Theory]
        [AutoData]
        public void CloseGroup(Group group)
        {
            var sut = new GroupHandler();
            sut.ResolveGroup(group);

            sut.CloseGroup(group);

            group.State.Should().Be(State.Closed);
        }


        [Theory]
        [AutoData]
        public void CannotCloseGroupThatAreActive(Group group)
        {
            var sut = new GroupHandler();

            sut.Invoking(x => x.CloseGroup(group)).Should().Throw<Exception>().WithMessage("Cannot close group, please resolve group first.");

        }


        [Theory]
        [AutoData]
        public void CannotCloseGroupWithPaymentsPending(Group group, Payment payment)
        {
            var sut = new GroupHandler();
            var paymenthandler = new PaymentHandler();

            paymenthandler.AddPayment(payment, group.Members.First().User,group);

            sut.ResolveGroup(group);

            sut.Invoking(x => x.CloseGroup(group)).Should().Throw<Exception>().WithMessage("Cannot close group, some payment are still unresolved");

            
        }
    }
}
