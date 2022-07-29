using System;
using System.Security.Cryptography;
using System.Text;

namespace task4.Services
{
	public class HashingService : IHashingService
	{
		public string GetHashString(string originString)
		{
			var bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(originString));

			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}
	}
}
