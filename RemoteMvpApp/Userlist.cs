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
        private record User(string UserName, string Password);
        private readonly List<User> _users;

        private string CSVFilename="users.csv";

        public event EventHandler ModelChanged;


        public Userlist()
        {
            _users = new List<User>();

            ModelChanged += OnModelChanged;

            _users=LoadUsersFromCSV();

            //Test users no validation
            //for (int i = 1; i < 20; i++)
            //{
            //    _users.Add(new User("Test " + i, "PW " + i));
            //}



        }

        private void OnModelChanged(object? sender, EventArgs e)
        {
            SaveUserToCSV(_users);
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

            ModelChanged?.Invoke(this, EventArgs.Empty);

            return UserListActionResult.RegistrationOk;
        }

        private void SaveUserToCSV(List<User> users)
        {

            try
            {
                using (var file = File.CreateText(Path.Combine(Environment.CurrentDirectory, CSVFilename)))
            {
                foreach (var user in users)
                {
                    file.WriteLine(user.UserName+";"+ user.Password);
                }
            }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error while write the file: " + e.Message);
            }
}

        private List<User> LoadUsersFromCSV()
        {

            List<User> CSVusers= new List<User>();

            try
            {
                using (StreamReader sr = new StreamReader((Path.Combine(Environment.CurrentDirectory, CSVFilename))))
                {
                    string line;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        var splitString = line.Split(';');
                        CSVusers.Add(new User(splitString[0], splitString[1]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading the file or file has not yet been created: " + e.Message);
            }

            return CSVusers;
        }

        public List<string>UserToStringList()
        {
            List<string> stringUserList = new List<string>();

            foreach (var user in _users)
            {
                stringUserList.Add(user.UserName + ";" + user.Password);
            }

            return stringUserList;
        }

        public void RemoveUser(string username)
        {
            _users.RemoveAll(user => user.UserName.Equals(username));
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveAllUsers()
        {
            _users.Clear();
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
