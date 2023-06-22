namespace RemoteMVPAdmin
{
    public partial class AdminView : Form
    {

        public event EventHandler<int> DeleteRequested;
        public AdminView()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRequested?.Invoke(this, listViewUser.SelectedIndices[0]);
        }

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

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void DeletedOK(string text)
        {
            MessageBox.Show(text, "User Deleted", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}