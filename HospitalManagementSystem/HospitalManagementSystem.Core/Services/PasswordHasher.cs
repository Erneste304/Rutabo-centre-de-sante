using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Core.Services
{
    public static class PasswordHasher
    {
        private const int Iterations = 310_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const string Prefix = "pbkdf2-sha256";

        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

            return string.Join('$',
                Prefix,
                Iterations.ToString(CultureInfo.InvariantCulture),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('$');
            if (parts.Length == 4 && parts[0] == Prefix &&
                int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var iterations) &&
                iterations is >= 100_000 and <= 1_000_000)
            {
                try
                {
                    var salt = Convert.FromBase64String(parts[2]);
                    var expectedHash = Convert.FromBase64String(parts[3]);
                    if (salt.Length != SaltSize || expectedHash.Length != HashSize)
                        return false;

                    var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                        password, salt, iterations, HashAlgorithmName.SHA256, HashSize);
                    return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return VerifyLegacySha256(password, storedHash);
        }

        public static bool NeedsRehash(string storedHash)
        {
            var parts = storedHash.Split('$');
            return parts.Length != 4 || parts[0] != Prefix ||
                   !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var iterations) ||
                   iterations != Iterations;
        }

        private static bool VerifyLegacySha256(string password, string storedHash)
        {
            try
            {
                var expectedHash = Convert.FromBase64String(storedHash);
                var actualHash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
                return expectedHash.Length == actualHash.Length &&
                       CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
