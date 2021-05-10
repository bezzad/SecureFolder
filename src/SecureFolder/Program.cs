using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SecureFolder
{
    class Program
    {
        private static SecureFile _secureFile;
        private static Options _opt;
        private static List<FileInfo> _files;
        private static readonly string ExecutionPath = Assembly.GetEntryAssembly()?.Location;

        static void Main(string[] args)
        {
            Console.WriteLine(@"
            ░█▀▀▀█ █▀▀ █▀▀ █──█ █▀▀█ █▀▀ 　 ░█▀▀▀ █▀▀█ █── █▀▀▄ █▀▀ █▀▀█ 
            ─▀▀▀▄▄ █▀▀ █── █──█ █▄▄▀ █▀▀ 　 ░█▀▀▀ █──█ █── █──█ █▀▀ █▄▄▀ 
            ░█▄▄▄█ ▀▀▀ ▀▀▀ ─▀▀▀ ▀─▀▀ ▀▀▀ 　 ░█─── ▀▀▀▀ ▀▀▀ ▀▀▀─ ▀▀▀ ▀─▀▀");
            Console.WriteLine("\n\n");

            Parser.Default.ParseArguments<Options>(args).WithParsed(ExecuteCommands);
            Console.WriteLine("Finished :)");
            Console.Read();
        }

        private static void ExecuteCommands(Options opt)
        {
            _opt = opt;
            SetValidPassword(opt);
            FetchValidFiles(opt);

            if (opt.Encrypt)
            {
                foreach (var file in _files)
                {
                    Encrypt(file);
                }
            }
            else if (opt.Decrypt)
            {
                foreach (var file in _files)
                {
                    Decrypt(file);
                }
            }
        }

        private static void SetValidPassword(Options opt)
        {
            if ((opt.Encrypt || opt.Decrypt) &&
                string.IsNullOrWhiteSpace(opt.Password))
            {
                opt.Password = GetPassword();
            }
            _secureFile = new SecureFile(opt.Password);
        }
        private static string GetPassword()
        {
            Console.Write("Enter Password: ");
            return Helper.GetPassword().ToPlainString();
        }
        private static void FetchValidFiles(Options opt)
        {
            _files = new List<FileInfo>();
            if (string.IsNullOrWhiteSpace(opt.FileName) == false)
            {
                _files.Add(new FileInfo(opt.FileName));
            }
            else if (string.IsNullOrWhiteSpace(opt.Directory) == false)
            {
                _files.AddRange(GetFiles(_opt.Directory));
            }
        }
        private static IEnumerable<FileInfo> GetFiles(string dir, string searchPattern = "*.*")
        {
            var directory = new DirectoryInfo(dir);
            return directory.GetFiles(searchPattern, SearchOption.AllDirectories)
                .SkipWhile(f => f.FullName.Equals(ExecutionPath));
        }
        private static void Encrypt(FileInfo file)
        {
            _secureFile.EncryptFile(file.FullName);
            if (_opt.Remove)
            {
                Remove(file);
            }
        }
        private static void Decrypt(FileInfo file)
        {
            _secureFile.DecryptFile(file.FullName);
            if (_opt.Remove)
            {
                Remove(file);
            }
        }
        private static void Remove(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
