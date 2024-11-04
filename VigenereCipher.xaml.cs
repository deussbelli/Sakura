using Mathematical_cryptology_CaesarCipher;
using Mathematical_cryptology_RailFenceCipher;
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
using static System.Net.Mime.MediaTypeNames;
using Mathematical_cryptology_CardanoGrilleCipher;
using Mathematical_cryptology_AffineCipher;

namespace Mathematical_cryptology_VigenereCipher
{
    public partial class VigenereCipher : Window
    {
        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e) => mediaElement.Position = TimeSpan.Zero;

        public string englishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        public List<char> englishAlphabet1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        public List<char> ukrainianAlphabet1 = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ".ToList();

        private List<string> englishDictionary;
        private List<string> ukrainianDictionary;
        private List<string> englishDictionaryKey;
        private List<string> ukrainianDictionaryKey;

        public VigenereCipher()
        {
            InitializeComponent();
            englishDictionaryKey = LoadDictionary("E:\\C#\\Mathematical cryptology\\english_keywords.txt");
            ukrainianDictionaryKey = LoadDictionary("E:\\C#\\Mathematical cryptology\\ukranian_keywords.txt");
            englishDictionary = LoadDictionary("E:\\C#\\Mathematical cryptology\\words.txt");
            ukrainianDictionary = LoadDictionary("E:\\C#\\Mathematical cryptology\\words_ukranian_test.txt");

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5000) };
            timer.Tick += Timer_Tick;
            timer.Start();
            textInput.TextChanged += TextBox_TextChanged;
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
                if (textBox.Name == "textInput" && textCheckbox.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "textInput" && fileCheckbox.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "keyLengthEntry")
                    textBox.Text = "Key";
                else if (textBox.Name == "recommendedKeyLengthTextBox")
                    textBox.Text = "Recommended key";
                else if (textBox.Name == "keyInput")
                    textBox.Text = "Entered key";
            }
            textBox.Foreground = Brushes.White;
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Contains("Enter"))
            {
                if (textBox.Name == "textInput" && textCheckbox.IsChecked == true)
                    textBox.Text = "Enter text";
                else if (textBox.Name == "textInput" && fileCheckbox.IsChecked == true)
                    textBox.Text = "Enter link";
                else if (textBox.Name == "keyLengthEntry")
                    textBox.Text = "Key";
                else if (textBox.Name == "recommendedKeyLengthTextBox")
                    textBox.Text = "Recommended key";
                else if (textBox.Name == "keyInput")
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Name == "textCheckbox")
            {
                textInput.Text = "Enter text";
                textInput.IsEnabled = true;
                keyInput.Text = "Entered key";
                keyInput.IsEnabled = true;
            }
            else if (chkBox.Name == "fileCheckbox")
            {
                textInput.Text = "Paste file link";
                textInput.IsEnabled = true;
                keyInput.Text = "Entered key";
                keyInput.IsEnabled = true;
            }
        }

        public void StartButton_Click(object sender, RoutedEventArgs e)
        {

            if (!(fileCheckbox.IsChecked == true || textCheckbox.IsChecked == true))
            {
                resultOutput.Text = "Please select either read from file or read from textbox.";
                return;
            }

            string text = "";

            if (textCheckbox.IsChecked == true)
            {
                text = textInput.Text.ToUpper();
            }
            else if (fileCheckbox.IsChecked == true)
            {
                string filePath = textInput.Text;
                if (string.IsNullOrEmpty(filePath))
                {
                    resultOutput.Text = "Please enter the file path.";
                    return;
                }

                try
                {
                    text = File.ReadAllText(filePath).ToUpper();
                }
                catch (Exception ex)
                {
                    resultOutput.Text = $"Error reading file: {ex.Message}";
                    return;
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                resultOutput.Text = "Input text is empty.";
                return;
            }

            if (cryptoanalysisCheckbox.IsChecked == true && cryptoanalysisKFCheckbox.IsChecked == true)
            {
                resultOutput.Text = "Please select only one cryptoanalysis method.";
                return;
            }

            if (cryptoanalysisCheckbox.IsChecked == true)
            {
                if (englishCheckbox.IsChecked == true)
                {
                    resultOutput.Text = EnglishCryptoanalysis(text);
                }
                else if (ukrainianCheckbox.IsChecked == true)
                {
                    resultOutput.Text = UkrainianCryptoanalysis(text);
                }
                else
                {
                    resultOutput.Text = "Please select a language for cryptoanalysis.";
                    return;
                }
            }
            else if (cryptoanalysisKFCheckbox.IsChecked == true)
            {
                string result = "";

                if (englishCheckbox.IsChecked == true || ukrainianCheckbox.IsChecked == true)
                {
                    string text_cr = textCheckbox.IsChecked == true ? textInput.Text.ToUpper() : File.ReadAllText(textInput.Text).ToUpper();

                    if (englishCheckbox.IsChecked == true)
                    {
                        result = EnglishKasiskiFriedmanCryptoanalysis(text_cr);
                    }
                    else if (ukrainianCheckbox.IsChecked == true)
                    {
                        result = UkrainianKasiskiFriedmanCryptoanalysis(text_cr);
                    }

                    string filePath = "E:\\C#\\Mathematical cryptology\\Kasiski-Friedman_cryptoanalysis.txt";

                    File.WriteAllText(filePath, result);

                    resultOutput.Text = $"Result saved to file: {filePath}";
                }
                else
                {
                    resultOutput.Text = "Please select a language for cryptoanalysis.";
                    return;
                }
            }
            else if (bruteForceCheckbox.IsChecked == true)
            {
                BruteForceDecrypt(text);
            }
            else
            {
                string key = keyInput.Text.ToUpper();
                string result = "";
                string executionTime = "";

                if (encryptCheckbox.IsChecked == true)
                {
                    (result, executionTime) = VigenereEncrypt(text, key);
                }
                else if (decryptCheckbox.IsChecked == true)
                {
                    (result, executionTime) = VigenereDecrypt(text, key);
                }
                else
                {
                    result = "Please select an action.";
                }

                if (fileCheckbox.IsChecked == true)
                {
                    string filePath = encryptCheckbox.IsChecked == true ? "E:\\C#\\Mathematical cryptology\\vigenere_output_encrypt.txt" :
                                      "E:\\C#\\Mathematical cryptology\\vigenere_output_decrypt.txt";
                    File.WriteAllText(filePath, result);
                    resultOutput.Text = $"Result saved to file: {filePath}";
                }
                else
                {
                    resultOutput.Text = $"Execution time: {executionTime} milliseconds\n{result}";
                }
            }
        }

        private string Cryptoanalysis(string cipherText, List<char> alphabet, List<double> frequencies)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int patternLength = 5;
            var patternDict = new Dictionary<string, int>();

            for (int i = 0; i < cipherText.Length; i += patternLength)
            {
                if (i + patternLength > cipherText.Length - 1) continue;

                var substring = cipherText.Substring(i, patternLength);
                if (!patternDict.ContainsKey(substring)) patternDict.Add(substring, 0);

                patternDict[substring]++;
            }

            var maxPatternCount = patternDict.Max(kvp => kvp.Value);

            var distances = new List<int>();
            foreach (var kvp in patternDict.Where(kvp => kvp.Value == maxPatternCount))
            {
                bool searching = true;
                int currentIndex = -1;
                while (searching)
                {
                    var nextIndex = cipherText.IndexOf(kvp.Key, currentIndex == -1 ? 0 : currentIndex + patternLength);

                    if (nextIndex == -1)
                    {
                        searching = false;
                        continue;
                    }
                    else if (currentIndex != -1)
                    {
                        distances.Add(nextIndex - currentIndex);
                    }

                    currentIndex = nextIndex;
                }
            }

            var period = distances.Aggregate(GCD);

            var slices = new List<string>();

            for (int i = 0; i < cipherText.Length; i += period)
            {
                var substring = "";
                if (i + period > cipherText.Length - 1)
                    substring = cipherText.Substring(i);
                else
                    substring = cipherText.Substring(i, period);
                slices.Add(substring);
            }

            var kList = new List<int>();
            for (int t = 0; t < period; t++)
            {
                var tText = "";

                foreach (var slice in slices.Where(sl => sl.Length == period)) tText += slice[t];

                var iDict = new Dictionary<int, double>();
                for (int j = 0; j < alphabet.Count; j++)
                {
                    double I = 0;
                    for (int i = 0; i < alphabet.Count; i++)
                    {
                        var alphabetIndex = (i + j) % alphabet.Count;
                        I += frequencies[i] * tText.Where(c => c == alphabet[alphabetIndex]).Count() / tText.Length;
                    }
                    iDict.Add(j, I);
                }

                var nearestJ = iDict.OrderBy(kvp => Math.Abs(kvp.Value - frequencies[0])).First();
                kList.Add(nearestJ.Key);
            }

            var plaintext = "";
            foreach (var slice in slices)
            {
                for (int i = 0; i < slice.Length; i++)
                {
                    var alphabetIndex = (alphabet.IndexOf(slice[i]) - kList[i] + alphabet.Count) % alphabet.Count;

                    plaintext += alphabet[alphabetIndex];
                }
            }

            stopwatch.Stop();

            return $"Cryptoanalysis execution time: {stopwatch.ElapsedMilliseconds} milliseconds\n{plaintext}";

        }


        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }



        private string EnglishCryptoanalysis(string cipherText)
        {
            List<char> englishAlphabet = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".AsEnumerable());
            List<double> englishFrequencies = new List<double>() {
                0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015,
                0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749,
                0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758,
                0.00978, 0.02360, 0.00150, 0.01974, 0.00074
            };
            return Cryptoanalysis(cipherText, englishAlphabet, englishFrequencies);
        }

        private string UkrainianCryptoanalysis(string cipherText)
        {
            List<char> ukrainianAlphabet = new List<char>("АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ".AsEnumerable());
            List<double> ukrainianFrequencies = new List<double>() {
                  0.072, 0.017,  0.052,  0.016, 0.016,  0.035, 0.017,  0.008,  0.009,
                  0.023,  0.061, 0.057,   0.006,   0.008, 0.035, 0.036,0.031,0.065,
                  0.094, 0.029,0.047,0.041,0.055,0.04,0.001,0.012, 0.006, 0.018, 0.012,
                  0.001,0.029,0.004,0.029
            };
            return Cryptoanalysis(cipherText, ukrainianAlphabet, ukrainianFrequencies);
        }


        static int GCD(params int[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        private string EnglishKasiskiFriedmanCryptoanalysis(string cipherText)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<char> englishAlphabet = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".AsEnumerable());
            List<double> englishFrequencies = new List<double>() {
                0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015,
                0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749,
                0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758,
                0.00978, 0.02360, 0.00150, 0.01974, 0.00074
            };
            var result = KasiskiFriedmanCryptoanalysis(cipherText, englishAlphabet, englishFrequencies);

            stopwatch.Stop();

            return $"English Cryptoanalysis execution time: {stopwatch.ElapsedMilliseconds} milliseconds\nKey: {result.Item2},\nDecrypted text: {result.Item1}\n";
        }

        private string UkrainianKasiskiFriedmanCryptoanalysis(string cipherText)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<char> ukrainianAlphabet = new List<char>("АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ".AsEnumerable());
            List<double> ukrainianFrequencies = new List<double>() {
                0.072, 0.017,  0.052,  0.016, 0.016,  0.035, 0.017,  0.008,  0.009,
                0.023,  0.061, 0.057,   0.006,   0.008, 0.035, 0.036,0.031,0.065,
                0.094, 0.029,0.047,0.041,0.055,0.04,0.001,0.012, 0.006, 0.018, 0.012,
                0.001,0.029,0.004,0.029
            };

            var result = KasiskiFriedmanCryptoanalysis(cipherText, ukrainianAlphabet, ukrainianFrequencies);

            stopwatch.Stop();

            return $"Ukrainian Cryptoanalysis execution time: {stopwatch.ElapsedMilliseconds} milliseconds\nKey: {result.Item2},\nDecrypted text: {result.Item1}\n";
        }



        private (string, string) KasiskiFriedmanCryptoanalysis(string cipherText, List<char> alphabet, List<double> frequencies)
        {
            int patternLength = 5;
            var patternDict = new Dictionary<string, int>();

            for (int i = 0; i < cipherText.Length; i += patternLength)
            {
                if (i + patternLength > cipherText.Length - 1) continue;

                var substring = cipherText.Substring(i, patternLength);
                if (!patternDict.ContainsKey(substring)) patternDict.Add(substring, 0);

                patternDict[substring]++;
            }

            var maxPatternCount = patternDict.Max(kvp => kvp.Value);

            var distances = new List<int>();
            foreach (var kvp in patternDict.Where(kvp => kvp.Value == maxPatternCount))
            {
                bool searching = true;
                int currentIndex = -1;
                while (searching)
                {
                    var nextIndex = cipherText.IndexOf(kvp.Key, currentIndex == -1 ? 0 : currentIndex + patternLength);

                    if (nextIndex == -1)
                    {
                        searching = false;
                        continue;
                    }
                    else if (currentIndex != -1)
                    {
                        distances.Add(nextIndex - currentIndex);
                    }

                    currentIndex = nextIndex;
                }
            }

            var period = distances.Aggregate(GCD1);

            var slices = new List<string>();
            for (int i = 0; i < cipherText.Length; i += period)
            {
                var substring = "";
                if (i + period > cipherText.Length - 1)
                    substring = cipherText.Substring(i);
                else
                    substring = cipherText.Substring(i, period);
                slices.Add(substring);
            }

            var kList = new List<int>();
            for (int t = 0; t < period; t++)
            {
                var tText = "";

                foreach (var slice in slices.Where(sl => sl.Length == period)) tText += slice[t];

                var iDict = new Dictionary<int, double>();
                for (int j = 0; j < alphabet.Count; j++)
                {
                    double I = 0;
                    for (int i = 0; i < alphabet.Count; i++)
                    {
                        var alphabetIndex = (i + j) % alphabet.Count;
                        I += frequencies[i] * tText.Where(c => c == alphabet[alphabetIndex]).Count() / tText.Length;
                    }
                    iDict.Add(j, I);
                }

                var nearestJ = iDict.OrderBy(kvp => Math.Abs(kvp.Value - frequencies[0])).First();
                kList.Add(nearestJ.Key);
            }

            var plaintext = "";
            foreach (var slice in slices)
            {
                for (int i = 0; i < slice.Length; i++)
                {
                    var alphabetIndex = (alphabet.IndexOf(slice[i]) - kList[i] + alphabet.Count) % alphabet.Count;

                    plaintext += alphabet[alphabetIndex];
                }
            }

            return (plaintext, string.Join(", ", kList.Select(k => k.ToString())));
        }

        static int GCD1(int a, int b)
        {
            return b == 0 ? a : GCD1(b, a % b);
        }


        private void BruteForceDecrypt(string text)
        {

        }


        private void bruteForceCheckbox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void bruteForceCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            keyInput.IsEnabled = true;
        }


        private List<string> LoadDictionary(string filePath)
        {
            return File.ReadAllLines(filePath).ToList();
        }

        private (string, string) VigenereEncrypt(string text, string key)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                if (char.IsLetter(currentChar))
                {
                    string alphabet = GetAlphabet();
                    int charIndex = alphabet.IndexOf(currentChar);
                    int keyIndex = alphabet.IndexOf(key[i % key.Length]);
                    int encryptedIndex = (charIndex + keyIndex) % alphabet.Length;
                    result.Append(alphabet[encryptedIndex]);
                }
                else
                {
                    result.Append(currentChar);
                }
            }

            stopwatch.Stop();

            return (result.ToString(), stopwatch.ElapsedMilliseconds.ToString());
        }

        private (string, string) VigenereDecrypt(string text, string key)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                if (char.IsLetter(currentChar))
                {
                    string alphabet = GetAlphabet();
                    int charIndex = alphabet.IndexOf(currentChar);
                    int keyIndex = alphabet.IndexOf(key[i % key.Length]);
                    int decryptedIndex = (charIndex - keyIndex + alphabet.Length) % alphabet.Length;
                    result.Append(alphabet[decryptedIndex]);
                }
                else
                {
                    result.Append(currentChar);
                }
            }

            stopwatch.Stop();

            return (result.ToString(), stopwatch.ElapsedMilliseconds.ToString());
        }


        private string GetAlphabet()
        {
            return englishCheckbox.IsChecked == true ? englishAlphabet : ukrainianAlphabet;
        }

        private void englishCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ukrainianCheckbox.IsChecked = false;
        }

        private void ukrainianCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            englishCheckbox.IsChecked = false;
        }

        private void fileCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            textCheckbox.IsChecked = false;
        }

        private void textCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            fileCheckbox.IsChecked = false;
        }

        private void decryptCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void encryptCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cryptoanalysisCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cryptoanalysisKFCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void keyEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
