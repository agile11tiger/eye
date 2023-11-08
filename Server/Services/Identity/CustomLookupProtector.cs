using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace EyEServer.Services.Identity;

/// <summary>
/// https://dev.to/mohammedahmed/encrypt-and-decrypt-users-data-in-aspnet-core-identity-using-the-ilookupprotector-and-ilookupprotectorkeyring-interfaces-2gep
/// </summary>
public class CustomLookupProtector : ILookupProtector
{
    private readonly byte[] _iv = [208, 148, 29, 187, 168, 51, 181, 178, 137, 83, 40, 13, 28, 177, 131, 248];
    public string Protect(string keyId, string data)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(data);
        using var algorithm = Aes.Create();
        using var encryptor = algorithm.CreateEncryptor(Encoding.UTF8.GetBytes(keyId), _iv);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.Close();
        var chiperTextByte = memoryStream.ToArray();
        var cipherText = Convert.ToBase64String(chiperTextByte);
        return cipherText;
    }

    public string Unprotect(string keyId, string data)
    {
        var cipherTextBytes = Convert.FromBase64String(data);
        using var algorithm = Aes.Create();
        using var decrypter = algorithm.CreateDecryptor(Encoding.UTF8.GetBytes(keyId), _iv);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        var plainText = streamReader.ReadToEnd();
        return plainText;
    }
}