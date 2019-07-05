using System;
using Server;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MinecraftBankApp
{
    class Client
    {
        static IPAddress address;
        static int port;
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Start();
        }

        static void Start()
        {
            Console.WriteLine("Welcome to MC Bank! Please type in a valid IP Address.");
            string ipAddress = Console.ReadLine();

            while (!IPAddress.TryParse(ipAddress, out address))
            {
                Console.WriteLine("IP Address invalid, please type in a valid IP Address.");
                ipAddress = Console.ReadLine();
            }

            Console.WriteLine("Please enter a valid port.");

            string portString = Console.ReadLine();

            while (!int.TryParse(portString, out port))
            {
                Console.WriteLine("Port invalid, please type in a valid Port.");
                portString = Console.ReadLine();
            }

            ConnectLoop();

            while (!clientSocket.Connected)
            {
                
            }

            Console.WriteLine("Please type in your username.");
            string un = Console.ReadLine();
            Console.WriteLine("Please type in your password.");
            string pw = Console.ReadLine();

            byte[] answer = SendRequest($"1 {un} {pw}");

            string ans = Encoding.ASCII.GetString(answer);
            Console.WriteLine(ans);
            Console.ReadLine();
        }

        static byte[] SendRequest(string s)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            clientSocket.Send(buffer);

            byte[] receivedBuf = new byte[1024];
            int rec = clientSocket.Receive(receivedBuf);
            byte[] data = new byte[rec];
            Array.Copy(receivedBuf, data, rec);

            return data;
        }
        private static void ConnectLoop()
        {
            Console.Clear();
            int attempts = 0;
            while (!clientSocket.Connected)
            {
                try
                {
                    clientSocket.Connect(address, port);
                }
                catch (SocketException)
                {
                    attempts++;
                    Console.Clear();
                    Console.WriteLine("Attempting to Connect: " + attempts);

                    if (attempts == 5)
                    {
                        Console.WriteLine("Could not connect to the server. Please try again later.");
                        Console.ReadLine();
                    }
                }
            }

            Console.WriteLine("Successfully connected to the server!");
        }
    }
}
