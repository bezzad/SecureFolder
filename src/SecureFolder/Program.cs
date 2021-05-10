using CommandLine;
using System;

namespace SecureFolder
{
    class Program
    {
        private static SecureFile _secureFile;
        private static Options _opt;

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
            _opt = opt;
            if ((opt.Encrypt || opt.Decrypt) &&
                string.IsNullOrWhiteSpace(opt.Password))
            {
                opt.Password = GetPassword();
            }

            if (opt.Encrypt)
            {
                _secureFile = new SecureFile(opt.Password);
                Encrypt();
            }
            else if (opt.Decrypt)
            {
                _secureFile = new SecureFile(opt.Password);
                Decrypt();
            }
        }

        private static string GetPassword()
        {
            Console.Write("Enter Password: ");
            return Helper.GetPassword().ToPlainString();
        }

        private static string[] GetFiles(string dir)
        {

        }

        private static void Encrypt()
        {

        }

        private static void Decrypt()
        {

        }
    }
}
