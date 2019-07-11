using System;
using System.Collections.Generic;
using System.Text;

namespace MCBankApp
{
    public static class ClientCommands
    {
        public static string Commands(string cmd, string username, int permission)
        {
            string[] split = cmd.Split(new Char[] { ' ' });

            if (split[0] == "/check" && split.Length > 1)
            {
                if (split[1] == "balance" && split.Length == 2)
                {
                    return CheckBalance(username);
                }

                if (split[1] == "balance" && split.Length == 3 && permission == 1)
                {
                    return CheckBalance(split[2]);
                }

                if (split[1] == "users")
                {
                    return CheckUsers();
                }

                if (split[1] == "permissions" && split.Length == 2)
                {
                    int i = permission;
                    if (i == 0)
                    {
                        return "You have basic permissions.";
                    }
                    else
                    {
                        return "You have administrator permissions.";
                    }
                }

                if (split[1] == "permissions" && split.Length == 3)
                {
                    if (split[2] == "raw")
                    {
                        return permission.ToString();
                    }

                    if (permission == 1)
                    {
                        int i = int.Parse(CheckPermissions(split[2]));

                        if (i == -1)
                        {
                            return $"{split[2]} does not exist.";
                        }
                        else if (i == 0)
                        {
                            return $"{split[2]} has basic permissions.";
                        }
                        else
                        {
                            return $"{split[2]} has administrator permissions.";
                        }
                    }
                }
            }

            if (split[0] == "/fund" && split.Length > 2)
            {
                if (permission == 1 && split.Length == 4)
                {
                    if (split[3] == "bankfund")
                    {
                        return Funding(username, split[1], split[2], 1);
                    }
                }

                if (split.Length == 3)
                {
                    return Funding(username, split[1], split[2], 0);
                }
            }

            if (split[0] == "/withdraw" && permission == 1 && split.Length == 3)
            {
                return Withdrawal(split[1], split[2]);
            }

            if (split[0] == "/set" && split.Length == 4 && split[2] == "permissions")
            {
                return Permission(split[1], split[3]);
            }

            if (split[0] == "/get" && split.Length > 1)
            {
                if (split[1] == "transactions")
                {
                    if (split.Length == 2)
                    {
                        return Transactions(username, 0);
                    }

                    if (split.Length == 3 && permission == 1)
                    {
                        if (split[2] == "all")
                        {
                            return Transactions(username, 1);
                        } else
                        {
                            return Transactions(split[2], 0);
                        }
                    }
                }

                if (split[1] == "conversions" && split.Length == 2)
                {
                    return Conversions();
                }
            }

            if (split[0] == "/help" && split.Length == 1)
            {
                return CommandList(permission);
            }

            if (split[0] == "/clear" && split.Length == 1)
            {
                Console.Clear();
                return "Console has been cleared.";
            }

            return "Invalid command";
        }

        private static string CommandList(int permission)
        {
            string commandsList = "\n";

            if (permission == 1)
            {
                commandsList += "/check balance {username}\n/check permissions {username}\n/fund {username} {amount} bankfund\n/withdraw {username} {amount}\n/set {username} permissions {permission}\n/get transactions all\n/get transactions {username}\n";
            }

            commandsList += "/check balance\n/check users\n/check permissions\n/check permissions raw\n/fund {username} {amount}\n/get transactions\n/get conversions\n/clear\n";

            return commandsList;
        }

        private static string Conversions()
        {
            byte[] answer = Client.SendRequest($"12");

            return Encoding.ASCII.GetString(answer);
        }

        private static string Transactions(string username, int permission)
        {
            if (permission == 1)
            {
                byte[] answer = Client.SendRequest($"10");

                return Encoding.ASCII.GetString(answer);
            }

            byte[] answer2 = Client.SendRequest($"11 {username}");

            return Encoding.ASCII.GetString(answer2);
        }

        private static string Permission(string recipient, string permission)
        {
            byte[] answer = Client.SendRequest($"9 {recipient} {permission}");

            return Encoding.ASCII.GetString(answer);
        }

        private static string Withdrawal(string recipient, string funds)
        {
            byte[] answer = Client.SendRequest($"8 {recipient} {funds}");

            return Encoding.ASCII.GetString(answer);
        }

        private static string Funding(string username, string recipient, string funds, int permissionLevel)
        {
            if (permissionLevel == 1)
            {
                byte[] answer = Client.SendRequest($"6 {recipient} {funds}");

                return Encoding.ASCII.GetString(answer);
            }
            else
            {
                byte[] answer = Client.SendRequest($"7 {username} {recipient} {funds}");

                return Encoding.ASCII.GetString(answer);
            }
        }

        private static string CheckBalance(string username)
        {
            byte[] answer = Client.SendRequest($"3 {username}");

            return Encoding.ASCII.GetString(answer);
        }

        private static string CheckUsers()
        {
            byte[] answer = Client.SendRequest($"4");

            return Encoding.ASCII.GetString(answer);
        }

        private static string CheckPermissions(string username)
        {
            byte[] answer = Client.SendRequest($"5 {username}");
            return Encoding.ASCII.GetString(answer);
        }
    }
}
