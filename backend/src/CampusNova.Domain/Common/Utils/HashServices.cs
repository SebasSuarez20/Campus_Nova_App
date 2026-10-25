using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Common.Services
{
    public static class HashServices
    {
        public static string Hash(string password)
          => Convert.ToHexString(
              System.Security.Cryptography.SHA256.HashData(
                  System.Text.Encoding.UTF8.GetBytes(password)
              )).ToLower();
    }
}
