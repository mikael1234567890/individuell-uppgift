using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var encryptionKey = "din_secret_key"; 

app.MapPost("/encrypt", (HttpContext context) =>
{
    using (var reader = new StreamReader(context.Request.Body))
    {
        var plaintext = reader.ReadToEnd();
        var encryptedText = Encrypt(plaintext, encryptionKey);
        return context.Response.WriteAsync(encryptedText);
    }
});

app.MapPost("/decrypt", (HttpContext context) =>
{
    using (var reader = new StreamReader(context.Request.Body))
    {
        var encryptedText = reader.ReadToEnd();
        var decryptedText = Decrypt(encryptedText, encryptionKey);
        return context.Response.WriteAsync(decryptedText);
    }
});

app.Run();

string Encrypt(string text, string key)
{
    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.IV = new byte[16];

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(text);
                }
            }

            var encryptedData = msEncrypt.ToArray();
            return Convert.ToBase64String(encryptedData);
        }
    }
}

string Decrypt(string encryptedText, string key)
{
    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.IV = new byte[16];

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
