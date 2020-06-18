using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocketClient
{

    // This template use base socket syntax to change Pattern. (like Send, Receive, and so on)
    // Convert to Task-based Asynchronous Pattern. (TAP)
    public static class AsynchronousClientSocket
    {

        public static async Task<string> SendMessage(string ps_IPAddress, int pi_Port, string ps_Message)
        {
            string ls_response = "";

            try
            {
                string ls_ReturnMessage = "";

                // Establish the remote endpoint for the socket.  
                IPAddress ipAddress = IPAddress.Parse(ps_IPAddress);
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, pi_Port);

                // Create a TCP/IP socket.  
                var client = new Socket(ipAddress.AddressFamily,
                                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                var isConnect = await client.ConnectAsync(remoteEndPoint).ConfigureAwait(false);
                if (!isConnect)
                {
                    Console.WriteLine("Can not connect.");
                    return ls_ReturnMessage;
                }

                // Send test data to the remote device. 

                var bytesSent = await client.SendAsync(ps_Message + "<EOF>").ConfigureAwait(false);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Receive the response from the remote device.  
                ls_response = await client.ReceiveAsync().ConfigureAwait(false);

                // Write the response to the console.  
                Console.WriteLine("Response received : {0}", ls_response);

                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            return ls_response;
        }

        private static Task<bool> ConnectAsync(this Socket client, IPEndPoint remoteEndPoint)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (remoteEndPoint == null) throw new ArgumentNullException(nameof(remoteEndPoint));
            return Task.Run(() => Connect(client, remoteEndPoint));
        }

        private static bool Connect(this Socket client, EndPoint remoteEndPoint)
        {
            if (client == null || remoteEndPoint == null)
                return false;

            try
            {
                client.Connect(remoteEndPoint);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<string> ReceiveAsync(this Socket client, int waitForFirstDelaySeconds = 3)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Timeout for wait to receive and prepare data.
            for (var i = 0; i < waitForFirstDelaySeconds; i++)
            {
                if (client.Available > 0)
                    break;
                await Task.Delay(1000).ConfigureAwait(false);
            }
            // return null If data is not available.
            if (client.Available < 1)
                return null;

            // Size of receive buffer.
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];

            // Get data
            var response = new StringBuilder(bufferSize);
            do
            {
                var size = Math.Min(bufferSize, client.Available);
                await Task.Run(() => client.Receive(buffer)).ConfigureAwait(false);
                response.Append(Encoding.ASCII.GetString(buffer, 0, size));
            } while (client.Available > 0);

            // Return result.
            return response.ToString();
        }

        private static async Task<int> SendAsync(this Socket client, string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);
            return await SendAsync(client, byteData, 0, byteData.Length, 0).ConfigureAwait(false);
        }

        private static Task<int> SendAsync(this Socket client, byte[] buffer, int offset,
            int size, SocketFlags socketFlags)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return Task.Run(() => client.Send(buffer, offset, size, socketFlags));
        }
    }
}