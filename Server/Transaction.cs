using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server
{
    public static class Transaction
    {
        static DirectoryInfo transactionFile = new DirectoryInfo(@"C:\Users\Divie\Documents\Server Info\Transaction History");

        public static void AppendTransaction(string recipient, string funder, string type, string amount)
        {
            using (StreamWriter w = File.AppendText(transactionFile.FullName + @"\History.txt"))
            {
                w.WriteLine($"{recipient} {funder} {type} {amount}");
            }
        }

        public static string GetTransactions()
        {
            string transactions = "\n";
            using (StreamReader fs = new StreamReader(transactionFile.FullName + @"\History.txt"))
            {
                int lines = File.ReadLines(transactionFile.FullName + @"\History.txt").Count() - 32;

                if (lines < 0)
                {
                    lines = 0;
                }

                for (int i = 0; i < lines; i++)
                {
                    fs.ReadLine();
                }

                while (fs.Peek() >= 0)
                {
                    transactions += fs.ReadLine() + "\n";
                }
            }

            return transactions;
        }

        public static string GetTransactions(string username)
        {
            string transactions = "\n";

            using (StreamReader fs = new StreamReader(transactionFile.FullName + @"\History.txt"))
            {
                int lines = File.ReadLines(transactionFile.FullName + @"\History.txt").Count() - 32;

                if (lines < 0)
                {
                    lines = 0;
                }

                for (int i = 0; i < lines; i++)
                {
                    fs.ReadLine();
                }

                while (fs.Peek() >= 0)
                {
                    string transaction = fs.ReadLine();

                    string[] split = transaction.Split(new char[] { ' ' });

                    if (split[0].ToLower() == username.ToLower())
                    {
                        transactions += transaction + "\n";
                    }
                }
            }

            return transactions;
        }
    }
}
