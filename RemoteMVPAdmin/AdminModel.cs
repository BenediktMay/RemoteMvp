using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    public class User
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }


    public class AdminModel
    {
        public List<User> _users;

        public AdminModel()
        {
            _users = new List<User>();
        }

        public void AddToList(string user, string password)
        {
            _users.Add(new User(user, password));
        }

        public void ClearList()
        {
            _users.Clear();
        }
    


    }
}
