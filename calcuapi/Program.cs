using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Skriv in texten som du vill kryptera:");
        string originalText = Console.ReadLine();

        Console.WriteLine("Skriv in en nyckel för kryptering:");
        string encryptionKey = Console.ReadLine();

        // Kryptera texten
        string encryptedText = Encrypt(originalText, encryptionKey);
        Console.WriteLine($"Krypterad text: {encryptedText}");

        // Dekryptera texten
        string decryptedText = Decrypt(encryptedText, encryptionKey);
        Console.WriteLine($"Dekrypterad text: {decryptedText}");
    }

    static string Encrypt(string text, string key)
    {
        char[] encryptedChars = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            encryptedChars[i] = (char)(text[i] ^ key[i % key.Length]);
        }
        return new string(encryptedChars);
    }

    static string Decrypt(string text, string key)
    {
        return Encrypt(text, key); // XOR-kryptering är symmetrisk, så att dekryptera är samma som att kryptera igen
    }
}
