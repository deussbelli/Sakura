using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Mathematical_cryptology_AffineCipher
{
    public class EncryptionService_fifth
    {
        private readonly int nEnglish = 27;
        private readonly int nUkrainian = 34;
        private int n;
        private string alphabet;
        private string specialChar = " ";

        public string EncryptText(string plainText, int[,] A, int[] S, int k)
        {
            SetLanguageParameters(true); // Default to English

            plainText = CleanText(plainText.ToUpper(), true);

            StringBuilder encryptedText = new StringBuilder();
            int[] plainVector = new int[k];

            for (int i = 0; i < plainText.Length; i += k)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i + j < plainText.Length)
                    {
                        char currentChar = plainText[i + j];
                        int index = alphabet.IndexOf(currentChar);
                        plainVector[j] = index == -1 ? n - 1 : index;
                    }
                    else
                    {
                        plainVector[j] = 0;
                    }
                }

                int[] cipherVector = new int[k];
                for (int row = 0; row < k; row++)
                {
                    cipherVector[row] = 0;
                    for (int col = 0; col < k; col++)
                    {
                        cipherVector[row] = (cipherVector[row] + A[row, col] * plainVector[col]) % n;
                    }
                    cipherVector[row] = (cipherVector[row] + S[row]) % n;
                }

                for (int j = 0; j < k; j++)
                {
                    if (cipherVector[j] == n - 1)
                    {
                        encryptedText.Append(specialChar);
                    }
                    else
                    {
                        encryptedText.Append(alphabet[cipherVector[j]]);
                    }
                }
            }

            return encryptedText.ToString();
        }

        public string DecryptText(string encryptedText, int[,] A, int[] S, int k)
        {
            SetLanguageParameters(true); // Default to English

            encryptedText = CleanText(encryptedText.ToUpper(), true);

            StringBuilder decryptedText = new StringBuilder();
            int[] cipherVector = new int[k];
            int[,] AInverse = InverseMatrix(A, k, n);

            for (int i = 0; i < encryptedText.Length; i += k)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i + j < encryptedText.Length)
                    {
                        char currentChar = encryptedText[i + j];
                        int index = alphabet.IndexOf(currentChar);
                        cipherVector[j] = index == -1 ? n - 1 : index;
                    }
                    else
                    {
                        cipherVector[j] = 0;
                    }
                }

                int[] plainVector = new int[k];
                for (int row = 0; row < k; row++)
                {
                    plainVector[row] = 0;
                    for (int col = 0; col < k; col++)
                    {
                        plainVector[row] = (plainVector[row] + AInverse[row, col] * ((cipherVector[col] - S[col] + n) % n)) % n;
                        if (plainVector[row] < 0) plainVector[row] += n;
                    }
                }

                for (int j = 0; j < k; j++)
                {
                    if (plainVector[j] == n - 1)
                    {
                        decryptedText.Append(specialChar);
                    }
                    else
                    {
                        decryptedText.Append(alphabet[plainVector[j]]);
                    }
                }
            }

            return decryptedText.ToString();
        }

        public void SetLanguageParameters(bool isEnglish)
        {
            if (isEnglish)
            {
                n = nEnglish;
                alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            }
            else
            {
                n = nUkrainian;
                alphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ ";
            }
        }

        public string CleanText(string input, bool isEnglish)
        {
            string pattern = isEnglish ? @"[^A-Z ]" : @"[^А-ЯІЇҐЄ ]";
            return Regex.Replace(input, pattern, "");
        }

        public int[,] InverseMatrix(int[,] A, int k, int n)
        {
            int[,] adj = new int[k, k];
            int[,] inv = new int[k, k];
            int det = (int)Determinant(A, k);
            int detInv = ModInverse(det, n);

            if (detInv == -1) throw new Exception("Inverse doesn't exist");

            Adjoint(A, adj);

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    inv[i, j] = adj[i, j] * detInv % n;
                    if (inv[i, j] < 0) inv[i, j] += n;
                }
            }

            return inv;
        }

        public int ModInverse(int a, int n)
        {
            int t = 0, newt = 1;
            int r = n, newr = a;

            while (newr != 0)
            {
                int quotient = r / newr;
                (t, newt) = (newt, t - quotient * newt);
                (r, newr) = (newr, r - quotient * newr);
            }

            if (r > 1) return -1;
            if (t < 0) t = t + n;
            return t;
        }

        public void Adjoint(int[,] A, int[,] adj)
        {
            int sign = 1;
            int[,] temp = new int[A.GetLength(0), A.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    GetCofactor(A, temp, i, j, A.GetLength(0));

                    sign = ((i + j) % 2 == 0) ? 1 : -1;

                    adj[j, i] = (sign * (int)Determinant(temp, A.GetLength(0) - 1)) % n;
                    if (adj[j, i] < 0) adj[j, i] += n;
                }
            }
        }

        public BigInteger Determinant(int[,] matrix, int n)
        {
            if (n == 1)
                return matrix[0, 0];

            BigInteger determinant = 0;
            int[,] temp = new int[n, n];
            int sign = 1;

            for (int f = 0; f < n; f++)
            {
                GetCofactor(matrix, temp, 0, f, n);
                determinant += sign * matrix[0, f] * Determinant(temp, n - 1);
                sign = -sign;
            }

            return determinant;
        }

        public void GetCofactor(int[,] A, int[,] temp, int p, int q, int n)
        {
            int i = 0, j = 0;

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    if (row != p && col != q)
                    {
                        temp[i, j++] = A[row, col];

                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }

        public int[,] ParseMatrix(string matrixString, int k)
        {
            int[,] matrix = new int[k, k];
            string[] rows = matrixString.Split(';');
            for (int i = 0; i < k; i++)
            {
                string[] values = rows[i].Split(',');
                for (int j = 0; j < k; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }
            return matrix;
        }

        public int[] ParseVector(string vectorString, int k)
        {
            int[] vector = new int[k];
            string[] values = vectorString.Split(',');
            for (int i = 0; i < k; i++)
            {
                vector[i] = int.Parse(values[i]);
            }
            return vector;
        }

        public int[,] GenerateKeyMatrix(int k)
        {
            int[,] matrix = new int[k, k];
            Random random = new Random();

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    matrix[i, j] = random.Next(1, n);
                }
            }

            while (Determinant(matrix, k) == 0 || !HasFullRank(matrix))
            {
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        matrix[i, j] = random.Next(1, n);
                    }
                }
            }

            return matrix;
        }

        public int[] GenerateKeyVector(int k)
        {
            int[] vector = new int[k];
            Random random = new Random();

            for (int i = 0; i < k; i++)
            {
                vector[i] = random.Next(1, n);
            }

            return vector;
        }

        public bool HasFullRank(int[,] matrix)
        {
            int rank = GaussianElimination(matrix);
            return rank == matrix.GetLength(0);
        }

        public int GaussianElimination(int[,] matrix)
        {
            int[,] temp = new int[matrix.GetLength(0), matrix.GetLength(1)];
            Array.Copy(matrix, temp, matrix.Length);

            int rowCount = temp.GetLength(0);
            int columnCount = temp.GetLength(1);

            int rank = columnCount;

            for (int row = 0; row < rowCount; row++)
            {
                for (int i = row + 1; i < rowCount; i++)
                {
                    int factor = temp[i, row] / temp[row, row];
                    for (int j = row; j < columnCount; j++)
                    {
                        temp[i, j] -= factor * temp[row, j];
                    }
                }

                if (temp[row, row] == 0)
                {
                    rank--;
                }
            }

            return rank;
        }
    }

}

