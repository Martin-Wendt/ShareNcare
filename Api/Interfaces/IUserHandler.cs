using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IUserHandler
    {
        User CreateUser(string name);

    }

    public class UserHandler : IUserHandler
    {
        public User CreateUser(string name)
        {
            return new User(name);
        }
    }
}
