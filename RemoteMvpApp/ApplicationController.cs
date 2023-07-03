using RemoteMvpLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RemoteMvpApp
{
    internal class ApplicationController
    {
        // Model 
        private readonly Userlist _users;

        // ActionEndpoint (to be called by the view)
        private readonly IActionEndpoint _actionEndpoint;

        // sendsequences
        private int _sendIndex = 1;
        private int _sendBlocksCount = 0;

        public ApplicationController(IActionEndpoint actionEndpoint)
        {
            // Create new Model
            _users = new Userlist();

            // Link ActionEndpoint to local method
            _actionEndpoint = actionEndpoint;
            _actionEndpoint.OnActionPerformed += EndpointOnActionPerformed;
        }


        public void RunActionEndPoint() => _actionEndpoint.RunActionEndpoint();


        public Task RunActionEndPointAsync()
        {
            var task = new Task(_actionEndpoint.RunActionEndpoint);
            task.Start();
            return task;
        }

        private void EndpointOnActionPerformed(object? sender, RemoteActionRequest request)
        {
            if (sender is not RemoteActionEndpoint) return;

            var handler = (RemoteActionEndpoint)sender;
            switch (request.Type)
            {
                case ActionType.Login:
                    Process_Login(handler, request.UserName, request.Password);
                    break;
                case ActionType.Register:
                    Process_Register(handler, request.UserName, request.Password);
                    break;
                case ActionType.Delete:
                    if (request.UserType == UserType.Admin)
                    {
                        Process_Delete(handler, request.UserName, request.Password);
                    }
                    else handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, $"Delete user failed"));
                    break;
                case ActionType.RequestList:
                    SentUserList(handler);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Request not supported");
            }
        }

        private void Process_Login(RemoteActionEndpoint handler, string username, string password)
        {
            switch (_users.LoginUser(username, password))
            {
                case UserListActionResult.AccessGranted:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, $"Access granted for {username}."));
                    break;
                case UserListActionResult.UserOkPasswordWrong:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Wrong password."));
                    break;
                case UserListActionResult.UserNotExisting:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, $"User {username} not existing."));
                    break;
                default:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Unsupported action."));
                    break;
            }
        }

        private void Process_Register(RemoteActionEndpoint handler, string username, string password)
        {
            switch (_users.RegisterUser(username, password))
            {
                case UserListActionResult.UserAlreadyExists:
                    Console.WriteLine("Error registering: User already existing.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, $"Error! User {username} is already existing."));
                    break;
                case UserListActionResult.RegistrationOk:
                    Console.WriteLine("User registration OK.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, $"Registration successful for {username}. You can now login."));
                    break;
                default:
                    Console.WriteLine("Unknown action.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Unsupported operation."));
                    break;
            }
        }

        /// <summary>
        /// call the model method to delete the user
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void Process_Delete(RemoteActionEndpoint handler, string username, string password)
        {
            _users.RemoveUser(username);
            handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, "User was deleted (probably fails silently ;) )"));
        }
        /// <summary>
        /// Method that reorders users string and call the PerformActionResponse method
        /// </summary>
        /// <param name="handler"></param>
        private void SentUserList(RemoteActionEndpoint handler)
        {
            List<string> stringUserList = new List<string>();
            stringUserList = _users.UserToStringList();
            string responseString = string.Empty;

            if (_sendBlocksCount == 0)
            {
                _sendBlocksCount = stringUserList.Count / 10;
                if (_sendBlocksCount % 1 != 0) _sendBlocksCount += 1;
            }

            //Add bit for not finished transmission
            if (_sendBlocksCount > _sendIndex)
            {
                for (int i = _sendIndex * 10; i < _sendIndex * 10 + 10; i++)
                {
                    responseString += "0\n";
                    responseString += stringUserList[i];
                    if (stringUserList.Count - 1 > i) responseString += "\n";
                }
                _sendIndex++;
            }
            //add bit for finised transmission
            else
            {
                for (int i = _sendIndex * 10; i < stringUserList.Count; i++)
                {
                    responseString += "1\n";
                    responseString += stringUserList[i];
                    if (stringUserList.Count - 1 > i) responseString += "\n";
                }
                _sendIndex = 1;
                _sendBlocksCount = 0;
            }
            handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, responseString));
        }


        /// <summary>
        /// Helper method to parse semicolon-separated key=value pairs
        /// </summary>
        /// <param name="cmd">A string semicolon-separated key=value pairs</param>
        /// <returns>A dictionary with key value pairs</returns>
        private Dictionary<string, string> ProcessCmd(string cmd)
        {
            cmd = cmd.TrimEnd(';');

            string[] parts = cmd.Split(new char[] { ';' });

            Dictionary<string, string> keyValuePairs = cmd.Split(';')
                .Select(value => value.Split('='))
                .ToDictionary(pair => pair[0], pair => pair[1]);

            return keyValuePairs;
        }
    }
}
