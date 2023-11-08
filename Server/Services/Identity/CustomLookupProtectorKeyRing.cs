using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EyEServer.Services.Identity;

public class CustomLookupProtectorKeyRing : ILookupProtectorKeyRing
{
    public string this[string keyId] => GetAllKeyIds().Where(x => x == keyId).FirstOrDefault();
    public string CurrentKeyId { get; } = Convert.ToBase64String([200, 15, 147, 5, 155, 78, 118, 57, 180, 179, 60, 150, 188, 18, 165, 134]);
    public IEnumerable<string> GetAllKeyIds()
    {
        // This is 24 bytes length
        return new List<string>
        {
            Convert.ToBase64String([200, 15, 147, 5, 155, 78, 118, 57, 180, 179, 60, 150, 188, 18, 165, 134]),
            Convert.ToBase64String([242, 207, 146, 81, 121, 231, 168, 93, 89, 130, 4, 68, 18, 185, 98, 154]),
            Convert.ToBase64String([101, 104, 174, 233, 88, 29, 20, 16, 21, 216, 249, 45, 148, 18, 102, 150])
        };
    }
}