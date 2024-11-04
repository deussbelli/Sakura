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
using Mathematical_cryptology_CaesarCipher;
using Mathematical_cryptology_CardanoGrilleCipher;
using Mathematical_cryptology_VigenereCipher;
using Mathematical_cryptology_AffineCipher;

namespace Mathematical_cryptology_RailFenceCipher
{
    public partial class RailFenceCipher : Window

    {
        public RailFenceCipher()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ListBoxItem)sender;

            switch (selectedItem.Content.ToString())
            {
                case "Шифр частоколу [ Rail Fence ]":
                    lstMethods.SelectedItem = selectedItem;
                    return; 
                case "Шифр зсуву [ Caesar cipher ]":
                    new CaesarCipher().Show();
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


        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Zero;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            this.Close();
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


        private void TxtHeightOfFence_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter height of fence" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtHeightOfFence_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter height of fence";
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

        private async Task<string> RailFenceCipherEncryptDecryptBlocksAsync(string text, int rails, int blockSize, bool encrypt, bool topToBottom)
        {
            List<Task<string>> tasks = new List<Task<string>>();

            if (blockSize > 0)
            {
                for (int i = 0; i < text.Length; i += blockSize)
                {
                    int blockStart = i;
                    tasks.Add(Task.Run(() =>
                    {
                        var block = text.Substring(blockStart, Math.Min(blockSize, text.Length - blockStart));
                        return encrypt ? RailFenceCipherEncrypt(block, rails, topToBottom) : RailFenceCipherDecrypt(block, rails, topToBottom);
                    }));
                }
            }
            else
            {
                tasks.Add(Task.Run(() => encrypt ? RailFenceCipherEncrypt(text, rails, topToBottom) : RailFenceCipherDecrypt(text, rails, topToBottom)));
            }

            var results = await Task.WhenAll(tasks);
            return string.Concat(results);
        }

        private string RailFenceCipherEncrypt(string plainText, int rails, bool topToBottom)
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

        private string RailFenceCipherDecrypt(string cipherText, int rails, bool topToBottom)
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

        private async void chkCryptographicAnalysis_Checked(object sender, RoutedEventArgs e)
        {
            if (lstMethods.SelectedItem == null)
            {
                MessageBox.Show("Please select an encryption method.");
                return;
            }

            bool encrypt = chkEncrypt.IsChecked == true;
            bool decrypt = chkDecrypt.IsChecked == true;

            if (!encrypt && !decrypt)
            {
                MessageBox.Show("Please select encryption or decryption.");
                return;
            }

            bool encryptTopToBottom = chkEncryptTopToBottom.IsChecked == true;
            bool encryptBottomToTop = chkEncryptBottomToTop.IsChecked == true;

            if (!encryptTopToBottom && !encryptBottomToTop)
            {
                MessageBox.Show("Please select encryption direction.");
                return;
            }

            string inputText = txtInput.Text;
            bool useFilePath = chkFilePath.IsChecked == true;
            string filePath = txtInput.Text;
            int heightOfFence = int.Parse(txtHeightOfFence.Text);
            int numberOfBlocks = int.Parse(txtNumberOfBlocks.Text);

            if (chkReadFromConsole.IsChecked == true)
            {
                inputText = txtInput.Text;
            }
            else if (chkFilePath.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputText) || !File.Exists(inputText))
                {
                    MessageBox.Show("Please enter a valid file path.");
                    return;
                }

                inputText = File.ReadAllText(filePath);
            }

            if (!int.TryParse(txtHeightOfFence.Text, out heightOfFence) || heightOfFence < 0 ||
                !int.TryParse(txtNumberOfBlocks.Text, out numberOfBlocks) || numberOfBlocks < 0 ||
                txtHeightOfFence.Text.Contains(".") || txtNumberOfBlocks.Text.Contains("."))
            {
                MessageBox.Show("Please enter valid integer values for height of fence and number of blocks.");
                return;
            }

            try
            {
                string result = await PerformCryptographicAnalysis(inputText, heightOfFence, numberOfBlocks, encrypt, encryptTopToBottom);
                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async Task<string> PerformCryptographicAnalysis(string inputText, int heightOfFence, int numberOfBlocks, bool encrypt, bool topToBottom)
        {
            StringBuilder analysisResult = new StringBuilder();

            for (int h = 1; h <= heightOfFence; h++)
            {
                for (int b = 1; b <= numberOfBlocks; b++)
                {
                    string analysis = await RailFenceCipherEncryptDecryptBlocksAsync(inputText, h, b, encrypt, topToBottom);

                    if (encrypt)
                    {
                        analysisResult.AppendLine($"Encrypted text with height {h} and {b} blocks:\n{analysis}\n");
                    }
                    else
                    {
                        string[] words = analysis.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string word in words)
                        {
                            if (File.ReadAllText(@"E:\C#\Mathematical cryptology\words.txt").Contains(word.ToLower()))
                            {                               
                                analysisResult.AppendLine($"Decrypted text with height {h} and {b} blocks:\n{analysis}\n");
                                break;
                            }
                        }
                    }
                }
            }

            if (analysisResult.Length == 0)
            {
                analysisResult.AppendLine("No matches found for the specified parameters.");
            }

            return analysisResult.ToString();
        }


        private void chkFilePath_Checked(object sender, RoutedEventArgs e)
        {
            if (chkFilePath.IsChecked == true)
            {
                txtInput.Text = "Enter file path";
            }
            else
            {
                txtInput.Text = "Enter text";
            }
        }

        private DateTime startTime;
        private DateTime endTime;

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            startTime = DateTime.Now;

            if (lstMethods.SelectedItem == null)
            {
                MessageBox.Show("Please select an encryption method.");
                return;
            }

            bool encrypt = chkEncrypt.IsChecked == true;
            bool decrypt = chkDecrypt.IsChecked == true;

            if (!encrypt && !decrypt)
            {
                MessageBox.Show("Please select encryption or decryption.");
                return;
            }

            bool encryptTopToBottom = chkEncryptTopToBottom.IsChecked == true;
            bool encryptBottomToTop = chkEncryptBottomToTop.IsChecked == true;

            if (!encryptTopToBottom && !encryptBottomToTop)
            {
                MessageBox.Show("Please select encryption direction.");
                return;
            }

            string inputText = txtInput.Text;
            bool useFilePath = chkFilePath.IsChecked == true;
            string filePath = txtInput.Text;
            int heightOfFence = int.Parse(txtHeightOfFence.Text);
            int numberOfBlocks = int.Parse(txtNumberOfBlocks.Text);

            if (lstMethods.SelectedItem == null)
            {
                MessageBox.Show("Please select an encryption method.");
                return;
            }

            if (!chkEncrypt.IsChecked.HasValue || !chkDecrypt.IsChecked.HasValue)
            {
                MessageBox.Show("Please select encryption or decryption.");
                return;
            }

            if (!chkEncryptTopToBottom.IsChecked.HasValue || !chkEncryptBottomToTop.IsChecked.HasValue)
            {
                MessageBox.Show("Please select encryption direction.");
                return;
            }

            if (chkReadFromConsole.IsChecked == true)
            {
                inputText = txtInput.Text;
            }
            else if (chkFilePath.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputText) || !File.Exists(inputText))
                {
                    MessageBox.Show("Please enter a valid file path.");
                    return;
                }

                inputText = File.ReadAllText(filePath);
            }

            if (!int.TryParse(txtHeightOfFence.Text, out heightOfFence) || heightOfFence < 0 ||
                !int.TryParse(txtNumberOfBlocks.Text, out numberOfBlocks) || numberOfBlocks < 0 ||
                txtHeightOfFence.Text.Contains(".") || txtNumberOfBlocks.Text.Contains("."))
            {
                MessageBox.Show("Please enter valid integer values for height of fence and number of blocks.");
                return;
            }

            string result = string.Empty;
            if (lstMethods.SelectedIndex == 0)
            {
                try
                {
                    result = await RailFenceCipherEncryptDecryptBlocksAsync(inputText, heightOfFence, numberOfBlocks, encrypt, encryptTopToBottom);

                    endTime = DateTime.Now;
                    TimeSpan elapsedTime = endTime - startTime;

                    if (chkReadFromConsole.IsChecked == true)
                    {
                        txtResult.Text = $"Output:\n{result}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                    }
                    else if (chkFilePath.IsChecked == true)
                    {
                        string outputFileName = encrypt ? "output_encrypt.txt" : "output_decrypt.txt";
                        string outputPath = System.IO.Path.Combine(@"E:\C#\Mathematical cryptology", outputFileName);

                        File.WriteAllText(outputPath, result);

                        txtResult.Text = $"Output file saved to:\n{outputPath}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    return;
                }
            }
            else
            {
                MessageBox.Show("This method is currently unavailable.");
            }
        }



        private void rbEncryptBottomToTop_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rbEncryptTopToBottom_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void chkReadFromConsole_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbEncrypt_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}



























/*namespace Mathematical_cryptology
{
    public partial class RailFenceCipher : Window

    {
        public RailFenceCipher()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
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


        private void TxtHeightOfFence_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Enter height of fence" || textBox.Text == ""))
            {
                textBox.Text = "";
            }
            textBox.Foreground = Brushes.White;
        }

        private void TxtHeightOfFence_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "")
            {
                textBox.Text = "Enter height of fence";
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

        private string RailFenceCipherEncryptDecryptBlocks(string text, int rails, int blockSize, bool encrypt, bool topToBottom)
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

        private string RailFenceCipherEncrypt(string plainText, int rails, bool topToBottom)
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

        private string RailFenceCipherDecrypt(string cipherText, int rails, bool topToBottom)
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

        private void chkFilePath_Checked(object sender, RoutedEventArgs e)
        {
            if (chkFilePath.IsChecked == true)
            {
                txtInput.Text = "Enter file path"; 
            }
            else
            {
                txtInput.Text = "Enter text";
            }
        }

        private DateTime startTime;
        private DateTime endTime;

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            startTime = DateTime.Now;

            if (lstMethods.SelectedItem == null)
            {
                MessageBox.Show("Please select an encryption method.");
                return;
            }

            bool encrypt = chkEncrypt.IsChecked == true;
            bool decrypt = chkDecrypt.IsChecked == true;

            if (!encrypt && !decrypt)
            {
                MessageBox.Show("Please select encryption or decryption.");
                return;
            }

            bool encryptTopToBottom = chkEncryptTopToBottom.IsChecked == true;
            bool encryptBottomToTop = chkEncryptBottomToTop.IsChecked == true;

            if (!encryptTopToBottom && !encryptBottomToTop)
            {
                MessageBox.Show("Please select encryption direction.");
                return;
            }

            string inputText = txtInput.Text;
            bool useFilePath = chkFilePath.IsChecked == true;
            string filePath = txtInput.Text;
            int heightOfFence = int.Parse(txtHeightOfFence.Text);
            int numberOfBlocks = int.Parse(txtNumberOfBlocks.Text);

            if (lstMethods.SelectedItem == null)
            {
                MessageBox.Show("Please select an encryption method.");
                return;
            }

            if (!chkEncrypt.IsChecked.HasValue || !chkDecrypt.IsChecked.HasValue)
            {
                MessageBox.Show("Please select encryption or decryption.");
                return;
            }

            if (!chkEncryptTopToBottom.IsChecked.HasValue || !chkEncryptBottomToTop.IsChecked.HasValue)
            {
                MessageBox.Show("Please select encryption direction.");
                return;
            }

            if (chkReadFromConsole.IsChecked == true)
            {
                inputText = txtInput.Text;
            }
            else if (chkFilePath.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputText) || !File.Exists(inputText))
                {
                    MessageBox.Show("Please enter a valid file path.");
                    return;
                }

                inputText = File.ReadAllText(filePath);
            }

            if (!int.TryParse(txtHeightOfFence.Text, out heightOfFence) || heightOfFence < 0 ||
                !int.TryParse(txtNumberOfBlocks.Text, out numberOfBlocks) || numberOfBlocks < 0 ||
                txtHeightOfFence.Text.Contains(".") || txtNumberOfBlocks.Text.Contains("."))
            {
                MessageBox.Show("Please enter valid integer values for height of fence and number of blocks.");
                return;
            }

            string result = string.Empty;
            if (lstMethods.SelectedIndex == 0)
            {
                try
                {
                    result = await Task.Run(() => RailFenceCipherEncryptDecryptBlocks(inputText, heightOfFence, numberOfBlocks, encrypt, encryptTopToBottom));

                    endTime = DateTime.Now;

                    TimeSpan elapsedTime = endTime - startTime;

                    if (chkReadFromConsole.IsChecked == true)
                    {
                        txtResult.Text = $"Output:\n{result}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                    }
                    else if (chkFilePath.IsChecked == true)
                    {
                        string outputFileName = encrypt ? "output_encrypt.txt" : "output_decrypt.txt";
                        string outputPath = System.IO.Path.Combine(@"E:\C#\Mathematical cryptology", outputFileName);

                        File.WriteAllText(outputPath, result);

                        txtResult.Text = $"Output file saved to:\n{outputPath}\n\n\nElapsed Time: {elapsedTime.TotalSeconds} seconds";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    return;
                }
            }
            else
            {
                MessageBox.Show("This method is currently unavailable.");
            }
        }


        private void rbEncryptBottomToTop_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rbEncryptTopToBottom_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void chkReadFromConsole_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbEncrypt_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
*/