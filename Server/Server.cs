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
            DirectoryCreation.CreateDirectories();
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
                        User user = new User(split[1]);

                        if (user.password == split[2])
                        {
                            Console.WriteLine($"{user.username} has logged in.");
                            return "Auth successful";
                        }

                        return "Auth unsuccessful";
                    }

                    return "Auth unsuccessful";
                case 2:
                    if (!User.UserExists(split[1]))
                    {
                        DataBasing.NewUser(split[1], split[2]);
                        Console.WriteLine($"{split[1]} has logged in.");
                        return "Signup successful";
                    }
                    return "Signup failed";
                case 3:
                    return new User(split[1]).balance.ToString("0.00");
                case 4:
                    FileInfo[] information = DataBasing.GetFiles();

                    string usernames = "";

                    for (int j = 0; j < information.Length; j++)
                    {
                        if (j < information.Length - 1)
                        {
                            usernames += information[j].Name.Replace(".txt", ", ");
                        }
                        else
                        {
                            usernames += information[j].Name.Replace(".txt", "");
                        }
                    }

                    return usernames;
                case 5:
                    if (User.UserExists(split[1]))
                    {
                        return new User(split[1]).permission.ToString();
                    }

                    return "-1";
                case 6:
                    if (User.UserExists(split[1]))
                    {
                        User user = new User(split[1]);

                        float num;

                        if (float.TryParse(split[2], out num))
                        {
                            if (num < 0)
                            {
                                return "The fund parameter cannot be a negative number.";
                            }
                            user.balance += num;
                            User.SaveUser(user);
                            Transaction.AppendTransaction(user.username, "Bank", "Deposit", num.ToString("0.00"));
                            return "Funding successful.";
                        }
                        return "The fund parameter is not a float.";
                    }
                    return "User does not exist.";
                case 7:
                    if (User.UserExists(split[2]))
                    {
                        User user = new User(split[1]);
                        User user2 = new User(split[2]);

                        float num;

                        if (float.TryParse(split[3], out num))
                        {
                            if (num < 0)
                            {
                                return "The fund parameter cannot be a negative number.";
                            }

                            if (user.balance < num)
                            {
                                return "Your balance is too low for this transaction.";
                            }

                            if (user.username.ToLower() == user2.username.ToLower())
                            {
                                return "You cannot fund yourself.";
                            }

                            user2.balance += num;
                            user.balance -= num;
                            User.SaveUser(user);
                            User.SaveUser(user2);
                            Transaction.AppendTransaction(user.username, user2.username, "Withdrawal", num.ToString("0.00"));
                            Transaction.AppendTransaction(user2.username, user.username, "Deposit", num.ToString("0.00"));
                            return "Funding successful.";
                        }
                        return "The fund parameter is not a float.";
                    }
                    return "User does not exist.";
                case 8:
                    if (User.UserExists(split[1]))
                    {
                        User user = new User(split[1]);

                        float num;

                        if (float.TryParse(split[2], out num))
                        {
                            if (num < 0)
                            {
                                return "The funds parameter cannot be a negative number.";
                            }
                            user.balance -= num;
                            User.SaveUser(user);
                            Transaction.AppendTransaction(user.username, "Bank", "Withdrawal", num.ToString("0.00"));
                            return "Withdraw successful.";
                        }
                        return "The funds parameter is not a float.";
                    }
                    return "User does not exist.";
                case 9:
                    if (User.UserExists(split[1]))
                    {
                        int permission;

                        if (!int.TryParse(split[2], out permission))
                        {
                            return "Permission parameter is not a number.";
                        }

                        if (permission < 0 || permission > 1)
                        {
                            return "Permission parameter is out of range.";
                        }

                        User user = new User(split[1]);

                        if (user.permission == permission && permission == 1)
                        {
                            return $"{split[1]} already has admin permissions.";
                        }

                        if (user.permission == permission && permission == 0)
                        {
                            return $"{split[1]} already has basic permissions.";
                        }

                        if (user.username.ToLower() == DataBasing.CheckOwner().ToLower())
                        {
                            return "Admin permissions cannot be revoked from owner.";
                        }

                        user.permission = permission;

                        User.SaveUser(user);

                        if (permission == 1)
                        {
                            return $"{split[1]} has been granted admin permissions.";
                        }

                        return $"{split[1]} has been granted basic permissions.";
                    }

                    return "User does not exist.";
                case 10:
                    return Transaction.GetTransactions();
                case 11:
                    if (User.UserExists(split[1]))
                    {
                        return Transaction.GetTransactions(split[1]);
                    }

                    return "User does not exist.";
                case 12:
                    return Conversions.ReturnConversions();
            }
            return "";
        }
    }
}
