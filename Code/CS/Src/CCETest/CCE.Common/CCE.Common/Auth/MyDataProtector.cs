using Microsoft.AspNetCore.DataProtection;

namespace CCE.Common.Auth
{
    public class MyDataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new MyDataProtector();
        }

        public byte[] Protect(byte[] plaintext)
        {
            return AesEncrypt.Encrypt(plaintext, "TestaTest1234567", "TestaTest1234567");
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return AesEncrypt.Decrypt(protectedData, "TestaTest1234567", "TestaTest1234567");
        }
    }
}