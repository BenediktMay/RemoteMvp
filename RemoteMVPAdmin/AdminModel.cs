using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    internal class AdminModel
    {
        public List<Tuple<string, string>> _users;

        public AdminModel()
        {
            _users = new List<Tuple<string, string>>();
        }

        public void AddToList(string user)
        {
            var parts = user.Split(';');
            _users.Add(Tuple.Create(parts[0], parts[1]));
        }

        public void ClearList()
        {
            _users.Clear();
        }



    }
}
