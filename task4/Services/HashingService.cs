using System;
using System.Security.Cryptography;
using System.Text;

namespace task4.Services
{
    public class HashingService : IHashingService
    {
        public string GetHashString(string originString)
        {
            var bytes = Encoding.UTF8.GetBytes(originString);
            var hashingBytes = SHA256.Create().ComputeHash(bytes);

            return BitConverter.ToString(hashingBytes).Replace("-", "").ToLower();
        }
    }
}
