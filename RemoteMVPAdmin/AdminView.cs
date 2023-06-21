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
    }
}