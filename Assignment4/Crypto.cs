using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Documents;

namespace Assignment4
{
    public class Crypto
    {
        public enum CryptoAlgorithm { RSA, AES };
        private Aes aes;
        private RSA rsa;
        static CryptoAlgorithm mode;


        public static void SetMode(bool radio)
        {
            switch (radio) {
                case true:
                    mode =CryptoAlgorithm.RSA;
                    break;
                case false:
                    mode = CryptoAlgorithm.AES;
                    break;
            }
        }

        public void Initialize()
        {
            var random = RandomNumberGenerator.Create();
            switch (mode)
            {
                case CryptoAlgorithm.AES:
                    aes = Aes.Create();
                    aes.KeySize = 256;
                    break;
                case CryptoAlgorithm.RSA:
                    rsa = RSA.Create();
                    break;
            }
        }
        public void Reset()
        {
            if (aes != null)
            {
                aes.Clear();
            }
            else if (rsa != null)
            {
                rsa.Clear();
            }
        }
        public void SaveK1(string path) ///RSA: Saves Private Key| Aes: Saves Shared Key
        {
            Initialize();
            switch(mode) { 
            case CryptoAlgorithm.RSA:
                    File.WriteAllText(path,rsa.ToXmlString(true));
                    break;
                case CryptoAlgorithm.AES:
                    File.WriteAllBytes(path,aes.Key);
                    break;
            }
        }
        public void SaveK2(string path) ///RSA: Saves Public Key| Aes: Saves IV
        {
            Initialize();
            switch (mode)
            {
                case CryptoAlgorithm.RSA:
                    File.WriteAllText(path, rsa.ToXmlString(false));
                    break;
                case CryptoAlgorithm.AES:
                    File.WriteAllBytes(path, aes.IV);
                    break;
            }

        }
        public void LoadK1(string path)
        /// RSA: Loads both Keys| Aes: Loads Shared Key
        {
            try
            {
                switch (mode)
                {
                    case CryptoAlgorithm.RSA:
                        rsa = new RSACryptoServiceProvider();
                        string key = File.ReadAllText(path);
                        rsa.FromXmlString(key);
                        break;
                    case CryptoAlgorithm.AES:
                        Initialize();
                        aes.KeySize = 256;
                        aes.Key = File.ReadAllBytes(path);
                        break;
                }
            }
            catch (CryptographicException)
            {
                string messageBoxText = "Incorrect Format.";
                string caption = "Importing Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                MessageBox.Show(messageBoxText, caption, button, icon);

            }
        }
        public void LoadK2(string path) ///RSA: Loads Public Key| Aes: Loads IV
        {
            try {
                switch (mode)
                {
                    case CryptoAlgorithm.RSA:
                        rsa = new RSACryptoServiceProvider();
                        string keys = File.ReadAllText(path);
                        rsa.FromXmlString(keys);
                        break;
                    case CryptoAlgorithm.AES:
                        Initialize();
                        aes.IV = File.ReadAllBytes(path);
                        break;
                }
            }
            catch (CryptographicException)
            {
                string messageBoxText = "Incorrect Format.";
                string caption = "Importing Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                MessageBox.Show(messageBoxText, caption, button, icon);

            }
}
        public byte[] Encrypt(byte[] input) ///Encrypts bytes
        {
            if (mode == CryptoAlgorithm.RSA) {
                return rsa.Encrypt(input, RSAEncryptionPadding.OaepSHA1);
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    aes.Padding = PaddingMode.PKCS7;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytes = aes.Key;
                        cryptoStream.Write(input, 0, input.Length);
                    }                    
                    return memoryStream.ToArray();
                }
            }
        }
        public byte[] Decrypt(byte[] input) { ///Decrypts bytes
            if (mode == CryptoAlgorithm.RSA)
            {
                rsa.ExportParameters(false);
                try
                {
                    byte[] plainbytes = rsa.Decrypt(input, RSAEncryptionPadding.OaepSHA1);
                    return plainbytes;
                }
                catch (CryptographicException)
                {
                    string messageBoxText = "Incorrect Parameters. Likely wrong key or wrong mode.";
                    string caption = "Decryption Error";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBoxResult result;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                    return null;
                }
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    aes.Padding = PaddingMode.PKCS7;
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytes = (aes.Key);
                        cryptoStream.Write(input, 0, input.Length);
                    }
                    return memoryStream.ToArray();
                }
            }



        }
    }
}
