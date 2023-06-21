using RemoteMvpLib;

namespace RemoteMvpApp
{
    internal class ApplicationController
    {
        // Model 
        private readonly Userlist _users;

        // ActionEndpoint (to be called by the view)
        private readonly IActionEndpoint _actionEndpoint;

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
                    else throw new NotImplementedException();//TODO SEND ERROR MESSAGE
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

        private void Process_Delete(RemoteActionEndpoint handler, string username, string password)
        {
            _users.RemoveUser(username);
            handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, "User was deleted (probably fails silently ;) )"));
        }

        private void SentUserList(RemoteActionEndpoint handler)
        {
            List<string> stringUserList = new List<string>();
            stringUserList=_users.UserToStringList();
            string responseString;

            foreach (var user in stringUserList)
            {
                handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, user));
            }


            //    stringUserList.Add(user.UserName + ";" + user.Password);
            //}

            //string responseString = Serialize(response);
            //byte[] msg = Encoding.ASCII.GetBytes(responseString);
            //handler.Send(msg);
            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();


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
