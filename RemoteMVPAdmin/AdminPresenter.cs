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

        private AdminView _adminView;
        private AdminModel _adminModel;
        private readonly IActionAdapter _adapter;

        private bool _allUsersRecieved = false;

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

            _adminView.DeleteRequested += OnDeleteRequested;
            _adminModel.ModelChanged += OnModelChanged;
             
            // get current data
            UpdateModel();
        }

      
        #region Events
     
        /// <summary>
        /// Update view when model changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelChanged(object? sender, EventArgs e)
        {
            _adminView.UpdateView(_adminModel._users);
        }

        /// <summary>
        /// request to delete user from access to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="indices"></param>
        private async void OnDeleteRequested(object? sender, int indices)
        {
            var user = _adminModel._users[indices];

            RemoteActionRequest deleteRequest = new RemoteActionRequest(ActionType.Delete, user.Name, user.Password, UserType.Admin);
            await ProcessRequest(deleteRequest);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get new users in packages and update model
        /// </summary>
        private async void UpdateModel()
        {
            _adminModel._users.Clear();
            do
            {
                RemoteActionRequest getList = new RemoteActionRequest(ActionType.RequestList, "", "", UserType.Admin);
                await ProcessRequest(getList);

            } while (!_allUsersRecieved);
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
                            UpdateModel();
                            break;
                        
                        case ActionType.RequestList:
                            try // validate errors in recieving and adding users
                            {
                                CollectUsers(response.Message);
                            }
                            catch (Exception e)
                            {
                                _adminView.ShowErrorMessage(e.Message);
                            }
                            break;
                    }
                    break;
            }

        }

        /// <summary>
        /// Adds a package of users to the Model
        /// Also identifies if there are more to come
        /// return false: not all users transmitted
        /// return true: all users tranmitted
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool CollectUsers(string response)
        {
            // split message
            var lines = response.Split('\n');

            // i=1 to jump over msb
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(';');
                User user = new User(parts[0], parts[1]);
                if (!_adminModel._users.Contains(user))
                {
                    _adminModel.AddToList(parts[0], parts[1]);
                }
            }

            // detection if more packages are following 
            if (lines[0] == "0")
            {
                _allUsersRecieved = false;
                return false;
            }
            else if (lines[0] == "1")
            {
                _allUsersRecieved = true;
                return true;
            }
            else throw new InvalidOperationException("MSB missing"); 

        }

       
        #endregion

    }
}
