using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Mathematical_cryptology_CaesarCipher
{
    public class EncryptionService_second
    {
        private readonly string englishAlphabet = "abcdefghijklmnopqrstuvwxyz ";
        private readonly string ukrainianAlphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя ";
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

    }
}
