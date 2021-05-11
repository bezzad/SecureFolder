using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ShellProgressBar;

namespace SecureFolder
{
    class Program
    {
        private static Options _opt;
        private static List<FileInfo> _files;
        private static readonly string ExecutionPath = Assembly.GetEntryAssembly()?.Location;
        private static ProgressBar _progress;
        private static string _currentFileName;
        private static ChildProgressBar _currentChildProgressBar;

        static void Main(string[] args)
        {
            Console.WriteLine(@"
            ░█▀▀▀█ █▀▀ █▀▀ █──█ █▀▀█ █▀▀ 　 ░█▀▀▀ █▀▀█ █── █▀▀▄ █▀▀ █▀▀█ 
            ─▀▀▀▄▄ █▀▀ █── █──█ █▄▄▀ █▀▀ 　 ░█▀▀▀ █──█ █── █──█ █▀▀ █▄▄▀ 
            ░█▄▄▄█ ▀▀▀ ▀▀▀ ─▀▀▀ ▀─▀▀ ▀▀▀ 　 ░█─── ▀▀▀▀ ▀▀▀ ▀▀▀─ ▀▀▀ ▀─▀▀");
            Console.WriteLine("\n\n");

            SecureFile.ProgressChanged += OnProgressChanged;
            Parser.Default.ParseArguments<Options>(args).WithParsed(ExecuteCommands).WithNotParsed(err => {
                Console.WriteLine("Given arguments is not valid!");
            });

            Console.WriteLine("\n Finished :)");
            Console.Read();
        }

        private static void ExecuteCommands(Options opt)
        {
            _opt = opt;
            Console.Title =
                $"{(opt.Encrypt ? "Encrypting" : "Decrypting")} {(string.IsNullOrWhiteSpace(opt.Directory) ? opt.FileName : opt.Directory)}";
            SetValidPassword(opt);
            FetchValidFiles(opt);

            Console.Clear();

            if (opt.Encrypt)
            {
                CreateProgressBar(_files.Count, "Encrypting");
                foreach (var file in _files)
                {
                    Encrypt(file);
                }
            }
            else if (opt.Decrypt)
            {
                CreateProgressBar(_files.Count, "Decrypting");
                foreach (var file in _files)
                {
                    Decrypt(file);
                }
            }
        }

        private static void OnProgressChanged(object sender, ProgressChangedEventArg e)
        {
            double percent = (double)e.ProgressedBytes / e.TotalBytes * 100;
            var message = $"{e.FileName} {percent:N1}% {(_opt.Encrypt ? "encrypted" : "decrypted")}";

            if (_currentFileName == e.FileName)
            {
                _currentChildProgressBar.Tick((int)percent, message);
            }
            else
            {
                var childProgressOption = new ProgressBarOptions()
                {
                    DisplayTimeInRealTime = false,
                    ProgressBarOnBottom = false,
                    ProgressCharacter = '_',
                    ShowEstimatedDuration = false,
                    ForegroundColor = ConsoleColor.DarkYellow
                };
                _currentFileName = e.FileName;
                _currentChildProgressBar = _progress.Spawn(100, message, childProgressOption);
            }
        }

        private static void CreateProgressBar(int totalTicks, string msg)
        {
            var options = new ProgressBarOptions {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = true,
                ProgressBarOnBottom = true,
                EnableTaskBarProgress = true
            };

            _progress = new ProgressBar(totalTicks, msg, options);
        }
        private static void SetValidPassword(Options opt)
        {
            if ((opt.Encrypt || opt.Decrypt) &&
                string.IsNullOrWhiteSpace(opt.Password))
            {
                opt.Password = GetPassword();
            }
            SecureFile.CreateAlgorithm(opt.Password);
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
            SecureFile.EncryptFile(file.FullName);
            _progress.Tick();
            if (_opt.Remove)
            {
                Remove(file);
            }
        }
        private static void Decrypt(FileInfo file)
        {
            SecureFile.DecryptFile(file.FullName);
            _progress.Tick();
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
