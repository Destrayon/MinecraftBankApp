using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    public static class DirectoryCreation
    {
        static string path = @"C:\Users\Divie\Documents\Server Info\";
        public static void CreateDirectories()
        {
            Directory.CreateDirectory(path + "Conversions");
            Directory.CreateDirectory(path + "Users");
            Directory.CreateDirectory(path + "Transaction History");
            Directory.CreateDirectory(path + "Owner");
            FileStream fs = File.Open(path + @"Conversions\Conversions.txt", FileMode.OpenOrCreate);
            fs.Close();
            fs = File.Open(path + @"Transaction History\History.txt", FileMode.OpenOrCreate);
            fs.Close();
            fs = File.Open(path + @"Owner\Owner.txt", FileMode.OpenOrCreate);
            fs.Close();
        }
    }
}
