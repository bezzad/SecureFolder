using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecureFolder
{
    public static class SecureFile
    {
        private const int BufferSize = 65536; // Note: changing this not effected on encryption or decryption speed
        private const string GlobalSalt = @"!@#$%^&*H\,g,d@1";
        private static AesCryptoServiceProvider _algorithm;
        private static ICryptoTransform _decryptor;
        private static ICryptoTransform _encryptor;
        public static event EventHandler<ProgressChangedEventArg> ProgressChanged;

        public static void CreateAlgorithm(string password)
        {
            Clear();
            var pass = password.PadRight(password.Length + (16 - password.Length % 16), '#');
            _algorithm = new AesCryptoServiceProvider {
                BlockSize = 128,
                KeySize = 256,
                Key = Encoding.UTF8.GetBytes(pass),
                IV = Encoding.ASCII.GetBytes(GlobalSalt.PadRight(16, '#')),
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB
            };

            // Creates a symmetric AES encryption object using the current key and initialization vector (IV).  
            _encryptor = _algorithm.CreateEncryptor(_algorithm.Key, _algorithm.IV);
            _decryptor = _algorithm.CreateDecryptor(_algorithm.Key, _algorithm.IV);
        }
        public static void EncryptFile(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var dir = Path.GetDirectoryName(filePath);

            var encryptedFilename = Encrypt(filename);
            var outputPath = Path.Combine(dir, encryptedFilename);

            CryptoFile(filePath, outputPath, true);
        }
        public static void DecryptFile(string filePath)
        {
            var encryptedFilename = Path.GetFileName(filePath);
            var dir = Path.GetDirectoryName(filePath);

            var filename = Decrypt(encryptedFilename);
            var outputPath = Path.Combine(dir, filename);

            CryptoFile(filePath, outputPath, false);
        }
        private static void CryptoFile(string inputPath, string outputPath, bool isEncryption)
        {
            var total = 0L;
            var buffer = new byte[BufferSize];
            var readCount = 1;
            using var readerStream = File.OpenRead(inputPath);
            using var writerStream = File.Create(outputPath);

            while (readCount > 0)
            {
                readCount = readerStream.Read(buffer);

                var result = isEncryption
                    ? Encrypt(buffer.Take(readCount).ToArray())
                    : Decrypt(buffer.Take(readCount).ToArray());

                writerStream.Write(result);
                total += readCount;
                OnProgressChanged(new ProgressChangedEventArg() { FileName = inputPath, TotalBytes = readerStream.Length, ProgressedBytes = total });
            }
        }

        public static string Encrypt(string name)
        {
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var base64 = Convert.ToBase64String(nameBytes);

            return base64;
        }
        public static string Decrypt(string encryptedName)
        {
            var base64Bytes = Convert.FromBase64String(encryptedName);
            var originName = Encoding.UTF8.GetString(base64Bytes);
            return originName;
        }
        public static byte[] Encrypt(byte[] rawBytes)
        {
            return PerformCrypto(_encryptor, rawBytes);
        }
        public static byte[] Decrypt(byte[] encryptedByes)
        {
            return PerformCrypto(_decryptor, encryptedByes);
        }
        private static byte[] PerformCrypto(ICryptoTransform crypto, byte[] bytes)
        {
            //TransformFinalBlock is a special function for transforming the last block or a partial block in the stream.   
            //It returns a new array that contains the remaining transformed bytes. A new array is returned, because the amount of   
            //information returned at the end might be larger than a single block when padding is added.  
            var transformed = crypto.TransformFinalBlock(bytes, 0, bytes.Length);
            return transformed;
        }

        public static void Clear()
        {
            _encryptor?.Dispose();
            _decryptor?.Dispose();
            _algorithm?.Dispose();
        }

        private static void OnProgressChanged(ProgressChangedEventArg e)
        {
            ProgressChanged?.Invoke(null, e);
        }
    }
}
