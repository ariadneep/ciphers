namespace Ciphers;

public class Program
{
    static void Main(string[] args)
    {
        int[,] key = new int[,]{{ 1, 3 },{ 2, 5 } };
        string message = "message";

        string encryption = HillCipher.Encrypt(key, message);
        Console.WriteLine(encryption.ToString());
    }
}
