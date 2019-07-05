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
        public User(string path)
        {
            StreamReader fs = new StreamReader(path);
            username = new DirectoryInfo(path).Name.Replace(".txt", "");
            password = fs.ReadLine();
            permission = 1;
            balance = 450.46f;
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

            DirectoryInfo dirinfo = new DirectoryInfo(@"..\..\..\..\Server\Users");
            File.WriteAllLines(dirinfo.FullName + @"\" + user.username + ".txt", info);
        }

        public static void SaveUser(string username, string password, string permission, string balance)
        {
            string[] info =
            {
                password,
                permission,
                balance
            };

            DirectoryInfo dirinfo = new DirectoryInfo(@"..\..\..\..\Server\Users");
            File.WriteAllLines(dirinfo.FullName + @"\" + username + ".txt", info);
        }

        public static bool UserExists(string user)
        {
            DirectoryInfo info = new DirectoryInfo(@"..\..\..\..\Server\Users");
            if (File.Exists(info.FullName + @"\" + user + ".txt"))
            {
                return true;
            }

            return false;
        }

        public static string Test()
        {
            DirectoryInfo info = new DirectoryInfo(@"..\..\..\..\Server\Users");
            return info.FullName;
        }
    }
}
