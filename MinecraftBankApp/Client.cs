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
        private static string username;
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

            if (!clientSocket.Connected)
            {
                return;
            }

            Console.Clear();

            Console.WriteLine("If you would like to login, please type 0. If you would like to sign up, please type 1.");

            string num = Console.ReadLine();

            int result;

            while (!int.TryParse(num, out result) || result > 1 || result < 0)
            {
                Console.WriteLine("Incorrect output, please try again.");

                num = Console.ReadLine();

            }

            Console.Clear();

            if (result == 0)
            {
                Login();
            }
            else
            {
                SignUp();
            }
        }

        private static void SignUp()
        {
            Console.WriteLine("Please type in a username.");

            string un = Console.ReadLine();

            Console.WriteLine("Please type in a password.");

            string pw = Console.ReadLine();

            Console.WriteLine("Please type in your password again.");

            string pw2 = Console.ReadLine();

            while (pw != pw2)
            {
                Console.Clear();
                Console.WriteLine("Passwords did not match, please try again.");

                Console.WriteLine("Please type in a username.");

                un = Console.ReadLine();

                Console.WriteLine("Please type in a password.");

                pw = Console.ReadLine();

                Console.WriteLine("Please type in your password again.");

                pw2 = Console.ReadLine();
            }

            byte[] answer = SendRequest($"2 {un} {pw}");
            
            string ans = Encoding.ASCII.GetString(answer);

            if (ans == "Signup failed")
            {
                Console.Clear();
                Console.WriteLine("Username already exists, please try again.");
                SignUp();
            }
            else
            {
                Console.WriteLine($"Welcome {un}!");
                username = un;
            }
        }

        private static void Login()
        {
            Console.WriteLine("Please type in your username.");
            string un = Console.ReadLine();
            Console.WriteLine("Please type in your password.");
            string pw = Console.ReadLine();

            byte[] answer = SendRequest($"1 {un} {pw}");

            string ans = Encoding.ASCII.GetString(answer);

            while (ans == "Auth unsuccessful")
            {
                Console.Clear();
                Console.WriteLine("Credentials incorrect, please try again.");

                Console.WriteLine("Please type in your username.");
                un = Console.ReadLine();
                Console.WriteLine("Please type in your password.");
                pw = Console.ReadLine();

                answer = SendRequest($"1 {un} {pw}");

                ans = Encoding.ASCII.GetString(answer);
            }

            Console.WriteLine($"Welcome {un}!");
            username = un;
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
                        return;
                    }
                }
            }

            Console.WriteLine("Successfully connected to the server!");
        }
    }
}
