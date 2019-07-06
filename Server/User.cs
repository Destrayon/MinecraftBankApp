using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public class User
    {
        public string username { get; private set; }
        public string password;
        public int permission;
        public float balance;
        static string path = @"C:\Users\Divie\Documents\Server Info\Users\";
        public User(string user)
        {
            string userPath = path + user + ".txt";
            StreamReader fs = new StreamReader(userPath);
            username = new DirectoryInfo(userPath).Name.Replace(".txt", "");
            password = fs.ReadLine();
            permission = int.Parse(fs.ReadLine());
            balance = float.Parse(fs.ReadLine());
            fs.Close();
        }

        public static void SaveUser(User user)
        {
            string[] info =
            {
                user.password,
                user.permission.ToString(),
                user.balance.ToString()
            };

            File.WriteAllLines(path + user.username + ".txt", info);
        }

        public static void SaveUser(string username, string password, string permission, string balance)
        {
            string[] info =
            {
                password,
                permission,
                balance
            };

            File.WriteAllLines(path + username + ".txt", info);
        }

        public static bool UserExists(string user)
        {
            if (File.Exists(path + user + ".txt"))
            {
                return true;
            }

            return false;
        }
    }
}
