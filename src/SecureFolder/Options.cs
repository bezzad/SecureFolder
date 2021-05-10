using CommandLine;

namespace SecureFolder
{
    public class Options
    {
        [Option('e', "encrypt", Required = false, HelpText = "Set crypto type to encrypt files.")]
        public bool Encrypt { get; set; }

        [Option('d', "decrypt", Required = false, HelpText = "Set crypto type to decrypt files.")]
        public bool Decrypt { get; set; }

        [Option('r', "remove", Default = false, Required = false, HelpText = "Delete all origin files of given directory after encryption or decryption.")]
        public bool Remove { get; set; }

        [Option('a', "address", Default = "", Required = false, HelpText = "Set the working directory.")]
        public string Directory { get; set; }

        [Option('f', "file", Required = false, HelpText = "Input file to be processed.")]
        public string FileName { get; set; }

        [Option('p', "password", Required = false, HelpText = "Set crypto cipher text")]
        public string Password { get; set; }
    }
}
