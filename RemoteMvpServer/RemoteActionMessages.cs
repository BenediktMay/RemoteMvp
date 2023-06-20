

namespace RemoteMvpLib
{
    public enum ActionType
    {
        Register,
        Login,
        Logout,
        Delete,
        RequestList
    }

    public enum UserType
    {
        User,
        Admin
    }

    public class RemoteActionRequest
    {
        public ActionType Type { get; }

        public string UserName { get; }

        public string Password { get; }

        public UserType UserType { get; }

        public RemoteActionRequest(ActionType type, string username, string password, UserType userType)
        {
            Type = type;
            UserName = username;
            Password = password;
            UserType = userType;
        }
    }

    public enum ResponseType
    {
        Success,
        Error
    }

    public class RemoteActionResponse
    {
        public ResponseType Type { get; set; }

        public string? Message { get; set; }

        public RemoteActionResponse(ResponseType type, string? message)
        {
            Type = type;
            Message = message;
        }
    }
}
