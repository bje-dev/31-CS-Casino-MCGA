using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using ConsoleApp1;
using ConsoleApp1.Dominio;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server
{
    class Program
    {
        //La variable path tiene la direccion del backoffice
        private static Casino cas;

        // Método principal
        static void Main(string[] args)
        {
            loadURLs();
            InitializeBackoffice();
            ExecuteServer();
        }
        private static void loadURLs()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            //config["ConnectionString:backofficeLocal"];
            cas = new Casino(Int32.Parse(config["Casino:id"]), 
                config["ConnectionString:backofficeAzure"],
                config["ConnectionString:bitacora"]);
        }

        private static void InitializeBackoffice()
        {
            //Le mandamos al backoffice cual es el webhook y Id Casino correspondiente
            //Verificar launchSettings.json
            //serviceCaller.callService(path, "POST", String.Concat("{idCasino=",";HttpWebhookCasino=44329}"));
        }

        public static void ExecuteServer()
        {

            string _response = "";
            BackofficeMaxMinResponse backofficeMaxMin = new BackofficeMaxMinResponse();

            //Variables de configuración del endpoint local que utilizaremos en el socket.
            // Dns.GetHostName retorna el nombre del host que está ejecutando la aplicación server
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1111);

            // Creación y configuración del Socket TCP/IP
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Usando el método Bind() asociamos una dirección de red al Socket del servidor.
                // Todos los clientes que se conectarán a este servidor deben conocer esta dirección de red
                listener.Bind(localEndPoint);

                // Usando el método Listen() creamos la lista de clientes
                // que querrán conectarse al servidor
                listener.Listen(10);

                while (true)
                {

                    Console.WriteLine("Waiting connection ... ");

                    // A la espera de una conexión entrante.
                    // Usando el método Accept(), el servidor aceptará la conexión del cliente.
                    Socket clientSocket = listener.Accept();

                    // Buffer de datos
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);

                        if (data.IndexOf("<EOT>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    data = data.Replace("<EOT>", "");
                    Terminal t = JsonConvert.DeserializeObject<Terminal>(data);

                    _response = cas.procesarTerminal(t);

                    byte[] message = Encoding.ASCII.GetBytes(_response);

                    // Envío del mensaje al cliente
                    clientSocket.Send(message);

                    // Cerramos el socket con el método Close().
                    // Después de cerrarlo, podemos utilizar el socket cerrado para una nueva conexión del cliente
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}