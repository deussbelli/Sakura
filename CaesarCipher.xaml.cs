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
using Mathematical_cryptology_CardanoGrilleCipher;
using Mathematical_cryptology_VigenereCipher;
using Mathematical_cryptology_AffineCipher;

namespace Mathematical_cryptology_CaesarCipher
{
    public partial class CaesarCipher : Window
    {
        private readonly string englishAlphabet = "abcdefghijklmnopqrstuvwxyz ";
        private readonly string ukrainianAlphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя ";

        private string selectedAlphabet = "English";

        public CaesarCipher()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Zero;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            this.Close();
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ListBoxItem)sender;

            switch (selectedItem.Content.ToString())
            {
                case "Шифр частоколу [ Rail Fence ]":
                    new RailFenceCipher().Show();
                    break;
                case "Шифр зсуву [ Caesar cipher ]":
                    Show();
                    break;
                case "Шифр Кардано [ Cardan grille ]":
                    new CardanoGrilleCipher().Show();
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

        private void TxtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter text or link" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter text or link";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtShift_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter shift value" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtShift_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter shift value";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtNumberOfBlocks_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter number of blocks" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtNumberOfBlocks_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter number of blocks";
            }
            textBox.Foreground = Brushes.White;
        }

        public bool ValidateFields()
        {
            if (!int.TryParse(txtShift.Text, out int shift) || shift < 0)
            {
                MessageBox.Show("Shift value must be a non-negative integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtNumberOfBlocks.Text, out int numberOfBlocks) || numberOfBlocks < 0)
            {
                MessageBox.Show("Number of blocks must be a non-negative integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkEncrypt.IsChecked.GetValueOrDefault(false) && !chkDecrypt.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select at least one method (Encrypt or Decrypt).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkEnglishAlphabet.IsChecked.GetValueOrDefault(false) && !chkUkrainianAlphabet.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select at least one language (English Alphabet or Ukrainian Alphabet).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkReadFromConsole.IsChecked.GetValueOrDefault(false) && !chkFilePath.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select either 'Read from the console' or 'File path'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (chkFilePath.IsChecked.GetValueOrDefault(false))
            {
                string filePath = txtInput.Text;
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void btnEnglishAlphabet_Click(object sender, RoutedEventArgs e)
        {
            selectedAlphabet = "English";
        }

        private void btnUkrainianAlphabet_Click(object sender, RoutedEventArgs e)
        {
            selectedAlphabet = "Ukrainian";
        }

        public string EncryptBlock(string block, int shift, string selectedAlphabet)
        {
            var alphabet = selectedAlphabet.Equals("English") ? englishAlphabet : ukrainianAlphabet;
            return TransformText(block, shift, alphabet);
        }

        public string DecryptBlock(string block, int shift, string selectedAlphabet)
        {
            return EncryptBlock(block, -shift, selectedAlphabet);
        }

        private string TransformText(string text, int shift, string alphabet)
        {
            var result = new StringBuilder();

            foreach (var c in text)
            {
                var index = alphabet.IndexOf(c);
                result.Append(index == -1 ? c : alphabet[(index + shift + alphabet.Length) % alphabet.Length]);
            }

            return result.ToString();
        }

        private string ProcessBlocks(string inputText, int shift, string selectedAlphabet, int blockSize, bool encrypt)
        {
            if (blockSize == 0)
                return encrypt ? EncryptBlock(inputText, shift, selectedAlphabet) : DecryptBlock(inputText, shift, selectedAlphabet);

            var blocks = new List<string>();
            for (int i = 0; i < inputText.Length; i += blockSize)
            {
                var length = Math.Min(blockSize, inputText.Length - i);
                blocks.Add(inputText.Substring(i, length));
            }

            var processedBlocks = new List<string>();

            foreach (var block in blocks)
            {
                string processedBlock = encrypt ? EncryptBlock(block, shift, selectedAlphabet) : DecryptBlock(block, shift, selectedAlphabet);
                processedBlocks.Add(processedBlock);
            }

            return string.Concat(processedBlocks);
        }

        private HashSet<string> LoadDictionary(string filePath)
        {
            HashSet<string> dictionary = new HashSet<string>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    dictionary.Add(line.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the dictionary: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return dictionary;
        }

        private HashSet<string> GetDictionary(string selectedAlphabet)
        {
            string filePath = selectedAlphabet == "English" ? @"E:\C#\Mathematical cryptology\words.txt" : @"E:\C#\Mathematical cryptology\words_ukranian.txt";
            return LoadDictionary(filePath);
        }

        private void Cryptanalyze(string ciphertext, string selectedAlphabet)
        {
            var dictionary = GetDictionary(selectedAlphabet);
            txtResult.Clear();

            txtResult.Text += "Performing frequency analysis...\n";
            string expectedShiftResult = Cryptanalyze_Frequency(ciphertext, selectedAlphabet);
            txtResult.Text += expectedShiftResult;


            txtResult.Text += "\n\nBruteforce:\n";
            string alphabet = selectedAlphabet == "English" ? englishAlphabet : ukrainianAlphabet;
            int alphabetLength = alphabet.Length;

            for (int shift = 0; shift < alphabetLength; shift++)
            {
                string decryptedText = DecryptBlock(ciphertext, shift, selectedAlphabet);
                string[] words = decryptedText.Split(new char[] { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    if (dictionary.Contains(word.ToLower()))
                    {
                        txtResult.Text += $"Shift: {shift}, Decrypted word: {word} -- found in the dictionary\n";
                    }
                    else
                    {
                        txtResult.Text += $"Shift: {shift}, Decrypted word: {word}\n";
                    }
                }
            }
        }

        private static readonly Dictionary<char, double> englishSymbolFrequency = new Dictionary<char, double>
        {
            {'a', 8.167}, {'b', 1.492}, {'c', 2.782}, {'d', 4.253}, {'e', 12.702},
            {'f', 2.228}, {'g', 2.015}, {'h', 6.094}, {'i', 6.966}, {'j', 0.153},
            {'k', 0.772}, {'l', 4.025}, {'m', 2.406}, {'n', 6.749}, {'o', 7.507},
            {'p', 1.929}, {'q', 0.095}, {'r', 5.987}, {'s', 6.327}, {'t', 9.056},
            {'u', 2.758}, {'v', 0.978}, {'w', 2.360}, {'x', 0.150}, {'y', 1.974},
            {'z', 0.074},
            {' ', 19.181},
        };

        private static readonly Dictionary<char, double> ukrainianSymbolFrequency = new Dictionary<char, double>
        {
            {'а', 7.2}, {'б', 1.7}, {'в', 5.2}, {'г', 1.6}, {'ґ', 1.6},
            {'д', 3.5}, {'е', 1.7}, {'є', 0.8}, {'ж', 0.9}, {'з', 2.3},
            {'и', 6.1}, {'і', 5.7}, {'ї', 0.6}, {'й', 0.8}, {'к', 3.5},
            {'л', 3.6}, {'м', 3.1}, {'н', 6.5}, {'о', 9.4}, {'п', 2.9},
            {'р', 4.7}, {'с', 4.1}, {'т', 5.5}, {'у', 4.0}, {'ф', 0.1},
            {'х', 1.2}, {'ц', 0.6}, {'ч', 1.8}, {'ш', 1.2}, {'щ', 0.1},
            {'ь', 2.9}, {'ю', 0.4}, {'я', 2.9},
            {' ', 17}
        };

        private static Dictionary<char, double> GetDictionary2(string selectedAlphabet)
        {
            return selectedAlphabet == "English" ? englishSymbolFrequency : ukrainianSymbolFrequency;
        }
        public string Cryptanalyze_Frequency(string message, string selectedAlphabet)
        {
            var dictionary = GetDictionary2(selectedAlphabet);

            string alphabet = selectedAlphabet == "English" ? englishAlphabet : ukrainianAlphabet;

            Dictionary<char, int> frequencyMap = new Dictionary<char, int>();
            foreach (char letter in alphabet)
            {
                frequencyMap[letter] = 0;
            }

            foreach (char letter in message)
            {
                if (alphabet.Contains(letter))
                {
                    frequencyMap[letter]++;
                }
            }

            char mostFrequentLetter = frequencyMap.OrderByDescending(x => x.Value).First().Key;

            Dictionary<char, double> symbolFrequency = GetDictionary2(selectedAlphabet);

            char mostFrequentSymbol = symbolFrequency.OrderByDescending(x => x.Value).First().Key;

            int expectedShift = alphabet.IndexOf(mostFrequentSymbol) - alphabet.IndexOf(mostFrequentLetter);

            string result = "";

            List<KeyValuePair<char, double>> sortedFrequencyList = symbolFrequency.ToList();
            sortedFrequencyList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            string filePath = selectedAlphabet == "English" ? "E:\\C#\\Mathematical cryptology\\words.txt" : "E:\\C#\\Mathematical cryptology\\words_ukranian.txt";
            List<string> lines = File.ReadAllLines(filePath).ToList();

            while (sortedFrequencyList.Count > 0)
            {
                KeyValuePair<char, double> lastElement = sortedFrequencyList[sortedFrequencyList.Count - 1];
                char lastletter = lastElement.Key;
                int key = alphabet.IndexOf(mostFrequentLetter) - alphabet.IndexOf(lastletter);

                string decryptedMessage = CeasarDecryption(alphabet, message, key);
                string[] words = decryptedMessage.Split(' ');

                foreach (string word in words)
                {
                    if (lines.Contains(word.ToLower()))
                    {
                        result = $"Expected shift: {key}\nDecrypted word: {word}";
                        return result;
                    }
                }
                sortedFrequencyList.RemoveAt(sortedFrequencyList.Count - 1);
            }

            return result;
        }

        private string CeasarDecryption(string alphabet, string message, int shift)
        {
            shift = (shift % alphabet.Length + alphabet.Length) % alphabet.Length;
            string result = "";
            foreach (char c in message)
            {
                if (alphabet.Contains(c))
                {
                    int index = (alphabet.IndexOf(c) - shift + alphabet.Length) % alphabet.Length;
                    result += alphabet[index];
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }

        /*  public string Cryptanalyze_Frequency(string message, string selectedAlphabet)
          {
              var dictionary = GetDictionary(selectedAlphabet);

              string alphabet = selectedAlphabet == "English" ? englishAlphabet : ukrainianAlphabet;

              Dictionary<char, int> frequencyMap = new Dictionary<char, int>();
              foreach (char letter in alphabet)
              {
                  frequencyMap[letter] = 0;
              }

              foreach (char letter in message)
              {
                  if (alphabet.Contains(letter))
                  {
                      frequencyMap[letter]++;
                  }
              }

              char mostFrequentLetter = frequencyMap.OrderByDescending(x => x.Value).First().Key;

              Dictionary<char, double> symbolFrequency = selectedAlphabet == "English" ? englishSymbolFrequency : ukrainianSymbolFrequency;

              char mostFrequentSymbol = symbolFrequency.OrderByDescending(x => x.Value).First().Key;

              int expectedShift = alphabet.IndexOf(mostFrequentSymbol) - alphabet.IndexOf(mostFrequentLetter);

              string result = "";

              for (int shift = 0; shift < alphabet.Length; shift++)
              {
                  string decryptedMessage = CaesarDecryptionWithShift(alphabet, message, shift);
                  string[] words = decryptedMessage.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                  foreach (string word in words)
                  {
                      if (dictionary.Contains(word.ToLower()))
                      {
                          result += $"Exepted shift: {shift} \nExepted decrypted word: {word}\n";
                          break;
                      }
                  }
              }

              return result;
          }

          private string CaesarDecryptionWithShift(string alphabet, string message, int shift)
          {
              shift = (shift % alphabet.Length + alphabet.Length) % alphabet.Length;
              string result = "";
              foreach (char c in message)
              {
                  if (alphabet.Contains(c))
                  {
                      int index = (alphabet.IndexOf(c) - shift + alphabet.Length) % alphabet.Length;
                      result += alphabet[index];
                  }
                  else
                  {
                      result += c;
                  }
              }
              return result;
          }*/

        private DateTime startTime;
        private DateTime endTime;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            if (chkCryptographicAnalysis.IsChecked == true)
            {
                string ciphertext = txtInput.Text;
                string selectedAlphabet = chkEnglishAlphabet.IsChecked == true ? "English" : "Ukrainian";

                Cryptanalyze(ciphertext, selectedAlphabet);
            }
            else
            {
                int shift = int.Parse(txtShift.Text);
                string selectedAlphabet = chkEnglishAlphabet.IsChecked == true ? "English" : "Ukrainian";
                int blockSize = int.Parse(txtNumberOfBlocks.Text);

                if (chkReadFromConsole.IsChecked == true)
                {
                    string inputText = txtInput.Text;

                    startTime = DateTime.Now;

                    string result = ProcessBlocks(inputText, shift, selectedAlphabet, blockSize, chkEncrypt.IsChecked == true);

                    endTime = DateTime.Now;
                    TimeSpan elapsedTime = endTime - startTime;

                    txtResult.Text = $"Output:\n{result}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                }
                else if (chkFilePath.IsChecked == true)
                {
                    string filePath = txtInput.Text;
                    string outputFilePath;
                    if (chkEncrypt.IsChecked == true)
                        outputFilePath = @"E:\C#\Mathematical cryptology\output_encrypt.txt";
                    else
                        outputFilePath = @"E:\C#\Mathematical cryptology\output_decrypt.txt";

                    string inputText = File.ReadAllText(filePath);

                    startTime = DateTime.Now;

                    string result = ProcessBlocks(inputText, shift, selectedAlphabet, blockSize, chkEncrypt.IsChecked == true);

                    endTime = DateTime.Now;
                    TimeSpan elapsedTime = endTime - startTime;

                    File.WriteAllText(outputFilePath, result);
                    txtResult.Text = $"Output:\n{outputFilePath}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                }
            }
        }

        private void chkFilePath_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = chkFilePath.IsChecked == true ? "Enter file path" : "Enter text";
        }
    

        private void chkCryptographicAnalysis_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtShift_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtResult_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void chkReadFromConsole_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkEnglishAlphabet_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkUkrainianAlphabet_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtNumberOfBlocks_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}





/*
namespace Mathematical_cryptology
{
    public partial class CaesarCipher : Window
    {
        private readonly string englishAlphabet = "abcdefghijklmnopqrstuvwxyz ,.!?;:'\"()[]{}";
        private readonly string ukrainianAlphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя ,.!?;:'\"()[]{}";

        private string selectedAlphabet = "English";

        public CaesarCipher()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Zero;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            this.Close();
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ListBoxItem)sender;

            switch (selectedItem.Content.ToString())
            {
                case "Шифр частоколу [ Rail Fence ]":
                    new RailFenceCipher().Show();
                    break;
                case "Шифр зсуву [ Caesar cipher ]":
                    Show();
                    break;
                    // add*
            }
            Close();
        }

        private void TxtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter text or link" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter text or link";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtShift_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter shift value" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtShift_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter shift value";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtNumberOfBlocks_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter number of blocks" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtNumberOfBlocks_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter number of blocks";
            }
            textBox.Foreground = Brushes.White;
        }

        private bool ValidateFields()
        {
            if (!int.TryParse(txtShift.Text, out int shift) || shift < 0)
            {
                MessageBox.Show("Shift value must be a non-negative integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtNumberOfBlocks.Text, out int numberOfBlocks) || numberOfBlocks < 0)
            {
                MessageBox.Show("Number of blocks must be a non-negative integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkEncrypt.IsChecked.GetValueOrDefault(false) && !chkDecrypt.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select at least one method (Encrypt or Decrypt).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkEnglishAlphabet.IsChecked.GetValueOrDefault(false) && !chkUkrainianAlphabet.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select at least one language (English Alphabet or Ukrainian Alphabet).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!chkReadFromConsole.IsChecked.GetValueOrDefault(false) && !chkFilePath.IsChecked.GetValueOrDefault(false))
            {
                MessageBox.Show("Please select either 'Read from the console' or 'File path'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (chkFilePath.IsChecked.GetValueOrDefault(false))
            {
                string filePath = txtInput.Text;
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void btnEnglishAlphabet_Click(object sender, RoutedEventArgs e)
        {
            selectedAlphabet = "English";
        }

        private void btnUkrainianAlphabet_Click(object sender, RoutedEventArgs e)
        {
            selectedAlphabet = "Ukrainian";
        }

        public string EncryptBlock(string block, int shift, string selectedAlphabet)
        {
            var alphabet = selectedAlphabet.Equals("English") ? englishAlphabet : ukrainianAlphabet;
            return TransformText(block, shift, alphabet);
        }

        public string DecryptBlock(string block, int shift, string selectedAlphabet)
        {
            return EncryptBlock(block, -shift, selectedAlphabet);
        }

        private string TransformText(string text, int shift, string alphabet)
        {
            var result = new StringBuilder();

            foreach (var c in text)
            {
                var index = alphabet.IndexOf(c);
                result.Append(index == -1 ? c : alphabet[(index + shift + alphabet.Length) % alphabet.Length]);
            }

            return result.ToString();
        }

        private string ProcessBlocks(string inputText, int shift, string selectedAlphabet, int blockSize, bool encrypt)
        {
            if (blockSize == 0)
                return encrypt ? EncryptBlock(inputText, shift, selectedAlphabet) : DecryptBlock(inputText, shift, selectedAlphabet);

            var blocks = new List<string>();
            for (int i = 0; i < inputText.Length; i += blockSize)
            {
                var length = Math.Min(blockSize, inputText.Length - i);
                blocks.Add(inputText.Substring(i, length));
            }

            var processedBlocks = new List<string>();

            Parallel.ForEach(blocks, block =>
            {
                string processedBlock = encrypt ? EncryptBlock(block, shift, selectedAlphabet) : DecryptBlock(block, shift, selectedAlphabet);
                lock (processedBlocks) { processedBlocks.Add(processedBlock); }
            });

            return string.Concat(processedBlocks);
        }

        private HashSet<string> LoadDictionary(string filePath)
        {
            HashSet<string> dictionary = new HashSet<string>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    dictionary.Add(line.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the dictionary: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return dictionary;
        }


        private HashSet<string> GetDictionary()
        {
            string filePath = selectedAlphabet == "English" ? @"E:\C#\Mathematical cryptology\words.txt" : @"E:\C#\Mathematical cryptology\words_uk.txt";
            return LoadDictionary(filePath);
        }

        private string BruteForce(string ciphertext, string selectedAlphabet)
        {
            var dictionary = GetDictionary();
            var decryptedText = "";

            for (int shift = 1; shift <= selectedAlphabet.Length; shift++)
            {
                decryptedText = DecryptBlock(ciphertext, shift, selectedAlphabet);

                if (IsInDictionary(decryptedText, dictionary))
                {
                    return decryptedText;
                }
            }

            return ciphertext;
        }

        private bool IsInDictionary(string text, HashSet<string> dictionary)
        {           
            var words = text.ToLower().Split(new char[] { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (dictionary.Contains(word))
                {
                    return true;
                }
            }

            return false;
        }


        private DateTime startTime;
        private DateTime endTime;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            if (chkCryptographicAnalysis.IsChecked == true)
            {
                string ciphertext = txtInput.Text;
                string selectedAlphabet = chkEnglishAlphabet.IsChecked == true ? "English" : "Ukrainian";

                string decryptedText = BruteForce(ciphertext, selectedAlphabet);

                txtResult.Text = $"Decrypted text using dictionary: {decryptedText}";
            }
            else
            {
                int shift = int.Parse(txtShift.Text);
                string selectedAlphabet = chkEnglishAlphabet.IsChecked == true ? "English" : "Ukrainian";
                int blockSize = int.Parse(txtNumberOfBlocks.Text);

                if (chkReadFromConsole.IsChecked == true)
                {
                    string inputText = txtInput.Text;

                    startTime = DateTime.Now;

                    string result = ProcessBlocks(inputText, shift, selectedAlphabet, blockSize, chkEncrypt.IsChecked == true);

                    endTime = DateTime.Now;
                    TimeSpan elapsedTime = endTime - startTime;

                    txtResult.Text = $"Output:\n{result}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                }
                else if (chkFilePath.IsChecked == true)
                {
                    string filePath = txtInput.Text;
                    string outputFilePath;
                    if (chkEncrypt.IsChecked == true)
                        outputFilePath = @"E:\C#\Mathematical cryptology\output_encrypt.txt";
                    else
                        outputFilePath = @"E:\C#\Mathematical cryptology\output_decrypt.txt";

                    string inputText = File.ReadAllText(filePath);

                    startTime = DateTime.Now;

                    string result = ProcessBlocks(inputText, shift, selectedAlphabet, blockSize, chkEncrypt.IsChecked == true);

                    endTime = DateTime.Now;
                    TimeSpan elapsedTime = endTime - startTime;

                    File.WriteAllText(outputFilePath, result);
                    txtResult.Text = $"Output:\n{outputFilePath}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                }
            }
        }


        private void chkFilePath_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = chkFilePath.IsChecked == true ? "Enter file path" : "Enter text";
        }


        private List<string> SplitTextIntoBlocks(string text, int blockSize)
        {
            List<string> blocks = new List<string>();
            for (int i = 0; i < text.Length; i += blockSize)
            {
                blocks.Add(text.Substring(i, Math.Min(blockSize, text.Length - i)));
            }
            return blocks;

        }


        private void chkCryptographicAnalysis_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtShift_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtResult_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void chkReadFromConsole_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkEnglishAlphabet_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkUkrainianAlphabet_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtNumberOfBlocks_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
*/