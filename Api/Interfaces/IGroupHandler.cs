using Api.Entities;
using Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IGroupHandler
    {
        Group ResolveGroup(Group group);
        Group CloseGroup(Group group);
        Group CreateGroup(string name, string description, List<User> members);
    }

    public class GroupHandler : IGroupHandler
    {
        public Group ResolveGroup(Group group)
        {
            group.State = State.Resolving;
            return group;
        }

        public Group CloseGroup(Group group)
        {
            switch (group.State)
            {       
                case State.Active:
                    throw new Exception("Cannot close group, please resolve group first.");
                case State.Resolving:
                    if (group.GetOutstandingPaymentShares().Count == 0)
                    {
                        group.State = State.Closed;
                        return group;
                    }
                    else
                    {
                        throw new Exception("Cannot close group, some payment are still unresolved");
                    }

                case State.Closed:
                    return group;
                default:
                    throw new Exception("Something went wrong... Please restart your device ;");
            }

        }  

        public Group CreateGroup(string name, string description, List<User> members)
        {
            return new Group(name, description, members);
        }
    }
}
