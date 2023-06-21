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

        public void UpdateView(List<string> users)
        {

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