using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    public class AdminModel
    {
        private record User(string UserName, string Password);
        private List<User> _users { get; }

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
