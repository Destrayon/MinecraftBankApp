using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public static class Conversions
    {
        static DirectoryInfo conversionFile = new DirectoryInfo(@"C:\Users\Divie\Documents\Server Info\Conversions\");
        public static string ReturnConversions()
        {
            string conversions = "\n";
            using (StreamReader fs = new StreamReader(conversionFile.FullName + @"Conversions.txt"))
            {
                while (fs.Peek() >= 0)
                {
                    conversions += fs.ReadLine() + "\n";
                }
            }

            return conversions;
        }
    }
}
