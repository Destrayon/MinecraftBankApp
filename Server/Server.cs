using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        private static byte[] buffer = new byte[1024];
        private static int port;
        private static List<Socket> clientSockets = new List<Socket>();
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Welcome to MCBank! Please type a valid Port.");

            string portString = Console.ReadLine();

            while (!int.TryParse(portString, out port))
            {
                Console.WriteLine("Port invalid, please type in a valid Port.");
                portString = Console.ReadLine();
            }

            SetupServer();
        }
        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(10);
            Console.WriteLine("Server is running.");
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            Console.Read();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = serverSocket.EndAccept(AR);
            Console.WriteLine("Client connected.");
            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            try
            {
                int received = socket.EndReceive(AR);
                byte[] dataBuf = new byte[received];

                Array.Copy(buffer, dataBuf, received);

                string command = Encoding.ASCII.GetString(dataBuf);
                byte[] data = Encoding.ASCII.GetBytes(Commands(command));
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch
            {
                Console.WriteLine("Client disconnected.");
                socket.Close();
                socket.Dispose();
            }
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        private static string Commands(string str)
        {
            string[] split = str.Split(new Char[] { ' ' });
            int i = int.Parse(split[0]);

            switch (i)
            {
                case 1:
                    if (User.UserExists(split[1]))
                    {
                        DirectoryInfo info = new DirectoryInfo(@"..\..\..\..\Server\Users");
                        User user = new User(info.FullName + @"\" + split[1] + ".txt");

                        if (user.password == split[2])
                        {
                            return "Auth successful";
                        }

                        return "Auth unsuccessful";
                    }
                    return "Auth unsuccessful";
            }

            return "";
        }
    }
}
