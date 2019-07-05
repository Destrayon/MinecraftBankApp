using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    public static class DataBasing
    {
        public static DirectoryInfo usersFolder = new DirectoryInfo(@"..\..\..\..\Server\Users");

        public static int userAmount = usersFolder.GetFiles().Length;

        public static void NewUser(string username, string password)
        {
            if (userAmount == 0)
            {
                User.SaveUser(username, password, "1", "0.00");
            }
            else
            {
                User.SaveUser(username, password, "0", "0.00");
            }
        }
    }
}
