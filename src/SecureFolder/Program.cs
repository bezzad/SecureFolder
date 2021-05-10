using System;
using System.Diagnostics;
using System.Linq;
using CommandLine;

namespace SecureFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
            ░█▀▀▀█ █▀▀ █▀▀ █──█ █▀▀█ █▀▀ 　 ░█▀▀▀ █▀▀█ █── █▀▀▄ █▀▀ █▀▀█ 
            ─▀▀▀▄▄ █▀▀ █── █──█ █▄▄▀ █▀▀ 　 ░█▀▀▀ █──█ █── █──█ █▀▀ █▄▄▀ 
            ░█▄▄▄█ ▀▀▀ ▀▀▀ ─▀▀▀ ▀─▀▀ ▀▀▀ 　 ░█─── ▀▀▀▀ ▀▀▀ ▀▀▀─ ▀▀▀ ▀─▀▀");
            Console.WriteLine("\n\n");

            Parser.Default.ParseArguments<Options>(args).WithParsed(SetState);
            Console.Read();
        }

        private static void SetState(Options opt)
        {
            if (opt.Encrypt)
            {

            }
        }

        private static void Encrypt(bool remove)
        {
            Console.Write("Enter Password: ");
            var password = Helper.GetPassword();
            var secure = new SecureFile(password);
            

        }

        private static void Decrypt(bool remove)
        {
            Console.Write("Enter Password: ");
            var password = Helper.GetPassword();
        }
    }
}
