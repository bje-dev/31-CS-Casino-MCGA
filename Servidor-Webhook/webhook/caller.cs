using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace webhook
{

    public static class caller
    {

        public static string ExecuteClient(string msg)
        {
            string msgToSend;
            string result = "";
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0]; //Localhost
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1111); //Puerto

                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    sender.Connect(localEndPoint);

                    msgToSend = String.Concat(msg, " <EOT> ");
                    byte[] messageSent = Encoding.ASCII.GetBytes(msgToSend);
                    int byteSent = sender.Send(messageSent);

                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
                    result = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    Console.WriteLine("Message from Server -> {0}", result);

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
            return result;
        }
    }
}
