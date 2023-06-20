using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteMvpApp
{
    public enum UserListActionResult
    {
        UserNotExisting,
        UserAlreadyExists,
        UserOkPasswordWrong,
        AccessGranted,
        RegistrationOk,
        AccessDenied,
        UserDelted
    }

    internal class Userlist
    {
        private record User(string UserName, string Password);
        private readonly List<User> _users;

        public Userlist()
        {
            _users = new List<User>();
        }

        public UserListActionResult LoginUser(string username, string password)
        {
            foreach (var user in _users.Where(user => user.UserName.Equals(username)))
            {
                if (user.Password.Equals(password))
                {
                    return UserListActionResult.AccessGranted;
                }
                else
                {
                    return UserListActionResult.UserOkPasswordWrong;
                }
            }

            return UserListActionResult.UserNotExisting;
        }

        public UserListActionResult RegisterUser(string username, string password)
        {
            if (_users.Any(user => user.UserName.Equals(username)))
            {
                return UserListActionResult.UserAlreadyExists;
            }

            User newUser = new(username, password);
            _users.Add(newUser);

            SaveUserToCSV(_users);

            return UserListActionResult.RegistrationOk;
        }

        private void SaveUserToCSV(List<User> users)
        {
            using (var file = File.CreateText(Path.Combine(Environment.CurrentDirectory, "users.csv")))
            {
                foreach (var user in users)
                {
                    file.WriteLine(user.UserName+";"+ user.Password);
                }
            }
        }

        public void RemoveUser(string username)
        {
            _users.RemoveAll(user => user.UserName.Equals(username));
        }

        public void RemoveAllUsers()
        {
            _users.Clear();
        }
    }
}
