using System.Security.Cryptography;

namespace Identity.API.Services
{
    public static class RandomStringGenerator
    {
        public static string Generate(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            using (var crypto = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                byte[] buffer = null;

                var maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                var result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    var value = data[i];
                    while (value > maxRandom)
                    {
                        buffer ??= new byte[1];

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }
    }
}
