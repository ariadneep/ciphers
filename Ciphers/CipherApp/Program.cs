using System.Net.NetworkInformation;

namespace Ciphers;
/// <summary>
/// Program to run the ciphers programmed in this Solution. 
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        double[][] key = {[ -1, 3 ],[ 9, 5 ]};
        string message = "message";

        string encryption = HillCipher.Encrypt(key, message);
        Console.WriteLine(message + " encryption with key " + MatrixToString(key) + ": " + encryption.ToString());

        string decryption = HillCipher.Decrypt(key, encryption);
        Console.WriteLine(encryption + " decryption with key " + MatrixToString(key) + ": " + decryption.ToString());
    }
    
    /// <summary>
    /// Returns a string representation of an integer matrix.
    /// </summary>
    /// <param name="matrix"> the matrix to display as a string</param>
    /// <returns> the string form of a matrix in MATLAB format</returns>
    private static string MatrixToString(double[][] matrix)
    {
        string str = "[ ";
        for(int row = 0; row < matrix.Length; row++)
        {
            for(int col = 0; col < matrix[0].Length; col++)
            {
                str += matrix[row][col];
                if (col < matrix[0].Length - 1)
                    str += " ";
            }
            if(row < matrix.Length - 1)
                str += " ; ";
        }
        return str + " ]";
    }
}