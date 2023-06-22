using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    /// <summary>
    /// User obejct consists name and password
    /// </summary>
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
        #region Declaration

        public event EventHandler ModelChanged;

        public List<User> _users;

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public AdminModel()
        {
            _users = new List<User>();
        }


        #region Methods

        /// <summary>
        /// Add a new user to userlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public void AddToList(string user, string password)
        {
            _users.Add(new User(user, password));
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new user to userlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public void AddToList(User user)
        {
            _users.Add(user);
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// clear intire userlist
        /// </summary>
        public void ClearList()
        {
            _users.Clear();
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


    }
}
