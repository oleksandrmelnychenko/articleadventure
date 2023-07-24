using common.ArticleAdventure.IdentityConfiguration;
using System.Security.Cryptography;
using System.Text;

namespace domain.ArticleAdventure.Helpers
{
    public static class AesManager
    {
        public static string Encrypt(string toEncryptString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(toEncryptString);

            using (Aes encryptor = Aes.Create())
            {
                if (encryptor == null) return toEncryptString;

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AuthOptions.KEY, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    toEncryptString = Convert.ToBase64String(ms.ToArray());
                }
            }

            return toEncryptString;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                if (encryptor == null) return cipherText;

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AuthOptions.KEY, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }

}
