
using System.Reflection.Metadata.Ecma335;

///
///<author> Ariadne Petroulakis </author>
///<version>November 2024</version>
namespace HillCipher
{
    char[] alphabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                       'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
    /// <summary>
    /// This class represents or creates a Hill cipher of a given string of text. 
    /// </summary>
    public static class HillCipher
    {
        /// <summary>
        /// Encrypts a message using the hill cipher. 
        /// </summary>
        /// <param name="key"> the key with which we intend to encrypt the message.
        /// Must be an invertible modulo 26 matrix</param>
        /// <param name="message"> the message to be encrypted. Must contain only letters</param>
        /// <returns> the encrypted message </returns>
        /// <exception cref="ArgumentException"> if the key or message is invalid </exception>
        public static string Encrypt(int[][] key, string message) 
        {
            //1) check if key and message are valid. if message is not dividible by 2, add X to the end
            if (!(CheckKeyValidity(key, out string? err) && CheckMessageValidity(message, out err)))
                throw new ArgumentException(err);

            //ensure that the char array is dividible by 2 so the chunks work out. 
            if (message.Length % 2 != 0)
                message += "x";

            char[] c = message.ToLower().ToCharArray(); 

            //2) set all letters in the string to a 2D array of their position in the alphabet broken into sub-vectors of length n (key.length)
                // > each column must be of length n 
            //3) multiply the key into each column 
            //4) translate the columns into letters and concatenate together the contents of each row anf THEN each column
            int[] positions = new int[message.Length];
            throw new NotImplementedException();
        }

        /// <summary>
        /// Assesses whether or not the key matrix used is invertible modulo 26. For this
        /// version, the key MUST be a 2x2 matrix.
        /// </summary>
        /// <param name="key"> the matrix used as a key for the encryption </param>
        /// <param name="err"> the error message to be displayed</param>
        /// <returns> true if the matrix is invertible modulo 26, false otherwise </returns>
        private static bool CheckKeyValidity(int[][] key, out string? err)
        {
            //check if its square: a non-square matrix cannot be invertible
            if (key.Length != key[0].Length)
            {
                err = "Key matrix is not square and thus not invertible.";
                return false;
            }

            //if it's not 2x2 then the key is invalid. 
            if (key.Length != 2)
            {
                err = "The key must be a 2x2 matrix.";
                return false;
            }

            //set each entry of the key to a modulo 26 version of itself/
            key = Mod26(key);

            //a 2x2 invertible matrix has a set formula for when it is and isn't invertible
            if ((key[0][0] * key[1][1]) - (key[0][1] * key[1][0]) != 0)
            {
                err = "This 2x2 matrix as a determinant of 0 and is not invertible";
                return true;
            }
            err = null;
            return false;

        }

        /// <summary>
        /// Checks that the message consists of only letters. 
        /// </summary>
        /// <param name="message"> the message whose validity is to be assessed </param>
        /// <param name="err"> the error message to be returned if things go wrong; else, null </param>
        /// <returns> true if the message is valid and false if it is not</returns>
        private static bool CheckMessageValidity(string message, out string? err)
        {
            err = null;
            foreach (char letter in message)
            {
                if (char.IsLetter(letter) == false)
                {
                    err = "Not all characters are letters.";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a modulo 26 version of a matrix.
        /// </summary>
        /// <param name="arr"> a matrix </param>
        /// <returns> a modified version of the original matrix where each entry becomes itself mod 26. </returns>
        private static int[][] Mod26(int[][] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    arr[i][j] = arr[i][j] % 26;
                }
            }
            return arr;
        }

        /// <summary>
        /// Decrypts a message encrypted using the hill cipher. 
        /// </summary>
        /// <param name="key">the key used to create the encryption</param>
        /// <param name="message"> the encrypted message</param>
        /// <returns> the decrypted message </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string Decrypt(int[][] key, string message)
        {
            throw new NotImplementedException();
        }
    }
}
