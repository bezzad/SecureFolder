using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecureFolder
{
    public class SecureFile : IDisposable
    {
        private const int BufferSize = 65536;
        private const string GlobalSalt = @"!@#$%^&*H\,g,d@1";
        private AesCryptoServiceProvider _algorithm;
        private ICryptoTransform _decryptor;
        private ICryptoTransform _encryptor;

        public SecureFile(string password)
        {
            var pass = password.PadRight(password.Length + (16 - password.Length % 16), '#');
            _algorithm = new AesCryptoServiceProvider {
                BlockSize = 128,
                KeySize = 128,
                Key = Encoding.UTF8.GetBytes(pass),
                IV = Encoding.ASCII.GetBytes(GlobalSalt.PadRight(16, '#')),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB
            };

            // Creates a symmetric AES encryption object using the current key and initialization vector (IV).  
            _encryptor = _algorithm.CreateEncryptor(_algorithm.Key, _algorithm.IV);
            _decryptor = _algorithm.CreateDecryptor(_algorithm.Key, _algorithm.IV);
        }

        public void EncryptFile(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var dir = Path.GetDirectoryName(filePath);

            var encryptedFilename = Encrypt(filename);
            var outputPath = Path.Combine(dir, encryptedFilename);

            CryptoFile(filePath, outputPath, true);
        }
        public void DecryptFile(string filePath)
        {
            var encryptedFilename = Path.GetFileName(filePath);
            var dir = Path.GetDirectoryName(filePath);

            var filename = Decrypt(encryptedFilename);
            var outputPath = Path.Combine(dir, filename);

            CryptoFile(filePath, outputPath, false);
        }
        private void CryptoFile(string inputPath, string outputPath, bool isEncryption)
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
                Console.Title = $"{inputPath} {total} bytes {(isEncryption ? "encrypted" : "decrypted")}";
            }
        }

        public string Encrypt(string name)
        {
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var base64 = Convert.ToBase64String(nameBytes);

            return base64;
        }
        public string Decrypt(string encryptedName)
        {
            var base64Bytes = Convert.FromBase64String(encryptedName);
            var originName = Encoding.UTF8.GetString(base64Bytes);
            return originName;
        }
        public byte[] Encrypt(byte[] rawBytes)
        {
            return PerformCrypto(_encryptor, rawBytes);
        }
        public byte[] Decrypt(byte[] encryptedByes)
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

        public void Dispose()
        {
            _decryptor?.Dispose();
            _encryptor?.Dispose();
            _algorithm?.Dispose();
        }
    }
}
