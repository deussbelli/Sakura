using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Media;
using System.Windows.Media.Animation;
using System.Timers;
using Mathematical_cryptology_RailFenceCipher;
using Mathematical_cryptology_CaesarCipher;
using static System.Collections.Specialized.BitVector32;
using System.Windows.Controls.Primitives;
using Mathematical_cryptology_VigenereCipher;
using Mathematical_cryptology_AffineCipher;

namespace Mathematical_cryptology_CardanoGrilleCipher
{
    public partial class CardanoGrilleCipher : Window
    {
        private readonly List<(int, int)> selectedCoordinates = new List<(int, int)>();
        private readonly List<char[,]> matrices = new List<char[,]>();
        private string action;
        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e) => mediaElement.Position = TimeSpan.Zero;

        public CardanoGrilleCipher()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5000) };
            timer.Tick += Timer_Tick;
            timer.Start();
            textEntry.TextChanged += TextBox_TextChanged;
            keyLengthEntry.TextChanged += TextBox_TextChanged;
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
                    Show();
                    break;
                case "Шифр Віженера [ Vigenère cipher ]":
                    new VigenereCipher().Show(); 
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
                if (textBox.Name == "textEntry" && chkReadText.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "textEntry" && chkReadFile.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "keyLengthEntry")
                    textBox.Text = "Key";
                else if (textBox.Name == "recommendedKeyLengthTextBox")
                    textBox.Text = "Recommended key";
                else if (textBox.Name == "keyEntry")
                    textBox.Text = "Entered key";
            }
            textBox.Foreground = Brushes.White;
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Contains("Enter"))
            {
                if (textBox.Name == "textEntry" && chkReadText.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "textEntry" && chkReadFile.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "keyLengthEntry")
                    textBox.Text = "Key";
                else if (textBox.Name == "recommendedKeyLengthTextBox")
                    textBox.Text = "Recommended key";
                else if (textBox.Name == "keyEntry")
                    textBox.Text = "Entered key";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox != null && (textBox.Text == "Enter text" || textBox.Text == "Key" || textBox.Text == "Recommended key" || textBox.Text == "Entered key" || string.IsNullOrEmpty(textBox.Text)))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private int CalculateRecommendedKeyLength(int textLength)
        {
            return (int)Math.Ceiling(Math.Sqrt(textLength));
        }

        private void textEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string text = textEntry.Text;
                int recommendedKeyLength = CalculateRecommendedKeyLength(text.Length);
                recommendedKeyLengthTextBox.Text = $"Key length: {recommendedKeyLength}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(keyLengthEntry.Text, out var keyLength))
                {
                    if (ValidateKey(keyLength))
                    {
                        GenerateMatrixButtons(keyLength);
                    }
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid key length.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ToggleButton selectedButton;

        private void GenerateMatrixButtons(int keyLength)
        {
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < keyLength; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int row = 0; row < keyLength; row++)
            {
                for (int col = 0; col < keyLength; col++)
                {
                    int currentRow = row;
                    int currentCol = col;
                    ToggleButton button = new ToggleButton { Content = $"({row},{col})", Style = (Style)FindResource("ToggleButtonStyle") };
                    button.Click += (s, ev) => SelectCoordinate(currentRow, currentCol);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    mainGrid.Children.Add(button);
                }
            }
        }

        private void SelectCoordinate(int row, int col)
        {
            selectedButton = (ToggleButton)mainGrid.Children.Cast<UIElement>()
                                            .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

            HashSet<(int, int)> blockedCoordinates = new HashSet<(int, int)>();
            LockSelectedAndOverlappingButtons(row, col, true, blockedCoordinates);

            selectedCoordinates.Add((row, col));
            UpdateSelectedKeyText();
        }

        private void LockSelectedAndOverlappingButtons(int row, int col, bool lockButtons, HashSet<(int, int)> blockedCoordinates)
        {
            LockButton(row, col, lockButtons, blockedCoordinates);

            for (int i = 0; i < 3; i++)
            {
                (row, col) = RotateCoordinates(row, col, mainGrid.RowDefinitions.Count);
                LockButton(row, col, lockButtons, blockedCoordinates);
            }
        }

        private void LockButton(int row, int col, bool lockButtons, HashSet<(int, int)> blockedCoordinates)
        {
            var button = (ToggleButton)mainGrid.Children.Cast<UIElement>()
                                .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
            if (button != null)
            {
                if (button != selectedButton)
                {
                    button.IsEnabled = !lockButtons;

                    if (lockButtons)
                    {
                        button.Style = (Style)FindResource("ToggleButtonStyle2");
                    }
                    else
                    {
                        button.Style = (Style)FindResource("ToggleButtonStyle");
                    }

                    if (lockButtons)
                    {
                        blockedCoordinates.Add((row, col));
                    }
                }
            }
        }

        private (int, int) RotateCoordinates(int row, int col, int keyLength)
        {
            int newRow = col;
            int newCol = keyLength - 1 - row;
            return (newRow, newCol);
        }

        private void UpdateSelectedKeyText()
        {
            var coordinatesList = new List<List<int>>();
            coordinatesList.AddRange(selectedCoordinates.ConvertAll(coordinate => new List<int> { coordinate.Item1, coordinate.Item2 }));
            keyEntry.Text = SerializeCoordinatesList(coordinatesList);
        }

        private string SerializeCoordinatesList(List<List<int>> coordinatesList)
        {
            var stringBuilder = new StringBuilder { Length = 0 };
            stringBuilder.Append("[");
            foreach (var row in coordinatesList)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(string.Join(",", row));
                stringBuilder.Append("],");
            }
            if (coordinatesList.Count > 0)
                stringBuilder.Length -= 1;
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        private bool ValidateKeyMatrix(List<List<int>> coordinatesList, int keyLength)
        {
            var previousCoordinates = new HashSet<(int, int)>();
            var rotatedCoordinates = new HashSet<(int, int)>();
            var wrongCoordinates = new List<(int, int)>();

            foreach (var coordinates in coordinatesList)
            {
                int row = coordinates[0];
                int col = coordinates[1];

                if (row < 0 || row >= keyLength || col < 0 || col >= keyLength)
                {
                    wrongCoordinates.Add((row, col));
                    continue;
                }

                if (!previousCoordinates.Add((row, col)))
                {
                    wrongCoordinates.Add((row, col));
                    continue;
                }

                if (!rotatedCoordinates.Add((row, col)))
                {
                    wrongCoordinates.Add((row, col));
                    continue;
                }

                for (int i = 0; i < 3; i++)
                {
                    int tempRow = row;
                    row = col;
                    col = keyLength - 1 - tempRow;

                    if (!rotatedCoordinates.Add((row, col)))
                    {
                        wrongCoordinates.Add((row, col));
                        break;
                    }
                }
            }

            if (wrongCoordinates.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine("Invalid coordinates found:");

                foreach (var (wrongRow, wrongCol) in wrongCoordinates)
                {
                    message.AppendLine($"Coordinate: ({wrongRow},{wrongCol})");
                    message.AppendLine("Collides with:");
                    for (int i = 0; i < 4; i++)
                    {
                        int tempRow = wrongRow;
                        int tempCol = wrongCol;
                        int rotatedRow = tempCol;
                        int rotatedCol = keyLength - 1 - tempRow;
                        message.AppendLine($"  - Rotation {i * 90}°: ({rotatedRow}, {rotatedCol})");
                    }
                    message.AppendLine();
                }

                MessageBox.Show(message.ToString(), "Invalid Coordinates", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return wrongCoordinates.Count == 0;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Name == "chkReadText")
            {
                textEntry.Text = "Enter text";
                textEntry.IsEnabled = true;
                keyEntry.Text = "Entered key";
                keyEntry.IsEnabled = true;
            }
            else if (chkBox.Name == "chkReadFile")
            {
                textEntry.Text = "Paste file link";
                textEntry.IsEnabled = true;
                keyEntry.Text = "Entered key";
                keyEntry.IsEnabled = true;
            }
        }

        private async void EncryptDecrypt_Click(object sender, RoutedEventArgs e)
        {
            resultEntry.Text = string.Empty;

            try
            {
                if (chkReadText.IsChecked == true || chkReadFile.IsChecked == true)
                {
                    if (chkEncrypt.IsChecked == true || chkDecrypt.IsChecked == true)
                    {
                        action = chkEncrypt.IsChecked == true ? "Encrypt" : "Decrypt";

                        string text = chkReadText.IsChecked == true ? textEntry.Text : textEntry.Text;
                        string key = keyEntry.Text;

                        List<List<int>> keyMatrix = ParseKey(key);
                        if (keyMatrix.Count == 0)
                            throw new FormatException("Key matrix is empty.");

                        int keyLength = keyMatrix.Count;

                        if (!ValidateKeyMatrix(keyMatrix, keyLength))
                            return;

                        string resultText;
                        TimeSpan elapsedTime;

                        if (chkReadFile.IsChecked == true)
                        {
                            var path = textEntry.Text;
                            var (fileResultText, fileElapsedTime) = await Task.Run(() => action == "Encrypt" ?
                                CardanoEncryptFileAsync(path, keyMatrix) :
                                CardanoDecryptFileAsync(path, keyMatrix));

                            resultText = fileResultText;
                            elapsedTime = fileElapsedTime;

                            resultEntry.Text = $"{action}ion time: {elapsedTime.TotalMilliseconds} milliseconds\n\n";
                            resultEntry.Text += $"Result:\n{resultText}\n";
                        }
                        else
                        {
                            Stopwatch textStopwatch = Stopwatch.StartNew();
                            var (textResultText, textElapsedTime) = await Task.Run(() => action == "Encrypt" ?
                                CardanoEncrypt(text, keyMatrix) :
                                CardanoDecrypt(text, keyMatrix));
                            textStopwatch.Stop();

                            resultText = textResultText;
                            elapsedTime = textStopwatch.Elapsed;

                            resultEntry.Text = $"{action}ion time: {elapsedTime.TotalMilliseconds} milliseconds\n\n";
                            resultEntry.Text += $"Result:\n{resultText}\n";

                            var matricesText = new StringBuilder();
                            for (int i = 0; i < matrices.Count; i++)
                            {
                                matricesText.Append($"\nMatrix at {i * 90} degrees:\n");
                                matricesText.Append(PrintMatrix(matrices[i]));
                            }
                            resultEntry.Text += matricesText;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an encryption or decryption method.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select one of the reading options: Read Text or Read File.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<(string, TimeSpan)> CardanoEncryptFileAsync(string path, List<List<int>> keyMatrix)
        {
            string outputPath = "E:\\C#\\Mathematical cryptology\\cardano_output_encrypt.txt";
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

        private async Task<(string, TimeSpan)> CardanoDecryptFileAsync(string path, List<List<int>> keyMatrix)
        {
            string outputPath = "E:\\C#\\Mathematical cryptology\\cardano_output_decrypt.txt";
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


        private List<List<int>> ParseKey(string key)
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

        private (string, TimeSpan) CardanoEncrypt(string text, List<List<int>> keyMatrix)
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
                matrices.Add((char[,])matrix.Clone());
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

        private (string, TimeSpan) CardanoDecrypt(string text, List<List<int>> keyMatrix)
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

        private string PrintMatrix(char[,] matrix)
        {
            StringBuilder result = new StringBuilder();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result.Append(matrix[i, j]);
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        private void RotateCardanoGrilleCounterClockwise(bool[,] grille)
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

        private void RotateCardanoGrilleClockwise(bool[,] grille)
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


        private void keyLengthEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void recommendedKeyLengthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void keyEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private bool ValidateKey(int keyLength)
        {
            if (keyLength % 2 != 0)
            {
                MessageBox.Show("Key length must be an even number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }



        private Random random = new Random();

        private void RandomKey_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(keyLengthEntry.Text, out int keyLength))
            {
                List<(int, int)> randomCoordinates = GenerateRandomCoordinates(keyLength);

                foreach (var coord in randomCoordinates)
                {
                    SelectCoordinate(coord.Item1, coord.Item2);
                    
                    ChangeButtonStyle(coord.Item1, coord.Item2, "ToggleButtonStyle");
                }
            }
        }

        private void ChangeButtonStyle(int row, int col, string styleName)
        {
            var button = (ToggleButton)mainGrid.Children.Cast<UIElement>()
                                .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
            if (button != null)
            {
                button.Style = (Style)FindResource(styleName);
            }
        }

        private List<(int, int)> GenerateRandomCoordinates(int keyLength)
        {
            List<(int, int)> allCoordinates = new List<(int, int)>();

            for (int row = 0; row < keyLength; row++)
            {
                for (int col = 0; col < keyLength; col++)
                {
                    allCoordinates.Add((row, col));
                }
            }

            Shuffle(allCoordinates);

            List<(int, int)> coordinates = new List<(int, int)>();
            HashSet<(int, int)> blockedCoordinates = new HashSet<(int, int)>();

            foreach (var coord in allCoordinates)
            {
                int row = coord.Item1;
                int col = coord.Item2;
                if (!blockedCoordinates.Contains(coord))
                {
                    coordinates.Add(coord);
                    blockedCoordinates.UnionWith(GetBlockedCoordinates(row, col, keyLength));
                }
            }

            return coordinates;
        }
        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        private int GenerateRandomColumn(int keyLength, HashSet<(int, int)> blockedCoordinates)
        {
            int col;
            do
            {
                col = random.Next(0, keyLength);
            }
            while (blockedCoordinates.Contains((col, col))); 
            return col;
        }

        private HashSet<(int, int)> GetBlockedCoordinates(int row, int col, int keyLength)
        {
            HashSet<(int, int)> blockedCoordinates = new HashSet<(int, int)>();

            LockSelectedAndOverlappingButtons(row, col, true, blockedCoordinates);

            int originalRow = row;
            int originalCol = col;

            for (int i = 0; i < 3; i++)
            {
                (row, col) = RotateCoordinates(row, col, keyLength);
                LockButton(row, col, true, blockedCoordinates);
            }

            LockButton(originalRow, col, true, blockedCoordinates);
            LockButton(row, originalCol, true, blockedCoordinates);

            return blockedCoordinates;
        }


        private void ResetKey_Click(object sender, RoutedEventArgs e)
        {
            keyEntry.Text = string.Empty;
            selectedCoordinates.Clear();
            UncheckToggleButtons();
            ResetToggleButtonStyle();
        }

        private void ResetToggleButtonStyle()
        {
            foreach (var child in mainGrid.Children)
            {
                var button = child as ToggleButton;
                if (button != null)
                {
                    button.IsEnabled = true;
                    button.Style = (Style)FindResource("ToggleButtonStyle");
                }
            }
        }

        private void UncheckToggleButtons()
        {
            foreach (var child in FindVisualChildren<ToggleButton>(this))
            {
                child.IsChecked = false;
            }
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}