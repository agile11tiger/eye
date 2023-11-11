using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace EyEServer.Services.Identity;

public class PasswordHasherCustom : IPasswordHasher<UserModel>
{
    private readonly static string _keyId = Convert.ToBase64String([200, 15, 147, 5, 155, 78, 118, 57, 180, 179, 60, 150, 188, 18, 165, 134]);
    private readonly static byte[] _iv = [208, 148, 29, 187, 168, 51, 181, 178, 137, 83, 40, 13, 28, 177, 131, 248];

    public string HashPassword(UserModel user, string password)
    {
        return Protect(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(UserModel user, string hashedPassword, string providedPassword)
    {
        return hashedPassword == Protect(providedPassword)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }

    public static string Unprotect(string password)
    {
        var cipherTextBytes = Convert.FromBase64String(password);
        using var algorithm = Aes.Create();
        using var decrypter = algorithm.CreateDecryptor(Encoding.UTF8.GetBytes(_keyId), _iv);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        var plainText = streamReader.ReadToEnd();
        return plainText;
    }

    private static string Protect(string password)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(password);
        using var algorithm = Aes.Create();
        using var encryptor = algorithm.CreateEncryptor(Encoding.UTF8.GetBytes(_keyId), _iv);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.Close();
        var chiperTextByte = memoryStream.ToArray();
        var cipherText = Convert.ToBase64String(chiperTextByte);
        return cipherText;
    }
}