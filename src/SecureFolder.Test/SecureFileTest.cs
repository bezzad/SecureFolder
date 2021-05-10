using Xunit;

namespace SecureFolder.Test
{
    public class SecureFileTest
    {
        [Fact]
        public void TestEncryptString()
        {
            // arrange
            var name = "#1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ.exe";
            var secureFile = new SecureFile("12345678");

            // act
            var encryptedName = secureFile.Encrypt(name);
            var decryptedName = secureFile.Decrypt(encryptedName);

            // assert
            Assert.Equal(name, decryptedName);
        }
    }
}
