using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class RovarspraketController : ControllerBase
{
    static void Main()
    {
      [HttpPost("encrypt")]
    public IActionResult Encrypt([FromBody] RovarspraketRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Text))
        {
            return BadRequest("Text is required.");
        }

        string encryptedText = EncryptRovarspraket(request.Text);
        return Ok(new { EncryptedText = encryptedText });
    }

    [HttpPost("decrypt")]
    public IActionResult Decrypt([FromBody] RovarspraketRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Text))
        {
            return BadRequest("Text is required.");
        }

        string decryptedText = DecryptRovarspraket(request.Text);
        return Ok(new { DecryptedText = decryptedText });
    }

    private string EncryptRovarspraket(string text)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            if ("aeiouy".Contains(Char.ToLower(c)) || char.IsWhiteSpace(c))
            {
                result.Append(c);
            }
            else
            {
                result.Append(c + "o" + Char.ToLower(c));
            }
        }

        return result.ToString();
    }

    private string DecryptRovarspraket(string text)
    {
        StringBuilder result = new StringBuilder();
        int i = 0;

        while (i < text.Length)
        {
            result.Append(text[i]);

            if (!char.IsWhiteSpace(text[i]))
            {
                i += 2;
            }
            else
            {
                i++;
            }
        }

        return result.ToString();
    }
    }
}
public class RovarspraketRequest
{
    public string Text { get; set; }
}
