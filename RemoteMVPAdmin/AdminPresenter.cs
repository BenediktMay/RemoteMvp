using Microsoft.VisualBasic.ApplicationServices;
using RemoteMvpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RemoteMVPAdmin
{
    public class AdminPresenter
    {
        private readonly AdminView _adminView;
        private readonly AdminModel _adminModel;
        private readonly IActionAdapter _adapter;

        public AdminPresenter(IActionAdapter adapter)
        {
            _adapter = adapter;
            _adminView = new AdminView();
            _adminModel = new AdminModel();
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

        private async void OnDeleteRequested(object? sender, int indices)
        {
            var user = _adminModel._users[indices];

            RemoteActionRequest deleteRequest = new RemoteActionRequest(ActionType.Delete, user.Item1, user.Item2, UserType.Admin);
            await ProcessRequest(deleteRequest);

            
        }

        private async void UpdateModel()
        {
            RemoteActionRequest getList = new RemoteActionRequest(ActionType.RequestList,"","",UserType.Admin);
            await ProcessRequest(getList);
        
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
                    _adminView.ShowErrorMessage(response.Message);
                    break;
                case ResponseType.Success:
                    switch (request.Type)
                    {
                        case ActionType.Delete:

                            _adminView.DeletedOK(response.Message);

                            break;

                        case ActionType.RequestList:

                            var parts = response.Message.Split(';');
                            User user = new User(parts[0], parts[1]);   

                            if (_adminModel._users.Contains(user))
                            {

                            }

                            _adminModel.AddToList(parts[0], parts[1]);
                            //response String (PreName;LastName)

                            break;
                    }



                    break;
            }

        }

    }
}
