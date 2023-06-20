using RemoteMvpLib;

namespace RemoteMVPAdmin
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var admin = new RemoteActionAdapter("localhost", 11000);
            var adminController = new AdminPresenter(admin);
            adminController.OpenUI(true);
        }
    }
}