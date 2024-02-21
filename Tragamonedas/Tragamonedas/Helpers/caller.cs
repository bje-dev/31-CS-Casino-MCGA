using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tragamonedas
{
    public static class caller
    {

        public static string ExecuteClient(string msg)
        {
            string msgToSend;
            string result = "";
            try
            {

                //Variables de configuración del endpoint remoto que utilizaremos en el socket.
                //Este ejemplo usa el puerto 1111 en la computadora local.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddr = IPAddress.Parse("172.16.1.200"); //Servidor de Casino se ejecuta en host remoto
                IPAddress ipAddr = ipHost.AddressList[0]; //Servidor de Casino Se ejecuta en localhost
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1111); //Puerto

                // Creación y configuración del Socket TCP/IP
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Conexión del socket al endpoint remoto utilizando
                    // el método Connect()
                    sender.Connect(localEndPoint);

                    // Mostramos por pantalla la información del endpoint
                    // donde estamos conectado.
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creación del mensaje que enviaremos al server
                    msgToSend = String.Concat(msg," <EOT> ");
                    byte[] messageSent = Encoding.ASCII.GetBytes(msgToSend);
                    int byteSent = sender.Send(messageSent);

                    // Buffer de datos
                    byte[] messageReceived = new byte[1024];

                    // Recibimos la respuesta del server a través del método Receive(). 
                    // Este método retorna el número de bytes recibidos que lo usaremos
                    // para realizar la conversión del mensaje a string.
                    int byteRecv = sender.Receive(messageReceived);
                    result = Encoding.ASCII.GetString(messageReceived,0, byteRecv);
                    Console.WriteLine("Message from Server -> {0}", result);

                    // Cerramos el socket con el método Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }

                // Manejamos las excepciones de socket posibles
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
