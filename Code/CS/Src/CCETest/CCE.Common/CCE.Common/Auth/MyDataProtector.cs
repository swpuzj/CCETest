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
            return AesEncrypt.Encrypt(plaintext, "SobeyHive1234567", "SobeyHive1234567");
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return AesEncrypt.Decrypt(protectedData, "SobeyHive1234567", "SobeyHive1234567");
        }
    }
}