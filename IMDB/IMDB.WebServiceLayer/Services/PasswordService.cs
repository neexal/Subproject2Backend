using System.Security.Cryptography;
using System.Text;

namespace IMDB.WebServiceLayer.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        // Generate a salt
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[16];
        rng.GetBytes(saltBytes);

        // Combine password and salt
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
        Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

        // Hash the combined bytes
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(combinedBytes);

        // Combine salt and hash for storage
        var result = new byte[saltBytes.Length + hashedBytes.Length];
        Array.Copy(saltBytes, 0, result, 0, saltBytes.Length);
        Array.Copy(hashedBytes, 0, result, saltBytes.Length, hashedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            var storedBytes = Convert.FromBase64String(hashedPassword);
            
            // Extract salt (first 16 bytes)
            var saltBytes = new byte[16];
            Array.Copy(storedBytes, 0, saltBytes, 0, 16);

            // Extract hash (remaining bytes)
            var storedHashBytes = new byte[storedBytes.Length - 16];
            Array.Copy(storedBytes, 16, storedHashBytes, 0, storedHashBytes.Length);

            // Hash the provided password with the extracted salt
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(combinedBytes);

            // Compare the hashes
            return hashedBytes.SequenceEqual(storedHashBytes);
        }
        catch
        {
            return false;
        }
    }
}
