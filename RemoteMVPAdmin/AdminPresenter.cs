using RemoteMvpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    internal class AdminPresenter
    {

        private async void OnLoginRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest loginRequest = new RemoteActionRequest(ActionType.Login, e.Item1, e.Item2, UserType.User);
            await ProcessRequest(loginRequest);
        }

        private async void OnRegisterRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest loginRequest = new RemoteActionRequest(ActionType.Register, e.Item1, e.Item2, UserType.User);
            await ProcessRequest(loginRequest);
        }

        private async void OnDeleteRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest deleteRequest = new RemoteActionRequest(ActionType.Delete, e.Item1, e.Item2, U);
            await ProcessRequest(deleteRequest);
        }
    }
}
