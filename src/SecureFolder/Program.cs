using System;
using System.Diagnostics;
using System.Linq;

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

            var state = IsEncryption(args);
            if (state.HasFlag(State.Encryption))
            {
                Encrypt(state.HasFlag(State.Remove));
            }
            else if (state.HasFlag(State.Decryption))
            {
                Decrypt(state.HasFlag(State.Remove));
            }
            else
            {
                Help();
            }

            Console.Read();
        }

        private static State IsEncryption(string[] args)
        {
            var state = State.Help;

            if (args.Any(a =>
                a.Equals("-e", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("-encrypt", StringComparison.OrdinalIgnoreCase)))
            {
                state = State.Encryption;
            }

            if (args.Any(a =>
                a.Equals("-d", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("-decrypt", StringComparison.OrdinalIgnoreCase)))
            {
                state = State.Decryption;
            }

            if (args.Any(a =>
                a.Equals("-r", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("-remove", StringComparison.OrdinalIgnoreCase)))
            {
                state |= State.Remove;
            }

            return state;
        }

        private static void Help()
        {
            Console.WriteLine();
            Console.WriteLine("-e -encrypt \t\t Encrypt all files of current directory.");
            Console.WriteLine("-d -decrypt \t\t Decrypt all files of current directory.");
            Console.WriteLine("-r -remove \t\t Delete all files of current directory after encryption or decryption.");
            Console.WriteLine("-h -help \t\t commands list help.");
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
