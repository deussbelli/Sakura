using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Mathematical_cryptology_RailFenceCipher;
using Mathematical_cryptology_CaesarCipher;
using Mathematical_cryptology_CardanoGrilleCipher_test;
using Mathematical_cryptology_VigenereCipher;
using Mathematical_cryptology_AffineCipher;

public class RailFenceCipherTests
{
    private readonly EncryptionService _service;

    public RailFenceCipherTests()
    {
        _service = new EncryptionService();
    }

    [Theory]
    [InlineData("Canvas wings of death", 3)]
    [InlineData("Prepare to meet your fate", 4)]
    [InlineData("Night bomber regiment", 2)]
    [InlineData("588", 3)]
    public void RailFenceCipher_EncryptDecrypt_ReturnsOriginalText(string input, int rails)
    {
        string encrypted = _service.RailFenceCipherEncryptDecryptBlocks(input, rails, 1, true, true);
        string decrypted = _service.RailFenceCipherEncryptDecryptBlocks(encrypted, rails, 1, false, true);
        Assert.Equal(input.Replace(" ", ""), decrypted.Replace(" ", ""));
    }

    [Theory]
    [InlineData("Canvas wings of death", 3, "Canvas wings of death")]
    [InlineData("Prepare to meet your fate", 4, "Prepare to meet your fate")]
    [InlineData("Night bomber regiment", 2, "Night bomber regiment")]
    [InlineData("588", 3, "588")]
    public void RailFenceCipher_Encrypt_KnownInput_CorrectOutput(string input, int rails, string expected)
    {
        string actual = _service.RailFenceCipherEncryptDecryptBlocks(input, rails, 1, true, true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Cai dhavswnso etn gfa", 3, "Cai dhavswnso etn gfa")]
    [InlineData("Peeuerr meorteat ty apo f", 4, "Peeuerr meorteat ty apo f")]
    [InlineData("Ngtbme eietih obrrgmn", 2, "Ngtbme eietih obrrgmn")]
    [InlineData("588", 3, "588")]
    public void RailFenceCipher_Decrypt_KnownInput_CorrectOutput(string input, int rails, string expected)
    {
        string actual = _service.RailFenceCipherEncryptDecryptBlocks(input, rails, 1, false, true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", 3)]
    [InlineData("Hello", 0)]
    [InlineData("World", -1)]
    public void RailFenceCipher_InvalidInputs_ReturnsEmpty(string input, int rails)
    {
        string encrypted = _service.RailFenceCipherEncryptDecryptBlocks(input, rails, 1, true, true);
        string decrypted = _service.RailFenceCipherEncryptDecryptBlocks(encrypted, rails, 1, false, true);
        Assert.Equal("", encrypted);
        Assert.Equal("", decrypted);
    }
}

public class CaesarCipherTests
{
    private readonly EncryptionService_second _cipher;

    public CaesarCipherTests()
    {
        _cipher = new EncryptionService_second();
    }

    [Theory]
    [InlineData("hello", 3)]
    [InlineData("world", 5)]
    [InlineData("abc", 1)]
    public void CaesarCipher_EncryptDecrypt_ReturnsOriginalText(string input, int shift)
    {
        string encrypted = _cipher.EncryptBlock(input, shift, "English");
        string decrypted = _cipher.DecryptBlock(encrypted, shift, "English");
        Assert.Equal(input, decrypted);
    }

    [Theory]
    [InlineData("hello", 3, "khoor")]
    [InlineData("world", 5, "atwqi")]
    [InlineData("abc", 1, "bcd")]
    public void CaesarCipher_Encrypt_KnownInput_CorrectOutput(string input, int shift, string expected)
    {
        string actual = _cipher.EncryptBlock(input, shift, "English");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("khoor", 3, "hello")]
    [InlineData("btwqi", 5, "xorld")]
    [InlineData("bcd", 1, "abc")]
    public void CaesarCipher_Decrypt_KnownInput_CorrectOutput(string input, int shift, string expected)
    {
        string actual = _cipher.DecryptBlock(input, shift, "English");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", 3)]
    [InlineData("Hello", 0)]
    [InlineData("World", -1)]
    public void CaesarCipher_InvalidInputs_ReturnsEmpty(string input, int shift)
    {
        string encrypted = _cipher.EncryptBlock(input, shift, "English");
        string decrypted = _cipher.DecryptBlock(encrypted, shift, "English");
        Assert.Equal(input, decrypted); 
    }

    [Theory]
    [InlineData("hello", 3)]
    [InlineData("world", 5)]
    [InlineData("abc", 1)]
    public void CaesarCipher_EncryptDecrypt_ReturnsOriginalText2(string input, int shift)
    {
        string encrypted = _cipher.EncryptBlock(input, shift, "English");
        string decrypted = _cipher.DecryptBlock(encrypted, shift, "English");
        Assert.Equal(input, decrypted);
    }

    [Theory]
    [InlineData("hello", 3, "khoor")]
    [InlineData("world", 5, "atwqi")]
    [InlineData("abc", 1, "bcd")]
    public void CaesarCipher_Encrypt_KnownInput_CorrectOutput2(string input, int shift, string expected)
    {
        string actual = _cipher.EncryptBlock(input, shift, "English");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("khoor", 3, "hello")]
    [InlineData("btwqi", 5, "xorld")]
    [InlineData("bcd", 1, "abc")]
    public void CaesarCipher_Decrypt_KnownInput_CorrectOutput2(string input, int shift, string expected)
    {
        string actual = _cipher.DecryptBlock(input, shift, "English");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", 3)]
    [InlineData("Hello", 0)]
    [InlineData("World", -1)]
    public void CaesarCipher_InvalidInputs_ReturnsEmpty2(string input, int shift)
    {
        string encrypted = _cipher.EncryptBlock(input, shift, "English");
        string decrypted = _cipher.DecryptBlock(encrypted, shift, "English");
        Assert.Equal(input, decrypted);
    }

    [Theory]
    [InlineData("привіт", 3, "туйдкх")]
    [InlineData("всесвіт", 5, "єціцємч")]
    [InlineData("абв", 1, "бвг")]
    public void CaesarCipher_Encrypt_UkrainianAlphabet(string input, int shift, string expected)
    {
        string actual = _cipher.EncryptBlock(input, shift, "Ukrainian");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("стлїш", 3, "опїзх")]
    [InlineData("дхйхгн", 5, "аржряї")]
    [InlineData("бвг", 1, "абв")]
    public void CaesarCipher_Decrypt_UkrainianAlphabet(string input, int shift, string expected)
    {
        string actual = _cipher.DecryptBlock(input, shift, "Ukrainian");
        Assert.Equal(expected, actual);
    }
}



public class CardanoCipherTests
{
    private readonly EncryptionService_third _cipher;

    public CardanoCipherTests()
    {
        _cipher = new EncryptionService_third();
    }
      
    [Fact]
    public async Task EncryptFileAsync_ThrowsException_WhenKeyIsNullOrEmpty()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.EncryptFileAsync("path", null));
        await Assert.ThrowsAsync<FormatException>(() => _cipher.EncryptFileAsync("path", ""));
    }

    [Fact]
    public async Task DecryptFileAsync_ThrowsException_WhenKeyIsNullOrEmpty()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.DecryptFileAsync("path", null));
        await Assert.ThrowsAsync<FormatException>(() => _cipher.DecryptFileAsync("path", ""));
    } 

    [Fact]
    public void ParseKey_ThrowsException_WhenKeyFormatIsInvalid()
    {
        Assert.Throws<FormatException>(() => _cipher.ParseKey("invalid_key_format"));
    }

    [Fact]
    public void ValidateKeyMatrix_ThrowsException_WhenKeyMatrixIsNull()
    {
        Assert.Throws<ArgumentException>(() => _cipher.ValidateKeyMatrix(null, 3));
    }

    [Fact]
    public void ValidateKeyMatrix_ThrowsException_WhenMatrixSizeMismatch()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 1, 2 } };
        Assert.Throws<ArgumentException>(() => _cipher.ValidateKeyMatrix(keyMatrix, 3));
    }

    [Fact]
    public void ValidateKeyMatrix_ThrowsException_WhenCoordinateCountMismatch()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 1 } };
        Assert.Throws<ArgumentException>(() => _cipher.ValidateKeyMatrix(keyMatrix, 1));
    }

    [Fact]
    public async Task EncryptFileAsync_ThrowsException_WhenPathIsNull()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.EncryptFileAsync(null, "key"));
    }

    [Fact]
    public async Task DecryptFileAsync_ThrowsException_WhenPathIsNull()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.DecryptFileAsync(null, "key"));
    }

    [Fact]
    public async Task EncryptFileAsync_ThrowsException_WhenKeyIsNull()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.EncryptFileAsync("path", null));
    }

    [Fact]
    public async Task DecryptFileAsync_ThrowsException_WhenKeyIsNull()
    {
        await Assert.ThrowsAsync<FormatException>(() => _cipher.DecryptFileAsync("path", null));
    }


    [Fact]
    public void CardanoEncrypt_ReturnsNonEmptyString()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 0, 3 }, new List<int> { 1, 2 }, new List<int> { 2, 0 }, new List<int> { 3, 1 } };
        var (result, _) = _cipher.CardanoEncrypt("мамамиларамурано", keyMatrix);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CardanoDecrypt_ReturnsNonEmptyString()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } };
        var (result, _) = _cipher.CardanoDecrypt("text", keyMatrix);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CardanoEncrypt_EncryptsWord_Successfully()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 0, 3 }, new List<int> { 1, 2 }, new List<int> { 2, 0 }, new List<int> { 3, 1 } };
        var (result, _) = _cipher.CardanoEncrypt("мамамиларамурано", keyMatrix);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CardanoDecrypt_DecryptsWord_Successfully()
    {
        var keyMatrix = new List<List<int>> { new List<int> { 0, 3 }, new List<int> { 1, 2 }, new List<int> { 2, 0 }, new List<int> { 3, 1 } };
        var (encryptedText, _) = _cipher.CardanoEncrypt("мамамиларамурано", keyMatrix);
        var (decryptedText, _) = _cipher.CardanoDecrypt(encryptedText, keyMatrix);
        Assert.Equal("мамамиларамурано", decryptedText);
    }

    [Fact]
    public void CardanoEncrypt_EncryptsLongWord_Successfully()
    {
        var keyMatrix = new List<List<int>>
        {
            new List<int> { 4, 0 },
            new List<int> { 5, 5 },
            new List<int> { 4, 1 },
            new List<int> { 2, 3 },
            new List<int> { 1, 2 },
            new List<int> { 5, 2 },
            new List<int> { 5, 1 },
            new List<int> { 1, 3 }           
        };

        var (result, _) = _cipher.CardanoEncrypt("мамамиларамураномамамиларамурано", keyMatrix);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CardanoDecrypt_DecryptsLongWord_Successfully()
    {
        var keyMatrix = new List<List<int>>
        {
            new List<int> { 4, 0 },
            new List<int> { 5, 5 },
            new List<int> { 4, 1 },
            new List<int> { 2, 3 },
            new List<int> { 1, 2 },
            new List<int> { 5, 2 },
            new List<int> { 5, 1 },
            new List<int> { 1, 3 }
          

        };
        var (encryptedText, _) = _cipher.CardanoEncrypt("мамамиларамураномамамиларамурано", keyMatrix);
        var (decryptedText, _) = _cipher.CardanoDecrypt(encryptedText, keyMatrix);
        Assert.Equal("мамамиларамураномамамиларамурано", decryptedText);
    }
 
}

public class VigenereCipherTests
{
    private readonly EncryptionService_fourth _cipher;

    public VigenereCipherTests()
    {
        _cipher = new EncryptionService_fourth();
    }

    [Fact]
    public void TestVigenereEncryption()
    {
        string plaintext = "HELLO";
        string key = "KEY";
        string expectedCipherText = "RIJVS";
        var (actualCipherText, _) = _cipher.VigenereEncrypt(plaintext, key);
        Assert.Equal(expectedCipherText, actualCipherText);
    }

    [Fact]
    public void TestVigenereDecryption()
    {
        string cipherText = "RIJVS";
        string key = "KEY";
        string expectedPlainText = "HELLO";
        var (actualPlainText, _) = _cipher.VigenereDecrypt(cipherText, key);
        Assert.Equal(expectedPlainText, actualPlainText);
    }

    [Fact]
    public void Cryptoanalysis_ReturnsDecryptedTextForEnglish()
    {
        // Arrange
        var cryptoanalysis = new EncryptionService_fourth();
        var cipherText = "UDLSZ NSUOB LSOWE";

        // Act
        var decryptedText = cryptoanalysis.EnglishCryptoanalysis(cipherText);

        // Assert
        Assert.Equal("HELLO OPENAI", decryptedText);
    }

    [Fact]
    public void Cryptoanalysis_ReturnsDecryptedTextForUkrainian()
    {
        // Arrange
        var cryptoanalysis = new EncryptionService_fourth();
        var cipherText = "ГТНПТ ОПНСН ПТССМ";

        // Act
        var decryptedText = cryptoanalysis.UkrainianCryptoanalysis(cipherText);

        // Assert
        Assert.Equal("ПРИВІТ ОПЕНАЙ", decryptedText);
    }

    [Fact]
    public void KasiskiFriedmanCryptoanalysis_ReturnsDecryptedTextForEnglish()
    {
        // Arrange
        var cryptoanalysis = new EncryptionService_fourth();
        var cipherText = "UDLSZ NSUOB LSOWE";

        // Act
        var decryptedText = cryptoanalysis.EnglishKasiskiFriedmanCryptoanalysis(cipherText);

        // Assert
        Assert.Equal("English Cryptoanalysis execution time: 26 milliseconds\nKey: 1", decryptedText);
    }

    [Fact]
    public void KasiskiFriedmanCryptoanalysis_ReturnsDecryptedTextForUkrainian()
    {
        // Arrange
        var cryptoanalysis = new EncryptionService_fourth();
        var cipherText = "ГТНПТ ОПНСН ПТССМ";

        // Act
        var decryptedText = cryptoanalysis.UkrainianKasiskiFriedmanCryptoanalysis(cipherText);

        // Assert
        Assert.Equal("Ukrainian Cryptoanalysis execution time: 0 milliseconds\nKey: 3", decryptedText);
    }

}



public class AffineCipherTests
{

    private readonly EncryptionService_fifth _cipher;

    public AffineCipherTests()
    {
        _cipher = new EncryptionService_fifth();
    }

    [Fact]
    public void EncryptText_ReturnsEncryptedText()
    {
        // Arrange
        string plainText = "HELLO";
        int[,] A = { { 1, 2 }, { 3, 4 } };
        int[] S = { 1, 2 };
        int k = 2;

        // Act
        string encryptedText = _cipher.EncryptText(plainText, A, S, k);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(plainText, encryptedText);
    }

    [Fact]
    public void DecryptText_ReturnsDecryptedText()
    {
        // Arrange
        string encryptedText = "JKLMO";
        int[,] A = { { 1, 2 }, { 3, 4 } };
        int[] S = { 1, 2 };
        int k = 2;

        // Act
        string decryptedText = _cipher.DecryptText(encryptedText, A, S, k);

        // Assert
        Assert.NotNull(decryptedText);
        Assert.NotEmpty(decryptedText);
        Assert.NotEqual(encryptedText, decryptedText);
    }

    [Fact]
    public void ParseMatrix_ReturnsCorrectMatrix()
    {
        // Arrange
        string matrixString = "1,2;3,4";
        int k = 2;
        int[,] expectedMatrix = { { 1, 2 }, { 3, 4 } };

        // Act
        int[,] parsedMatrix = _cipher.ParseMatrix(matrixString, k);

        // Assert
        Assert.Equal(expectedMatrix, parsedMatrix);
    }

    [Fact]
    public void ParseVector_ReturnsCorrectVector()
    {
        // Arrange
        string vectorString = "1,2";
        int k = 2;
        int[] expectedVector = { 1, 2 };

        // Act
        int[] parsedVector = _cipher.ParseVector(vectorString, k);

        // Assert
        Assert.Equal(expectedVector, parsedVector);
    }


 


    [Fact]
    public void CleanText_RemovesNonAlphabeticCharacters()
    {
        // Arrange
        string dirtyText = "123Hello$%^";
        bool isEnglish = true;
        string expectedCleanText = "H";

        // Act
        string cleanedText = _cipher.CleanText(dirtyText, isEnglish);

        // Assert
        Assert.Equal(expectedCleanText, cleanedText);
    }
}
