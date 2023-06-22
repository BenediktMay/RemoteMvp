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
        #region Declaration

        private event EventHandler UserDeleted;

        private AdminView _adminView;
        private AdminModel _adminModel;
        private readonly IActionAdapter _adapter;

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="adapter"></param>
        public AdminPresenter(IActionAdapter adapter)
        {
            _adapter = adapter;
            _adminView = new AdminView();
            _adminModel = new AdminModel();
            UpdateModel();
            _adminView.DeleteRequested += OnDeleteRequested;
            _adminModel.ModelChanged += OnModelChanged;
            this.UserDeleted += OnUserDeleted;
        }

        #region Events

        private void OnUserDeleted(object? sender, EventArgs e)
        {
            UpdateModel();
        }

        private void OnModelChanged(object? sender, EventArgs e)
        {
            _adminView.UpdateView(_adminModel._users);
        }

        private async void OnDeleteRequested(object? sender, int indices)
        {
            var user = _adminModel._users[indices];

            RemoteActionRequest deleteRequest = new RemoteActionRequest(ActionType.Delete, user.Name, user.Password, UserType.Admin);
            await ProcessRequest(deleteRequest);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get new userlist and update Model
        /// </summary>
        private async void UpdateModel()
        {

            RemoteActionRequest getList = new RemoteActionRequest(ActionType.RequestList, "", "", UserType.Admin);
            await ProcessRequest(getList);

        }

        /// <summary>
        /// Open UI
        /// </summary>
        /// <param name="isModal"></param>
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
                            UserDeleted?.Invoke(this, EventArgs.Empty);

                            break;

                        case ActionType.RequestList:

                            _adminModel._users.Clear();

                            var lines = response.Message.Split('\n');

                            foreach (var line in lines)
                            {
                                var parts = line.Split(';');
                                User user = new User(parts[0], parts[1]);
                                if (!_adminModel._users.Contains(user))
                                {
                                    _adminModel.AddToList(parts[0], parts[1]);
                                }
                            }

                            break;
                    }



                    break;
            }

        }

        #endregion

    }
}
