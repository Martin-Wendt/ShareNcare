
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public User(string name)
        {
            Name = name;
        }
    }
}
