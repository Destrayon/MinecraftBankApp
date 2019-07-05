using System;

namespace MinecraftBankApp
{
    class Program
    {
   static void StartUp()
        {
            Console.WriteLine("Hello, welcome to the Minecraft Banking App!");
            Console.WriteLine("To sign in, type 0. To sign up, type 1.");
            string command = Console.ReadLine();
        }

        static void CmdFailure()
        {
            Console.WriteLine("Command unrecognized, please type a valid command.");
        }
    }
}
