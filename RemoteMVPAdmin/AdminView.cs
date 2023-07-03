namespace RemoteMVPAdmin
{
    public partial class AdminView : Form
    {
        #region Declaration

        public event EventHandler<int> DeleteRequested;

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public AdminView()
        {
            InitializeComponent();
        }

        #region Events

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteRequested?.Invoke(this, listViewUser.SelectedIndices[0]);
            }
            catch (Exception)
            {
                DeletedNotOK();
            }  
        }

        #endregion

        #region Methods

        /// <summary>
        /// update listview
        /// </summary>
        /// <param name="users"></param>
        public void UpdateView(List<User> users)
        {
            listViewUser.Items.Clear();

            foreach (User user in users)
            {
                ListViewItem item = new ListViewItem(new string[] { user.Name, user.Password });

                listViewUser.Items.Add(item);
            }

            listViewUser.Update();
        }

        /// <summary>
        /// MessageBox to print error messages
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// MessageBox to inform delete success
        /// </summary>
        /// <param name="text"></param>
        public void DeletedOK(string text)
        {
            MessageBox.Show(text, "User Deleted", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /// <summary>
        /// MessageBox to inform failed delete
        /// </summary>
        public void DeletedNotOK()
        {
            MessageBox.Show("Please Select the user you want to Delete!", "No User Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

    }
}