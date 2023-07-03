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

            // if exe is started seperatly
            ApplicationConfiguration.Initialize();
            var admin = new RemoteActionAdapter("localhost", 11000);
            var adminPresenter = new AdminPresenter(admin);
            adminPresenter.OpenUI(true);
        }
    }
}