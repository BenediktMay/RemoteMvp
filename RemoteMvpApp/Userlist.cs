using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;

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
        #region Declaration

        private record User(string UserName, string Password);
        private readonly List<User> _users;

        private string CSVFilename = "users.csv";

        public event EventHandler ModelChanged;

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public Userlist()
        {
            _users = new List<User>();

            ModelChanged += OnModelChanged;

            // Admin user should always be in userlist
            _users.Add(new User("Admin", "*********"));

            LoadUsersFromCSV();

        }

        #region Events

        private void OnModelChanged(object? sender, EventArgs e)
        {
            SaveUserToCSV(_users);
        }

        #endregion


        #region Methods

        /// <summary>
        /// login user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// register user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserListActionResult RegisterUser(string username, string password)
        {
            if (_users.Any(user => user.UserName.Equals(username)))
            {
                return UserListActionResult.UserAlreadyExists;
            }

            User newUser = new(username, password);
            _users.Add(newUser);

            ModelChanged?.Invoke(this, EventArgs.Empty);

            return UserListActionResult.RegistrationOk;
        }

        /// <summary>
        /// save users as csv
        /// </summary>
        /// <param name="users"></param>
        private void SaveUserToCSV(List<User> users)
        {

            try
            {
                using (var file = File.CreateText(Path.Combine(Environment.CurrentDirectory, CSVFilename)))
                {
                    foreach (var user in users)
                    {
                        file.WriteLine(user.UserName + ";" + user.Password);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error while write the file: " + e.Message);
            }
        }

        /// <summary>
        /// load users from csv 
        /// </summary>
        private void LoadUsersFromCSV()
        {
            List<User> CSVusers = new List<User>();

            try
            {
                using (StreamReader sr = new StreamReader((Path.Combine(Environment.CurrentDirectory, CSVFilename))))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        var splitString = line.Split(';');
                        var newUser = new User(splitString[0], splitString[1]);

                        //To ensure no user are duplicated
                        if (!_users.Contains(newUser)) CSVusers.Add(newUser);
                    }
                }

                foreach (var user in CSVusers)
                {
                    _users.Add(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading the file or file has not yet been created: " + e.Message);
            }

        }

        /// <summary>
        /// convert userlist to stringlist
        /// delimiter = ;
        /// </summary>
        /// <returns></returns>
        public List<string> UserToStringList()
        {
            List<string> stringUserList = new List<string>();

            foreach (var user in _users)
            {
                stringUserList.Add(user.UserName + ";" + user.Password);
            }

            return stringUserList;
        }

        /// <summary>
        /// remove user defined by username
        /// </summary>
        /// <param name="username"></param>
        public void RemoveUser(string username)
        {
            _users.RemoveAll(user => user.UserName.Equals(username));
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// remove all users
        /// </summary>
        public void RemoveAllUsers()
        {
            _users.Clear();
            _users.Add(new User("Admin", "*********"));
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
