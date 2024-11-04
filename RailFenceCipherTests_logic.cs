using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Mathematical_cryptology_RailFenceCipher
{
    public class EncryptionService
    {

        public string RailFenceCipherEncryptDecryptBlocks(string text, int rails, int blockSize, bool encrypt, bool topToBottom)
        {
            StringBuilder result = new StringBuilder();
            if (blockSize > 0)
            {
                for (int i = 0; i < text.Length; i += blockSize)
                {
                    var block = text.Substring(i, Math.Min(blockSize, text.Length - i));
                    result.Append(encrypt ? RailFenceCipherEncrypt(block, rails, topToBottom) : RailFenceCipherDecrypt(block, rails, topToBottom));
                }
            }
            else
            {
                result.Append(encrypt ? RailFenceCipherEncrypt(text, rails, topToBottom) : RailFenceCipherDecrypt(text, rails, topToBottom));
            }
            return result.ToString();
        }

        public string RailFenceCipherEncrypt(string plainText, int rails, bool topToBottom)
        {
            if (rails == 1)
                return plainText;

            StringBuilder result = new StringBuilder();
            int cycleLen = 2 * rails - 2;

            if (topToBottom)
            {
                for (int i = 0; i < rails; i++)
                {
                    for (int j = i; j < plainText.Length; j += cycleLen)
                    {
                        result.Append(plainText[j]);
                        int secondJ = (j - i) + cycleLen - i;
                        if (i != 0 && i != rails - 1 && secondJ < plainText.Length)
                        {
                            result.Append(plainText[secondJ]);
                        }
                    }
                }
            }
            else
            {
                for (int i = rails - 1; i >= 0; i--)
                {
                    for (int j = i; j < plainText.Length; j += cycleLen)
                    {
                        result.Append(plainText[j]);
                        int secondJ = (j - i) + cycleLen - i;
                        if (i != 0 && i != rails - 1 && secondJ < plainText.Length)
                        {
                            result.Append(plainText[secondJ]);
                        }
                    }
                }
            }

            return result.ToString();
        }

        public string RailFenceCipherDecrypt(string cipherText, int rails, bool topToBottom)
        {
            if (rails == 1)
                return cipherText;

            int cycleLen = 2 * rails - 2;
            char[] result = new char[cipherText.Length];
            int index = 0;

            if (topToBottom)
            {
                for (int i = 0; i < rails; i++)
                {
                    for (int j = i; j < cipherText.Length; j += cycleLen)
                    {
                        result[j] = cipherText[index++];
                        int secondJ = (j - i) + cycleLen - i;
                        if (i != 0 && i != rails - 1 && secondJ < cipherText.Length && index < cipherText.Length)
                        {
                            result[secondJ] = cipherText[index++];
                        }
                    }
                }
            }
            else
            {
                for (int i = rails - 1; i >= 0; i--)
                {
                    for (int j = i; j < cipherText.Length; j += cycleLen)
                    {
                        result[j] = cipherText[index++];
                        int secondJ = (j - i) + cycleLen - i;
                        if (i != 0 && i != rails - 1 && secondJ < cipherText.Length && index < cipherText.Length)
                        {
                            result[secondJ] = cipherText[index++];
                        }
                    }
                }
            }

            return new string(result);
        }
    }
}


