using Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Group 
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public State State { get;internal set; } = State.Active;
        public List<GroupMember> Members { get; } = new();
               
        public Group(string name, string description, List<User> members) 
        {
            Name = name;
            Description = description;
            foreach (var user in members)
            {
                var member = new GroupMember(user);
                Members.Add(member);
            }         
        }
    }
    public enum State
    {
        Active,
        Resolving,
        Closed,
    }

}
