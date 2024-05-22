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
            switch (ComboBox.Text)
            {
                case "Caesar Cipher":
                    OutputTextBox.Text = CaesarCipher(InputTextBox.Text, -1 * (Convert.ToInt16(KeyTextBox.Text)));
                    break;
                case "Vigenère Cipher":
                    OutputTextBox.Text=VinegereCipher(InputTextBox.Text, KeyTextBox.Text,1);
                    break;
                case "Atbash":
                    OutputTextBox.Text = AtbashCipher(InputTextBox.Text);
                    break;
                case "Rail Fence Cipher":
                    OutputTextBox.Text = RailFenceEncrypt(InputTextBox.Text, Convert.ToInt16(KeyTextBox.Text));
                    break;
                default:
                    break;
            }

        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBox.Text)
            {
                case "Caesar Cipher":
                    OutputTextBox.Text = CaesarCipher(InputTextBox.Text, (Convert.ToInt16(KeyTextBox.Text))); break;
                case "Vigenère Cipher":
                    OutputTextBox.Text= VinegereCipher(InputTextBox.Text, KeyTextBox.Text,-1);
                    break;
                case "Atbash":
                    OutputTextBox.Text = AtbashCipher(InputTextBox.Text);
                    break;
                case "Rail Fence Cipher":
                    OutputTextBox.Text =  RailFenceDecrypt(InputTextBox.Text, Convert.ToInt16(KeyTextBox.Text));
                    break;
                default:
                    break;
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
            // Якщо кількість рядків рівна 1, повертаємо вихідний рядок
            if (rows == 1)
            {
                return stringForEncrypting;
            }

            // Створюємо StringBuilder для результату
            StringBuilder result = new StringBuilder();

            // Проходимося по кожному рядку "забору"
            for (int i = 0; i < rows; i++)
            {
                int increment = 2 * (rows - 1); // Інкремент для переміщення між символами

                // Ітерація через всі символи в рядку
                for (int k = i; k < stringForEncrypting.Length; k += increment)
                {
                    result.Append(stringForEncrypting[k]); // Додаємо символ до результату

                    // Додаємо додатковий символ для проміжних рядків
                    if (i > 0 && i < rows - 1 && (k + increment - 2 * i) < stringForEncrypting.Length)
                    {
                        result.Append(stringForEncrypting[k + increment - 2 * i]);
                    }
                }
            }

            // Повертаємо зашифрований текст
            return result.ToString();
        }
        private string RailFenceDecrypt(string cipherText, int rows)
        {
            // Якщо кількість рядків рівна 1, повертаємо вихідний рядок
            if (rows == 1)
            {
                return cipherText;
            }

            // Створюємо масив для збереження розшифрованого тексту
            char[] result = new char[cipherText.Length];
            int k = 0; // Індекс для відслідковування позицій у зашифрованому тексті

            // Масив для збереження позицій символів у зашифрованому тексті
            int[] pos = new int[cipherText.Length];

            // Проходимося по кожному рядку "забору"
            for (int i = 0; i < rows; i++)
            {
                int increment = 2 * (rows - 1); // Інкремент для переміщення між символами
                for (int j = i; j < cipherText.Length; j += increment)
                {
                    pos[k++] = j; // Записуємо позиції символів поточного рядка
                    if (i > 0 && i < rows - 1 && (j + increment - 2 * i) < cipherText.Length)
                    {
                        pos[k++] = j + increment - 2 * i; // Додаємо додаткові позиції для проміжних рядків
                    }
                }
            }

            // Записуємо символи з зашифрованого тексту у відповідні позиції у результат
            for (int i = 0; i < cipherText.Length; i++)
            {
                result[pos[i]] = cipherText[i];
            }

            // Повертаємо розшифрований текст як рядок
            return new string(result);
        }

    }
}
