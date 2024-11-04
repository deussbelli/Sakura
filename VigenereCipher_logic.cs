using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Mathematical_cryptology_VigenereCipher
{
    public class EncryptionService_fourth
    {
        public string englishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        public bool englishCheckbox = true;

        public string GetAlphabet()
        {
            return englishCheckbox ? englishAlphabet : ukrainianAlphabet;
        }


        public (string, string) VigenereEncrypt(string text, string key)
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

        public (string, string) VigenereDecrypt(string text, string key)
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


        public string Cryptoanalysis(string cipherText, List<char> alphabet, List<double> frequencies)
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

        public string EnglishCryptoanalysis(string cipherText)
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

        public string UkrainianCryptoanalysis(string cipherText)
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

        public string EnglishKasiskiFriedmanCryptoanalysis(string cipherText)
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

        public string UkrainianKasiskiFriedmanCryptoanalysis(string cipherText)
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



        public (string, string) KasiskiFriedmanCryptoanalysis(string cipherText, List<char> alphabet, List<double> frequencies)
        {
            int patternLength = 5;
            var patternDict = new Dictionary<string, int>();

            var maxPatternCount = patternDict.Count > 0 ? patternDict.Max(kvp => kvp.Value) : 0;

            for (int i = 0; i < cipherText.Length; i += patternLength)
            {
                if (i + patternLength > cipherText.Length - 1) continue;

                var substring = cipherText.Substring(i, patternLength);
                if (!patternDict.ContainsKey(substring)) patternDict.Add(substring, 0);

                patternDict[substring]++;
            }

            if (patternDict.Count == 0)
            {
                return ("", ""); // Return empty strings if no patterns are found
            }


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

            var period = distances.Any() ? distances.Aggregate(GCD1) : 1;


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

    }
}
