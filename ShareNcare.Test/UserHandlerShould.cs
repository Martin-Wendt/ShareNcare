using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Interfaces;

namespace ShareNcare.Test
{
    public class UserHandlerShould
    {
        [Fact]
        public void CreateUser()
        {
            string name = "randomName";
            var sut = new UserHandler();

            var user = sut.CreateUser(name);

            Assert.NotNull(user);
            Assert.True(user.Name == name);
        }

    }
}
