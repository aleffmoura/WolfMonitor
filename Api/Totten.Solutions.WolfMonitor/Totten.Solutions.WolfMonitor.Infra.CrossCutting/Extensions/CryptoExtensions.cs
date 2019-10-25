using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions
{
    public static class CryptoExtensions
    {
        public static string GenerateHash(this string passwordToHash)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToHash,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
