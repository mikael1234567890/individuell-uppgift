using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
var app = builder.Build();

app.MapPost("/encrypt", async context =>
{
    string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    string encryptedText = EncryptText(requestBody);
    await context.Response.WriteAsync(encryptedText);
});

app.MapGet("/decrypt", async context =>
{
    string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    string decryptedText = DecryptText(requestBody);
    await context.Response.WriteAsync(decryptedText);
});

app.Run();

string EncryptText(string text)
{
    char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'å', 'ä', 'ö' };

    StringBuilder encryptedText = new StringBuilder();
    foreach (char c in text)
    {
        encryptedText.Append(c);
        if (char.IsLetter(c) && !vowels.Contains(char.ToLower(c)))
        {
            encryptedText.Append('o').Append(char.ToLower(c));
        }
    }

    return encryptedText.ToString();
}

string DecryptText(string text)
{
    StringBuilder decryptedText = new StringBuilder();
    for (int i = 0; i < text.Length; i++)
    {
        if (i < text.Length - 1 && text[i] == 'o' && char.IsLetter(text[i + 1]))
        {
            i++; 
        }
        else
        {
            decryptedText.Append(text[i]);
        }
    }

    return decryptedText.ToString();
}
