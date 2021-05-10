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
            SecureFile.CreateAlgorithm("123");

            // act
            var encryptedName = SecureFile.Encrypt(name);
            var decryptedName = SecureFile.Decrypt(encryptedName);

            // assert
            Assert.Equal(name, decryptedName);
        }
    }
}
