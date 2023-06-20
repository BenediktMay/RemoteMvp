using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RemoteMvpLib;

namespace RemoteMvpClient
{
    public class ClientPresenter
    {
        private readonly ClientView _clientView;
        private readonly IActionAdapter _adapter;

        public ClientPresenter(IActionAdapter adapter)
        {
            _adapter = adapter;
            _clientView = new ClientView();
            _clientView.LoginRequested += OnLoginRequested;
            _clientView.RegisterRequested += OnRegisterRequested;
        }

        public void OpenUI(bool isModal)
        {
            if (isModal)
            {
                _clientView.ShowDialog();
            }
            else
            {
                _clientView.Show();
            }
            
        }

        private async void OnLoginRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest loginRequest = new RemoteActionRequest(ActionType.Login, e.Item1, e.Item2,UserType.User);
            await ProcessRequest(loginRequest);
        }

        private async void OnRegisterRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest loginRequest = new RemoteActionRequest(ActionType.Register, e.Item1, e.Item2,UserType.User);
            await ProcessRequest(loginRequest);
        }

  
        /// <summary>
        /// Collect and process all UI events
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="request">Property-based request</param>
        private async Task ProcessRequest(RemoteActionRequest request)
        {
            // Execute action in actionlistener and wait for result asynchronously
            RemoteActionResponse response = await _adapter.PerformActionAsync(request);

            // Process result

            switch (response.Type)
            {
                case ResponseType.Error:
                    _clientView.ShowErrorMessage(response.Message);
                    break;
                case ResponseType.Success:
                    switch (request.Type)
                    {
                        case ActionType.Register:
                            _clientView.RegisterOk(response.Message);
                            break;
                        case ActionType.Login:
                            _clientView.LoginOk(response.Message);
                            break;
                        case ActionType.Delete: //DOTO change from loginok to delteok
                            _clientView.LoginOk(response.Message);
                            break;
                    }
                    break;
            }
        }
    }
}
