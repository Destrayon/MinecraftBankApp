using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    public static class DataBasing
    {
        public static DirectoryInfo usersFolder = new DirectoryInfo(@"C:\Users\Divie\Documents\Server Info\Users");

        public static int userAmount;

        public static void NewUser(string username, string password)
        {
            userAmount = usersFolder.GetFiles().Length;

            if (userAmount == 0)
            {
                using (FileStream w = File.Open(@"C:\Users\Divie\Documents\Server Info\Owner\Owner.txt", FileMode.Create))
                {
                    AddText(w, username);
                }

                User.SaveUser(username, password, "1", "0.00");
            }
            else
            {
                User.SaveUser(username, password, "0", "0.00");
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public static string CheckOwner()
        {
            return new StreamReader(@"C:\Users\Divie\Documents\Server Info\Owner\Owner.txt").ReadLine();
        }

        public static FileInfo[] GetFiles()
        {
            return usersFolder.GetFiles();
        }
    }
}
