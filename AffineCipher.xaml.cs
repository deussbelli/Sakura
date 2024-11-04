using Mathematical_cryptology_CaesarCipher;
using Mathematical_cryptology_RailFenceCipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Mathematical_cryptology_CardanoGrilleCipher;
using Mathematical_cryptology_VigenereCipher;

namespace Mathematical_cryptology_AffineCipher
{
    public partial class AffineCipher : Window
    {
        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e) => mediaElement.Position = TimeSpan.Zero;

        private static readonly int nEnglish = 27;
        private static readonly int nUkrainian = 34;
        private int n;
        private string alphabet;
        private string specialChar = " ";

        public AffineCipher()
        {
            InitializeComponent();       

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5000) };
            timer.Tick += Timer_Tick;
            timer.Start();
            PlainTextFileTextBox.TextChanged += TextBox_TextChanged;
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ListBoxItem)sender;
            switch (selectedItem.Content.ToString())
            {
                case "Шифр частоколу [ Rail Fence ]":
                    new RailFenceCipher().Show();
                    break;
                case "Шифр зсуву [ Caesar cipher ]":
                    new CaesarCipher().Show();
                    break;
                case "Шифр Кардано [ Cardan grille ]":
                    new CardanoGrilleCipher().Show();
                    break;
                case "Шифр Віженера [ Vigenère cipher ]":
                    Show();
                    break;
                case "Афінний шифр [ Affine cipher ]":
                    new AffineCipher().Show();
                    break;
            }
            Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Contains("Enter") || textBox.Text.Contains("Key"))
            {
                if (textBox.Name == "PlainTextFileTextBox" && EnterTextCheckBox.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "PlainTextFileTextBox" && ReadFromFileCheckBox.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "OrderTextBox")
                    textBox.Text = "Order (k)";
                else if (textBox.Name == "KeyVectorTextBox")
                    textBox.Text = "Key Vector (S)";
                else if (textBox.Name == "KeyMatrixTextBox")
                    textBox.Text = "Key Matrix (A)";
            }
            textBox.Foreground = Brushes.White;
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Contains("Enter"))
            {
                if (textBox.Name == "textInput" && EnterTextCheckBox.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "textInput" && ReadFromFileCheckBox.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "OrderTextBox")
                    textBox.Text = "Order (k)";
                else if (textBox.Name == "KeyVectorTextBox")
                    textBox.Text = "Key Vector (S)";
                else if (textBox.Name == "KeyMatrixTextBox")
                    textBox.Text = "Key Matrix (A)";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox != null && (textBox.Text == "Enter text" || textBox.Text == "Order (k)" || textBox.Text == "Key Vector (S)" || textBox.Text == "Key Matrix (A)" || string.IsNullOrEmpty(textBox.Text)))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Name == "textCheckbox")
            {
                PlainTextFileTextBox.Text = "Enter text";
                EnterTextCheckBox.IsEnabled = true;
            
            }
            else if (chkBox.Name == "fileCheckbox")
            {
                PlainTextFileTextBox.Text = "Paste file link";
                EnterTextCheckBox.IsEnabled = true;
            }
        }


        private void GenerateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            int k;
            if (!int.TryParse(OrderTextBox.Text, out k) || k <= 1)
            {
                MessageBox.Show("Please enter a valid order (k).\n(k) must be greater than 1.");
                return;
            }

            SetLanguageParameters();

            int[,] keyMatrix;
            int[] keyVector;
            BigInteger determinant;

            do
            {
                keyMatrix = GenerateKeyMatrix(k);
                determinant = Determinant(keyMatrix, k);
                if (determinant == 0 || ModInverse((int)determinant, n) == -1 || !HasFullRank(keyMatrix))
                {
                    MessageBoxResult result = MessageBox.Show("Generated matrix is invalid. Do you want to generate again?", "Invalid Matrix", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            } while (determinant == 0 || ModInverse((int)determinant, n) == -1 || !HasFullRank(keyMatrix));

            do
            {
                keyVector = GenerateKeyVector(k);
            } while (keyVector == null);

            KeyMatrixTextBox.Text = MatrixToString(keyMatrix);
            KeyVectorTextBox.Text = VectorToString(keyVector);
        }



        private void SelectKeyButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string keyFile = openFileDialog.FileName;
                string[] keyFileContent = File.ReadAllLines(keyFile);

                if (keyFileContent.Length >= 3)
                {
                    string order = keyFileContent[0];
                    string keyMatrix = keyFileContent[1];
                    string keyVector = keyFileContent[2];

                    if (int.TryParse(order, out int parsedOrder))
                    {
                        OrderTextBox.Text = parsedOrder.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Invalid order format in the key file.");
                        return;
                    }

                    if (IsValidMatrixFormat(keyMatrix))
                    {
                        KeyMatrixTextBox.Text = keyMatrix;
                    }
                    else
                    {
                        MessageBox.Show("Invalid matrix format in the key file.");
                        return;
                    }

                    if (IsValidVectorFormat(keyVector))
                    {
                        KeyVectorTextBox.Text = keyVector;
                    }
                    else
                    {
                        MessageBox.Show("Invalid vector format in the key file.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid key file format.");
                }
            }
        }

        private bool IsValidMatrixFormat(string matrix)
        {
            return matrix.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                         .All(s => int.TryParse(s, out int _));
        }

        private bool IsValidVectorFormat(string vector)
        {
            return vector.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                         .All(s => int.TryParse(s, out int _));
        }


        private int[,] GenerateKeyMatrix(int k)
        {
            Random random = new Random();
            int[,] matrix = new int[k, k];

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


        private bool HasFullRank(int[,] matrix)
        {
            int rank = GaussianElimination(matrix);

            return rank == matrix.GetLength(0);
        }

        private int GaussianElimination(int[,] matrix)
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)EncryptCheckBox.IsChecked || (bool)DecryptCheckBox.IsChecked)
            {
                if ((bool)EnterTextCheckBox.IsChecked)
                {
                    if (string.IsNullOrWhiteSpace(PlainTextFileTextBox.Text))
                    {
                        MessageBox.Show("Please enter the text to encrypt/decrypt.");
                        return;
                    }
                }
                else if ((bool)ReadFromFileCheckBox.IsChecked)
                {
                    if (string.IsNullOrWhiteSpace(PlainTextFileTextBox.Text))
                    {
                        MessageBox.Show("Please select a file to read the text from.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please select how you want to enter the text.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(OrderTextBox.Text))
                {
                    MessageBox.Show("Please enter the order (k).");
                    return;
                }

                if (string.IsNullOrWhiteSpace(KeyMatrixTextBox.Text))
                {
                    MessageBox.Show("Please generate or select a key matrix.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(KeyVectorTextBox.Text))
                {
                    MessageBox.Show("Please generate or select a key vector.");
                    return;
                }

                if ((bool)EncryptCheckBox.IsChecked)
                {
                    EncryptButton_Click(sender, e);
                }
                else if ((bool)DecryptCheckBox.IsChecked)
                {
                    DecryptButton_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Please select whether to encrypt or decrypt.");
            }
        }



        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {

            DateTime startTime = DateTime.Now;


            int k = int.Parse(OrderTextBox.Text);
            SetLanguageParameters();

            int[,] A = ParseMatrix(KeyMatrixTextBox.Text, k);
            int[] S = ParseVector(KeyVectorTextBox.Text, k);

            string plainText = "";
            string encryptedTextFile = "";

            if ((bool)ReadFromFileCheckBox.IsChecked)
            {
                string plainTextFile = PlainTextFileTextBox.Text;
                if (string.IsNullOrEmpty(plainTextFile))
                {
                    MessageBox.Show("Please select a plain text file.");
                    return;
                }
                plainText = File.ReadAllText(plainTextFile);
                encryptedTextFile = (bool)EnglishCheckBox.IsChecked ? @"E:\C#\Mathematical cryptology\affine_cipher_encrypt.txt" : @"E:\C#\Mathematical cryptology\affine_cipher_encrypt_ukr.txt";
            }
            else if ((bool)EnterTextCheckBox.IsChecked)
            {
                plainText = PlainTextFileTextBox.Text;
            }

            string cleanedText = CleanText(plainText.ToUpper());
            string encryptedText = Encrypt(cleanedText, A, S, k);

            DateTime endTime = DateTime.Now;
            TimeSpan encryptionTime = endTime - startTime;

            if ((bool)ReadFromFileCheckBox.IsChecked)
            {
                File.WriteAllText(encryptedTextFile, encryptedText);
                ResultTextBox.Text = "Encryption completed in " + encryptionTime.TotalMilliseconds + " milliseconds.\nResult:\n" + encryptedTextFile;
            }
            else if ((bool)EnterTextCheckBox.IsChecked)
            {
                ResultTextBox.Text = "Encryption completed in " + encryptionTime.TotalMilliseconds + " milliseconds.\nResult:\n" + encryptedText;
            }


        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.Now;

            int k = int.Parse(OrderTextBox.Text);
            SetLanguageParameters();

            int[,] A = ParseMatrix(KeyMatrixTextBox.Text, k);
            int[] S = ParseVector(KeyVectorTextBox.Text, k);

            string encryptedText = "";
            string decryptedTextFile = "";

            if ((bool)ReadFromFileCheckBox.IsChecked)
            {
                string encryptedTextFile = PlainTextFileTextBox.Text;
                if (string.IsNullOrEmpty(encryptedTextFile))
                {
                    MessageBox.Show("Please select an encrypted text file.");
                    return;
                }
                encryptedText = File.ReadAllText(encryptedTextFile);
                decryptedTextFile = (bool)EnglishCheckBox.IsChecked ? @"E:\C#\Mathematical cryptology\affine_cipher_decrypt.txt" : @"E:\C#\Mathematical cryptology\affine_cipher_decrypt_ukr.txt";
            }
            else if ((bool)EnterTextCheckBox.IsChecked)
            {
                encryptedText = PlainTextFileTextBox.Text;
            }

            string decryptedText = Decrypt(encryptedText.ToUpper(), A, S, k);

            DateTime endTime = DateTime.Now;
            TimeSpan decryptionTime = endTime - startTime;

            if ((bool)ReadFromFileCheckBox.IsChecked)
            {
                File.WriteAllText(decryptedTextFile, decryptedText);
                ResultTextBox.Text = "Decryption completed in " + decryptionTime.TotalMilliseconds + " milliseconds.\nResult saved in: " + decryptedTextFile;
            }
            else if ((bool)EnterTextCheckBox.IsChecked)
            {
                ResultTextBox.Text = "Decryption completed in " + decryptionTime.TotalMilliseconds + " milliseconds.\nResult:\n" + decryptedText;
            }
        }

        private void SetLanguageParameters()
        {
            if ((bool)EnglishCheckBox.IsChecked)
            {
                n = nEnglish;
                alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            }
            else
            {
                n = nUkrainian;
                alphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ ";
            }
            Console.WriteLine($"Selected language parameters: n = {n}, alphabet = {alphabet}");
        }

        private string CleanText(string input)
        {
            string pattern = (bool)EnglishCheckBox.IsChecked ? @"[^A-Z ]" : @"[^А-ЯІЇҐЄ ]";
            return Regex.Replace(input, pattern, "");
        }

        private string Encrypt(string plainText, int[,] A, int[] S, int k)
        {
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

        private string Decrypt(string encryptedText, int[,] A, int[] S, int k)
        {
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
                    }
                    if (plainVector[row] < 0) plainVector[row] += n;
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

        private int[,] InverseMatrix(int[,] A, int k, int n)
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

        private int ModInverse(int a, int n)
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

        private void Adjoint(int[,] A, int[,] adj)
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

        private BigInteger Determinant(int[,] matrix, int n)
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

        private void GetCofactor(int[,] A, int[,] temp, int p, int q, int n)
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

        private int[,] ParseMatrix(string matrixString, int k)
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

        private int[] ParseVector(string vectorString, int k)
        {
            int[] vector = new int[k];
            string[] values = vectorString.Split(',');
            for (int i = 0; i < k; i++)
            {
                vector[i] = int.Parse(values[i]);
            }
            return vector;
        }

        private int[] GenerateKeyVector(int k)
        {
            int[] vector = new int[k];
            Random random = new Random();

            for (int i = 0; i < k; i++)
            {
                vector[i] = random.Next(1, n);
            }

            return vector;
        }


        private string MatrixToString(int[,] matrix)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    builder.Append(matrix[i, j]);
                    if (j < matrix.GetLength(1) - 1)
                    {
                        builder.Append(",");
                    }
                }
                builder.Append(";");
            }

            return builder.ToString();
        }

        private string VectorToString(int[] vector)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < vector.Length; i++)
            {
                builder.Append(vector[i]);
                if (i < vector.Length - 1)
                {
                    builder.Append(",");
                }
            }

            return builder.ToString();
        }

        //private void BrowsePlainTextButton_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        PlainTextFileTextBox.Text = openFileDialog.FileName;
        //    }
        //}



        private void ReadFromFileCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PlainTextFileTextBox.IsEnabled = true;

            EnterTextCheckBox.IsChecked = false;
        }

        private void ReadFromFileCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(bool)EnterTextCheckBox.IsChecked)
            {
                PlainTextFileTextBox.IsEnabled = false;
            }
        }

        private void EnterTextCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PlainTextFileTextBox.IsEnabled = true;
            ReadFromFileCheckBox.IsChecked = false;
        }

        private void EnterTextCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(bool)ReadFromFileCheckBox.IsChecked)
            {
                PlainTextFileTextBox.IsEnabled = false;
            }
        }
    }




}