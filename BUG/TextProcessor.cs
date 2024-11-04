/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematical_cryptology_RailFenceCipher;
using Mathematical_cryptology_CaesarCipher;

namespace Mathematical_cryptology_blocks
{
    public class TextProcessor
    {
        private CaesarCipher caesarCipher;
        private RailFenceCipher railFenceCipher;

        public TextProcessor()
        {
            caesarCipher = new CaesarCipher();
            railFenceCipher = new RailFenceCipher();
        }

        public string ProcessBlocks(string inputText, int shiftOrRails, string selectedAlphabet, int blockSize, bool encrypt, string selectedCipher, bool topToBottom = true)
        {
            if (selectedCipher == "Caesar")
            {
                return ProcessBlocksUsingCaesarCipher(inputText, shiftOrRails, selectedAlphabet, blockSize, encrypt).Result;
            }
            else if (selectedCipher == "RailFence")
            {
                return ProcessBlocksUsingRailFenceCipher(inputText, shiftOrRails, blockSize, encrypt, topToBottom).Result; 
            }
            else
            {
                throw new ArgumentException("Unknown cipher");
            }
        }

        public async Task<string> ProcessBlocksUsingCaesarCipher(string inputText, int shift, string selectedAlphabet, int blockSize, bool encrypt)
        {
            if (blockSize == 0)
                return encrypt ? caesarCipher.EncryptBlock(inputText, shift, selectedAlphabet) : caesarCipher.DecryptBlock(inputText, shift, selectedAlphabet);

            var blocks = new List<string>();
            for (int i = 0; i < inputText.Length; i += blockSize)
            {
                var length = Math.Min(blockSize, inputText.Length - i);
                blocks.Add(inputText.Substring(i, length));
            }

            var processedBlocks = new List<string>();

            foreach (var block in blocks)
            {
                string processedBlock = encrypt ? caesarCipher.EncryptBlock(block, shift, selectedAlphabet) : caesarCipher.DecryptBlock(block, shift, selectedAlphabet);
                processedBlocks.Add(processedBlock);
            }

            return string.Concat(processedBlocks);
        }

        public async Task<string> ProcessBlocksUsingRailFenceCipher(string inputText, int rails, int blockSize, bool encrypt, bool topToBottom)
        {
            List<Task<string>> tasks = new List<Task<string>>();

            if (blockSize > 0)
            {
                for (int i = 0; i < inputText.Length; i += blockSize)
                {
                    int blockStart = i;
                    tasks.Add(Task.Run(() =>
                    {
                        var block = inputText.Substring(blockStart, Math.Min(blockSize, inputText.Length - blockStart));
                        return encrypt ? railFenceCipher.RailFenceCipherEncrypt(block, rails, topToBottom) : railFenceCipher.RailFenceCipherDecrypt(block, rails, topToBottom);
                    }));
                }
            }
            else
            {
                tasks.Add(Task.Run(() => encrypt ? railFenceCipher.RailFenceCipherEncrypt(inputText, rails, topToBottom) : railFenceCipher.RailFenceCipherDecrypt(inputText, rails, topToBottom)));
            }

            var results = await Task.WhenAll(tasks);
            return string.Concat(results);
        }
    }
}*/
