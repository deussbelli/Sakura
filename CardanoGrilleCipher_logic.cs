using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace Mathematical_cryptology_CardanoGrilleCipher_test
{
    public class EncryptionService_third
    {
        public Random random = new Random();
        public List<(char[,], bool[,])> matrices = new List<(char[,], bool[,])>();

        public async Task<(string, TimeSpan)> EncryptFileAsync(string path, string key)
        {
            List<List<int>> keyMatrix = ParseKey(key);
            ValidateKeyMatrix(keyMatrix, keyMatrix.Count);

            var (outputPath, elapsedTime) = await CardanoEncryptFileAsync(path, keyMatrix);
            return (outputPath, elapsedTime);
        }

        public async Task<(string, TimeSpan)> DecryptFileAsync(string path, string key)
        {
            List<List<int>> keyMatrix = ParseKey(key);
            ValidateKeyMatrix(keyMatrix, keyMatrix.Count);

            var (outputPath, elapsedTime) = await CardanoDecryptFileAsync(path, keyMatrix);
            return (outputPath, elapsedTime);
        }

        public void ValidateKeyMatrix(List<List<int>> keyMatrix, int matrixSize)
        {
            if (keyMatrix == null || keyMatrix.Count != matrixSize)
                throw new ArgumentException("Invalid key matrix size.");

            foreach (var row in keyMatrix)
            {
                if (row.Count != 2)
                    throw new ArgumentException("Each key coordinate must have exactly two values (row and column).");
                if (row.Any(value => value < 0 || value >= matrixSize))
                    throw new ArgumentException("Key coordinates must be within the matrix bounds.");
            }
        }

        public async Task<(string, TimeSpan)> CardanoEncryptFileAsync(string path, List<List<int>> keyMatrix)
        {
            string outputPath = "cardano_output_encrypt.txt";
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (StreamWriter outputFile = new StreamWriter(outputPath))
                using (StreamReader reader = new StreamReader(path))
                {
                    int keyLength = keyMatrix.Count;
                    int blockSize = keyLength * keyLength;
                    char[] buffer = new char[blockSize];
                    StringBuilder encryptedTextBuilder = new StringBuilder();

                    while (reader.Peek() >= 0)
                    {
                        int charsRead = await reader.ReadBlockAsync(buffer, 0, blockSize);
                        string block = new string(buffer, 0, charsRead);
                        var (encryptedBlock, _) = CardanoEncrypt(block, keyMatrix);
                        encryptedTextBuilder.Append(encryptedBlock);
                    }

                    await outputFile.WriteAsync(encryptedTextBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            stopWatch.Stop();
            return (outputPath, stopWatch.Elapsed);
        }

        public async Task<(string, TimeSpan)> CardanoDecryptFileAsync(string path, List<List<int>> keyMatrix)
        {
            string outputPath = "cardano_output_decrypt.txt";
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (StreamWriter outputFile = new StreamWriter(outputPath))
                using (StreamReader reader = new StreamReader(path))
                {
                    int keyLength = keyMatrix.Count;
                    int blockSize = keyLength * keyLength;
                    char[] buffer = new char[blockSize];
                    StringBuilder decryptedTextBuilder = new StringBuilder();

                    while (reader.Peek() >= 0)
                    {
                        int charsRead = await reader.ReadBlockAsync(buffer, 0, blockSize);
                        string block = new string(buffer, 0, charsRead);
                        var (decryptedBlock, _) = CardanoDecrypt(block, keyMatrix);
                        decryptedTextBuilder.Append(decryptedBlock);
                    }

                    await outputFile.WriteAsync(decryptedTextBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            stopWatch.Stop();
            return (outputPath, stopWatch.Elapsed);
        }

        public List<List<int>> ParseKey(string key)
        {
            var keyMatrix = new List<List<int>>();
            if (string.IsNullOrWhiteSpace(key))
                throw new FormatException("Key is empty. Please enter a valid key.");

            key = key.Trim('[', ']');

            string[] coordinates = key.Split(new[] { "],[" }, StringSplitOptions.None);
            foreach (string coordinate in coordinates)
            {
                string trimmedCoordinate = coordinate.Trim('[', ']');
                string[] values = trimmedCoordinate.Split(',');
                var row = new List<int>();
                foreach (string value in values)
                {
                    if (int.TryParse(value, out var number))
                        row.Add(number);
                    else
                        throw new FormatException("Invalid key format. Please enter a valid key.");
                }
                keyMatrix.Add(row);
            }

            return keyMatrix;
        }

        public (string, TimeSpan) CardanoEncrypt(string text, List<List<int>> keyMatrix)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int matrixSize = (int)Math.Ceiling(Math.Sqrt(text.Length));
            char[,] matrix = new char[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    matrix[i, j] = '•';
                }
            }

            bool[,] cardanoGrille = new bool[matrixSize, matrixSize];

            foreach (var keyElem in keyMatrix)
            {
                int row = keyElem[0];
                int col = keyElem[1];
                if (row < matrixSize && col < matrixSize)
                {
                    cardanoGrille[row, col] = true;
                }
            }

            StringBuilder resultText = new StringBuilder();
            int count = 0;
            for (int rotate = 0; rotate < 4; rotate++)
            {
                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        if (cardanoGrille[i, j] && count < text.Length)
                            matrix[i, j] = text[count++];
                    }
                }
                matrices.Add((matrix.Clone() as char[,], cardanoGrille.Clone() as bool[,]));
                RotateCardanoGrilleClockwise(cardanoGrille);
            }

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    resultText.Append(matrix[i, j]);
                }
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            return (resultText.ToString(), elapsedTime);
        }

        public (string, TimeSpan) CardanoDecrypt(string text, List<List<int>> keyMatrix)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int matrixSize = (int)Math.Ceiling(Math.Sqrt(text.Length));
            char[,] matrix = new char[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    matrix[i, j] = '•';
                }
            }

            int count = 0;
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (count < text.Length)
                        matrix[i, j] = text[count++];
                }
            }

            bool[,] cardanoGrille = new bool[matrixSize, matrixSize];

            foreach (var keyElem in keyMatrix)
            {
                int row = keyElem[0];
                int col = keyElem[1];
                if (row < matrixSize && col < matrixSize)
                {
                    cardanoGrille[row, col] = true;
                }
            }

            StringBuilder resultText = new StringBuilder();
            for (int rotate = 0; rotate < 4; rotate++)
            {
                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        if (cardanoGrille[i, j])
                            resultText.Append(matrix[i, j]);
                    }
                }
                RotateCardanoGrilleClockwise(cardanoGrille);
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            return (resultText.ToString(), elapsedTime);
        }

        public void RotateCardanoGrilleCounterClockwise(bool[,] grille)
        {
            int size = grille.GetLength(0);
            bool[,] temp = new bool[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp[i, j] = grille[j, size - i - 1];
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grille[i, j] = temp[i, j];
                }
            }
        }

        public void RotateCardanoGrilleClockwise(bool[,] grille)
        {
            int size = grille.GetLength(0);
            bool[,] temp = new bool[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp[i, j] = grille[size - j - 1, i];
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grille[i, j] = temp[i, j];
                }
            }
        }
    }
}
