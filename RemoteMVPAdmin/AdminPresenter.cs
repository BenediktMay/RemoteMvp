using RemoteMvpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMVPAdmin
{
    public class AdminPresenter
    {
        private readonly AdminView _adminView;
        private readonly IActionAdapter _adapter;

        public AdminPresenter(IActionAdapter adapter)
        {
            _adapter = adapter;
            _adminView = new AdminView();
            _adminView.DeleteRequested += OnDeleteRequested;
        }

        public void OpenUI(bool isModal)
        {
            if (isModal)
            {
                _adminView.ShowDialog();
            }
            else
            {
                _adminView.Show();
            }

        }

        private async void OnDeleteRequested(object? sender, Tuple<string, string> e)
        {
            RemoteActionRequest deleteRequest = new RemoteActionRequest(ActionType.Delete, e.Item1, e.Item2, UserType.Admin);
            await ProcessRequest(deleteRequest);
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

            //switch (response.Type)
            //{
            //    case ResponseType.Error:
            //        _clientView.ShowErrorMessage(response.Message);
            //        break;
            //    case ResponseType.Success:
            //        switch (request.Type)
            //        {
            //            case ActionType.Register:
            //                _clientView.RegisterOk(response.Message);
            //                break;
            //            case ActionType.Login:
            //                _clientView.LoginOk(response.Message);
            //                break;
            //            case ActionType.Delete: //DOTO change from loginok to delteok
            //                _clientView.LoginOk(response.Message);
            //                break;
            //        }
            //        break;
            //}
        }
    }
}
