using System.Net.Mime;
using System.Windows.Forms;
using RemoteMvpApp;
using RemoteMvpClient;
using RemoteMvpLib;
using RemoteMVPAdmin;


// Bootstrapper sequence

// Server-side classes
var endpoint = new RemoteActionEndpoint("localhost", 11000);
var server = new ApplicationController(endpoint);
//server.RunActionEndPoint();                       // Synchronous (blocking) server start
Task serverTask = server.RunActionEndPointAsync();  // Asynchronous (non-blocking) server start

//// Client-side classes
var client = new RemoteActionAdapter("localhost", 11000);
var clientController = new ClientPresenter(client);
clientController.OpenUI(true);

// Client-side classes
//var admin = new RemoteActionAdapter("localhost", 11000);
//var adminController = new AdminPresenter(admin);
//adminController.OpenUI(true);


serverTask.Wait();

