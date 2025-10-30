using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace MagazijnApp.Services;

public static class PasswordHasher
{
    public static (string hash, string salt) HashPassword(string password)
    {
        //Generate a random salt, preventing identical passwords from having the same hash
        //32 Bytes = 256 bits of random data
        byte[] saltBytes = new byte[32];

        //Using RNG for cryptographically secure random numbers
        //'using' ensures the rng is properly disposed after use
        //It's like saying ' use this tool, then put it away properly' 
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes); //Fill the array with random bytes
        }

        //Convert the byte array to a Base64 string for easy database storage       
        string salt = Convert.ToBase64String(saltBytes);

         //Hash the password combined with the salt using SHA256
        using (var sha256 = SHA256.Create())
        {
            // Combine the password and salt, then convert to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);

             // Compute the hash of the password+salt combination
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash byte array to a Base64 string for database storage
            string hash = Convert.ToBase64String(hashBytes);

            // Return both the hash and salt as a tuple
            //Tuple is an organised collection of values which cannot be changed
            return (hash, salt);
        }
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        // Use the same hashing process as during registration
        using (var sha256 = SHA256.Create())
        {
            // Combine the entered password with the stored salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);

            // Create a hash using the same method as HashPassword
            byte[] newHashBytes = sha256.ComputeHash(passwordBytes);
            string newHash = Convert.ToBase64String(newHashBytes);

            // Convert both hashes to byte arrays for secure comparison
            byte[] storedHashBytes = Convert.FromBase64String(hash);
            byte[] newHashBytesForCompare = Convert.FromBase64String(newHash);

            // Securely compare the hashes (prevents timing attacks)
            return CryptographicOperations.FixedTimeEquals(storedHashBytes, newHashBytesForCompare);
        }
    }
}


// //A salt is a random string of characters that gets added to a password before hashing.
// //In case people use the same password, their hash is different, so you can tell the passwords apart. 


// //    1 bit = a 0 or 1 (binary digit)

// // 8 bits = 1 byte

// // 256 bits = 32 bytes (256 ÷ 8)

// // Base64 encoding converts these 32 bytes into about 43 characters (A-Z, a-z, 0-9, +, /, =)

// //SHA256 is a secure hashing algorithm that produces a 256-bit (32-byte) hash