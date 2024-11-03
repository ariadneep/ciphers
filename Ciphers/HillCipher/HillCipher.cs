using System;
using System.Drawing;

/// <author> Ariadne Petroulakis </author>
/// <version>November 2024</version>
namespace Ciphers;

/// <summary>
/// This class represents or creates a Hill cipher of a given string of text. 
/// </summary>
public static class HillCipher
{
    /// <summary>
    /// A variable to hold all the letters in the alphabet with their indexes. 
    /// </summary>
    private static char[] alphabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                  'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    /// <summary>
    /// Encrypts a message using the hill cipher. 
    /// </summary>
    /// <param name="key"> The key with which we intend to encrypt the message.
    /// Must be an invertible modulo 26 matrix</param>
    /// <param name="message"> The message to be encrypted. Must contain only letters</param>
    /// <returns> The encrypted message </returns>
    /// <exception cref="ArgumentException"> If the key or message is invalid </exception>
    public static string Encrypt(double[][] key, string message)
    {
        // 1) Check if key and message are valid. 
        if (!(CheckKeyValidity(key, out string? err) && CheckMessageValidity(message, out err)))
            throw new ArgumentException(err);

        // 2) Set all letters in the string to a jagged array of their position in the alphabet broken into sub-vectors of length n (key.Length)
        double[][] vectorCollection = BuildNumMatrix(message, key.Length);

        // 3) Multiply the key into each column 
        vectorCollection = MultiplyIntoCollection(key, vectorCollection);

        // 4) Translate the columns into letters and concatenate together the contents of each row and THEN each column
        return CipherToString(vectorCollection);
    }

    /// <summary>
    /// Assesses whether or not the key matrix used is invertible modulo 26. For this
    /// version, the key MUST be a 2x2 matrix.
    /// </summary>
    /// <param name="key"> The matrix used as a key for the encryption </param>
    /// <param name="err"> The error message to be displayed</param>
    /// <returns> True if the matrix is invertible modulo 26, false otherwise </returns>
    private static bool CheckKeyValidity(double[][] key, out string? err)
    {
        // Check if it's square: a non-square matrix cannot be invertible
        if (!Is2x2(key))
        {
            err = "Key matrix is not a 2x2 invertible matrix.";
            return false;
        }

        // Set each entry of the key to a modulo 26 version of itself
        key = Mod26(key);

        // A 2x2 invertible matrix has a set formula for when it is and isn't invertible
        double det = CalcDeterminant(key);
        if ((det == 0 || det%2==0 || det%13==0))
        {
            err = "This 2x2 matrix has a determinant of 0 and is not invertible.";
            return false;
        }
        err = null;
        return true;
    }

    /// <summary>
    /// Helper method that calculates the determinant of a 2x2 matrix.
    /// </summary>
    /// <param name="matrix"> a 2x2 square matrix</param>
    /// <exception cref="ArgumentException"> if matrix is not a 2x2 matrix</exception>
    /// <returns></returns>
    private static double CalcDeterminant(double[][] matrix)
    {
        if (!Is2x2(matrix))
            throw new ArgumentException("Matrix must be 2x2!");
        return (matrix[0][0] * matrix[1][1]) - (matrix[0][1] * matrix[1][0])%26;
    }

    /// <summary>
    /// Helper that checks if a matrix is a 2x2 matrix
    /// </summary>
    /// <param name="arr"> the matrix to check </param>
    /// <returns>true if it is, false if it is not</returns>
    private static bool Is2x2(double[][] arr)
    {
        return arr.Length == arr[0].Length && arr.Length == 2;
    }

    /// <summary>
    /// Checks that the message consists of only letters. 
    /// </summary>
    /// <param name="message"> The message whose validity is to be assessed </param>
    /// <param name="err"> The error message to be returned if things go wrong; else, null </param>
    /// <returns> True if the message is valid and false if it is not</returns>
    private static bool CheckMessageValidity(string message, out string? err)
    {
        err = null;
        foreach (char letter in message)
        {
            if (!char.IsLetter(letter))
            {
                err = "Not all characters are letters.";
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Creates a modulo 26 version of a jagged 2D array.
    /// </summary>
    /// <param name="arr"> A jagged array </param>
    /// <returns> A modified version of the original jagged array where each entry becomes itself mod 26. </returns>
    private static double[][] Mod26(double[][] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = (arr[i][j] % 26 + 26) % 26; //need this math so negatives don't break
            }
        }
        return arr;
    }

    /// <summary>
    /// Helper method to set up a jagged array for easy multiplication in the next step. Each column is a vector that must
    /// be multiplied later with the key. There are n rows (where n = m = key matrix length/width) and columns equal 
    /// to half the number of letters in the message, rounded up to the closest even number.
    /// </summary>
    /// <param name="message"> The message to turn into an array of coded numbers </param>
    /// <param name="numRows"> The number of rows/cols in the key. </param>
    /// <returns> The jagged array </returns>
    private static double[][] BuildNumMatrix(string message, int numRows)
    {
        if (message.Length % 2 != 0)
            message += "x";

        char[] toEncode = message.ToLower().ToCharArray();

        int numCols = toEncode.Length / 2;
        double[][] numArr = new double[numRows][];

        for (int i = 0; i < numRows; i++)
        {
            numArr[i] = new double[numCols]; // Initialize each row of the jagged array
        }

        int index = 0; // Keeps track of the toEncode index
        for (int col = 0; col < numCols; col++) // Fill out each column
        {
            for (int row = 0; row < numRows; row++) // Only after you fill out every row in that column
            {
                numArr[row][col] = Array.IndexOf(alphabet, toEncode[index]);
                index++;
            }
        }

        return numArr;
    }

    /// <summary>
    /// Helper method to multiply every key with each column of the vector collection
    /// </summary>
    /// <param name="key"> the key matrix</param>
    /// <param name="collection"> a collection of vectors </param>
    /// <returns> the multiplied collection </returns>
    private static double[][] MultiplyIntoCollection(double[][] key, double[][] collection)
    {
        // For each column of the collection 
        for (int col = 0; col < collection[0].Length; col++)
        {
            // Create an array to hold the column vector
            double[] columnVector = new double[collection.Length];

            // Extract the column at index 'col'
            for (int row = 0; row < collection.Length; row++)
            {
                columnVector[row] = collection[row][col]; // Accessing the column element from each row
            }

            // Perform the matrix multiplication
            double[] result = MatrixTimesVector(key, columnVector);

            // Store the result back into the collection at the appropriate column
            for (int row = 0; row < collection.Length; row++)
            {
                // Store each value of the result in the appropriate column index
                collection[row][col] = result[row];
            }
        }

        return collection;
    }

    /// <summary>
    /// Multiply an array by a vector, given they can be multiplied. 
    /// </summary>
    /// <param name="left"> the left side fo matrix multiplication, a square with columns = to the number of
    /// rows in the vector. </param>
    /// <param name="right"> a vector, with rows = to the number of columns in the left matrix </param>
    /// <returns></returns>
    private static double[] MatrixTimesVector(double[][] left, double[] right)
    {
        double[] product = new double[left.Length];
        for(int row = 0; row < left.Length; row++)
        {
            double dot = 0;
            for(int col = 0; col < left[row].Length; col++)
            {
                dot += left[row][col] * right[col];
            }
            product[row] = dot;
        }
        return product;
    }

    /// <summary>
    /// Helper method to translate modulo 26 column vectors to an encrypted string.
    /// </summary>
    /// <param name="vectorCollection"> a collection of column vectors of integers organized in a 2D array.
    /// Entries must be in mod 26 form </param>
    /// <returns></returns>
    private static string CipherToString(double[][] vectorCollection)
    {
        vectorCollection = Mod26(vectorCollection);

        string finalString = string.Empty;
        for (int col = 0; col < vectorCollection[0].Length; col++)
        {
            for(int row = 0; row < vectorCollection.Length; row++)
            {
                finalString += alphabet[(int)Math.Round(vectorCollection[row][col]) %26];
            }
        }
        return finalString;
    }

    /// <param name="key"> The key used to create the encryption</param>
    /// <param name="message"> The encrypted message</param>
    /// <returns> The decrypted message </returns>
    /// <exception cref="ArgumentException"> if the key is not invertible</exception>
    public static string Decrypt(double[][] key, string message)
    {
        string decrypted = string.Empty; //set up a string to return

        //ensure that the key and the message are both valid
        //(i.e. the message consists of only letters and the key is invertible)
        if (!(CheckKeyValidity(key, out string? err) && CheckMessageValidity(message, out err)))
            throw new ArgumentException(err);
        key = Mod26(key);
        //1) find the inverse of the key, modulo 26; 
        double[][] inverse = Mod26(Invert2x2(key));

        //2) revert message back into array
        double[][] vectorCollection = BuildNumMatrix(message, key.Length);

        //3) multiply the inverse into that collection
        vectorCollection = Mod26(MultiplyIntoCollection(inverse, vectorCollection));

        //4) rebuild the string
        decrypted = CipherToString(vectorCollection);
        
        return decrypted;
    }

    /// <summary>
    /// Takes the inverse of a 2x2 matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns>the inverse of the given 2x2 matrix.</returns>
    private static double[][] Invert2x2(double[][] matrix)
{
    double determinant = CalcDeterminant(matrix);
    // Get the modular inverse of the determinant
    int invDet = ModInverse((int)determinant, 26);

    double[][] inverse = new double[matrix.Length][];
    for (int i = 0; i < matrix.Length; i++)
        inverse[i] = new double[matrix.Length];

    // Fill the inverse matrix according to the formula
    inverse[0][0] = (matrix[1][1] * invDet) % 26;
    inverse[0][1] = (-matrix[0][1] * invDet) % 26;
    inverse[1][0] = (-matrix[1][0] * invDet) % 26;
    inverse[1][1] = (matrix[0][0] * invDet) % 26;

    // Adjust for negative values
    for (int i = 0; i < inverse.Length; i++)
    {
        for (int j = 0; j < inverse[i].Length; j++)
        {
            if (inverse[i][j] < 0)
                inverse[i][j] += 26;
        }
    }

    return inverse;
}

/// <summary>
/// Calculates the modular inverse
/// </summary>
/// <param name="a">the dividend</param>
/// <param name="m">the divisor</param>
/// <returns>the remainder as a moodular inverse</returns>
    private static int ModInverse(int a, int m)
    {
        a = a % m;
        for (int x = 1; x < m; x++)
            if ((a * x) % m == 1)
                return x;
        return a%m;
    }

}
