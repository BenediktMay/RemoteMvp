using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteMvpLib
{
    public class RemoteActionAdapter : IActionAdapter
    {

        private string _host = "localhost";
        private int _port;

        public RemoteActionAdapter(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task<RemoteActionResponse> PerformActionAsync(RemoteActionRequest request)
        {
            var buffer = new byte[1024];
            RemoteActionResponse response;

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                string message = Serialize(request);
                Console.WriteLine("Performing remote action: " + message);
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                var host = Dns.GetHostEntry(_host);
                var ipAddress = host.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, _port);

                // Create a TCP/IP  socket.
                var sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to Remote EndPoint
                sender.Connect(remoteEP);

                Console.WriteLine("Client connected to {0}", sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                var msg = Encoding.ASCII.GetBytes(message);

                // Send the data through the socket asynchronously.
                var bytesSent = await sender.SendAsync(msg, SocketFlags.None);
                Console.WriteLine($"{bytesSent} bytes sent to server. Waiting for response ...");

                // Receive the response from the remote device asynchronously
                var bytesRec = await sender.ReceiveAsync(buffer, SocketFlags.None);
                var responseString = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                Console.WriteLine($"Received {bytesRec} bytes: {responseString}");

                response = Deserialize(responseString);

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (ArgumentNullException aex)
            {
                Console.WriteLine("ArgumentNullException : {0}", aex.ToString());
                throw;
            }
            catch (SocketException sex)
            {
                Console.WriteLine("SocketException : {0}", sex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
                throw;
            }

            return response;
        }


        // ############# Protocol layer #############

        private static string Serialize(RemoteActionRequest request)
        {
            return string.Format("{0};{1};{2}", request.Type.ToString(), request.UserName, request.Password);
        }

        private static RemoteActionResponse Deserialize(string response)
        {
            string[] parts = response.Split(';');
            RemoteActionResponse res = new RemoteActionResponse(Enum.Parse<ResponseType>(parts[0]), parts[1]);
            return res;
        }
    }
}
