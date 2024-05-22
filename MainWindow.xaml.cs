using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace encrypting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string alfabet = "abcdefghijklmnopqrstuvwxyz";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InputTextBox.Text) || string.IsNullOrWhiteSpace(ComboBox.Text))
                {
                    MessageBox.Show("please fill all the fields.");
                    return;
                }

                switch (ComboBox.Text)
                {
                    case "Caesar Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text) || !int.TryParse(KeyTextBox.Text, out int caesarKey))
                        {
                            MessageBox.Show("invalid key for Caesar Cipher. must be a number");
                            return;
                        }
                        OutputTextBox.Text = CaesarCipher(InputTextBox.Text, -1 * caesarKey);
                        break;

                    case "Vigenère Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text))
                        {
                            MessageBox.Show("invalid key for Vigenere Cipher.");
                            return;
                        }
                        OutputTextBox.Text = VinegereCipher(InputTextBox.Text, KeyTextBox.Text, 1);
                        break;

                    case "Atbash":
                        OutputTextBox.Text = AtbashCipher(InputTextBox.Text);
                        break;

                    case "Rail Fence Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text) || !int.TryParse(KeyTextBox.Text, out int railFenceKey))
                        {
                            MessageBox.Show("invalid key for Rail Fence Cipher.");
                            return;
                        }
                        OutputTextBox.Text = RailFenceEncrypt(InputTextBox.Text, railFenceKey);
                        break;

                    default:
                        MessageBox.Show("invalid cipher selection.");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}");
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InputTextBox.Text) || string.IsNullOrWhiteSpace(ComboBox.Text))
                {
                    MessageBox.Show("Please fill all the fields.");
                    return;
                }

                switch (ComboBox.Text)
                {
                    case "Caesar Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text) || !int.TryParse(KeyTextBox.Text, out int caesarKey))
                        {
                            MessageBox.Show("invalid key for Caesar Cipher.");
                            return;
                        }
                        OutputTextBox.Text = CaesarCipher(InputTextBox.Text, caesarKey);
                        break;

                    case "Vigenère Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text))
                        {
                            MessageBox.Show("invalid key for Vigenere Cipher.");
                            return;
                        }
                        OutputTextBox.Text = VinegereCipher(InputTextBox.Text, KeyTextBox.Text, -1);
                        break;

                    case "Atbash":
                        OutputTextBox.Text = AtbashCipher(InputTextBox.Text);
                        break;

                    case "Rail Fence Cipher":
                        if (string.IsNullOrWhiteSpace(KeyTextBox.Text) || !int.TryParse(KeyTextBox.Text, out int railFenceKey))
                        {
                            MessageBox.Show("invalid key for Rail Fence Cipher.");
                            return;
                        }
                        OutputTextBox.Text = RailFenceDecrypt(InputTextBox.Text, railFenceKey);
                        break;

                    default:
                        MessageBox.Show("invalid cipher selection.");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}");
            }
        }

        private string CaesarCipher(string sentenceForEncrypting, int shift)
        {
            StringBuilder result = new StringBuilder();
            
            for (int i = 0; i < sentenceForEncrypting.Length; i++)
            {
                int x = alfabet.IndexOf(Char.ToLower(sentenceForEncrypting[i]));

                if (Char.IsLetter(sentenceForEncrypting[i]))
                {
                    if (Char.IsUpper(sentenceForEncrypting[i]))
                    {

                        result.Append((alfabet[(x - shift + alfabet.Length) % alfabet.Length]));

                    }
                    else {
                        result.Append(alfabet[(x - shift + alfabet.Length) % alfabet.Length]);
                    }
                }
                else
                {
                    result.Append(sentenceForEncrypting[i]);
                }
            }
            return result.ToString();
        }

        private string AtbashCipher(string sentenceForEncrypting) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < sentenceForEncrypting.Length; i++)
            {
                int x = alfabet.IndexOf(Char.ToLower(sentenceForEncrypting[i]));

                if (Char.IsLetter(sentenceForEncrypting[i]))
                {
                    if (Char.IsUpper(sentenceForEncrypting[i]))
                    {

                        result.Append(Char.ToUpper(alfabet[(alfabet.Length - x + alfabet.Length) % alfabet.Length]));
                    }
                    else
                    {
                        result.Append(alfabet[(alfabet.Length - x + alfabet.Length) % alfabet.Length]);
                    }
                }
                else
                {
                    result.Append(sentenceForEncrypting[i]);
                }
            }
                        
            return result.ToString();
        }

        private string VinegereCipher(string sentenceForEncrypting, string key, int decrypting_or_encrypting)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToLower();
            for (int i = 0; i < sentenceForEncrypting.Length;)
            {
                for (int k = 0; k < key.Length && i < sentenceForEncrypting.Length; k++)
                {
                    char currentChar = sentenceForEncrypting[i];

                    if (Char.IsLetter(currentChar))
                    {
                        int shift = alfabet.IndexOf(key[k]);
                        int currentPosition = alfabet.IndexOf(Char.ToLower(currentChar));
                        int newPosition = (currentPosition + (decrypting_or_encrypting * shift) + alfabet.Length) % alfabet.Length;

                        char encryptedChar = alfabet[newPosition];
                        if (Char.IsUpper(currentChar))
                        {
                            encryptedChar = Char.ToUpper(encryptedChar);
                        }
                        result.Append(encryptedChar);
                    }
                    else
                    {
                        result.Append(currentChar);
                        k--;
                    }

                    i++;
                }
            }

            return result.ToString();

            /* for (int i = 0; i < sentenceForEncrypting.Length; i++)
             {
                 char ch = sentenceForEncrypting[i];

                 if (Char.IsLetter(ch))
                 {
                     for (int k = 0; k < key.Length; k++)
                     {
                         int shift = alfabet.IndexOf(Char.ToLower(key[k]));
                         int current_position = alfabet.IndexOf(Char.ToLower(ch));
                         int position_after_encrypting = (current_position + shift) % alfabet.Length;
                         result.Append(alfabet[position_after_encrypting]);
                         i++;
                     }
                 }
                 else
                 {
                     result.Append(ch);
                 }
             }

             return result.ToString();
         }*/
        }

        private string RailFenceEncrypt(string stringForEncrypting, int rows)
        {
            if (rows == 1)
            {
                return stringForEncrypting;
            }
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                int increment = 2 * (rows - 1); 

                
                for (int k = i; k < stringForEncrypting.Length; k += increment)
                {
                    result.Append(stringForEncrypting[k]); 

                    
                    if (i > 0 && i < rows - 1 && (k + increment - 2 * i) < stringForEncrypting.Length)
                    {
                        result.Append(stringForEncrypting[k + increment - 2 * i]);
                    }
                }
            }

            
            return result.ToString();
        }
        private string RailFenceDecrypt(string cipherText, int rows)
        {
            
            if (rows == 1)
            {
                return cipherText;
            }

            
            char[] result = new char[cipherText.Length];
            int k = 0; 

            
            int[] pos = new int[cipherText.Length];

            
            for (int i = 0; i < rows; i++)
            {
                int increment = 2 * (rows - 1); 
                for (int j = i; j < cipherText.Length; j += increment)
                {
                    pos[k++] = j; 
                    if (i > 0 && i < rows - 1 && (j + increment - 2 * i) < cipherText.Length)
                    {
                        pos[k++] = j + increment - 2 * i;
                    }
                }
            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                result[pos[i]] = cipherText[i];
            }
            return new string(result);
        }

    }
}
