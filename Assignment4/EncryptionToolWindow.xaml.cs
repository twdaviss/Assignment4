using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Assignment4.KeysWindow;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for EncryptionToolWindow.xaml
    /// </summary>
    public partial class EncryptionToolWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string MessageInput { get { return _messageInput; } set { _messageInput = value; OnPropertyChanged(); } }
        private string _messageInput = "";

        public string MessageOutput { get { return _messageOutput; } set { _messageOutput = value; OnPropertyChanged(); } }
        private string _messageOutput = "";

        public string CypherInput { get { return _cypherInput; } set { _cypherInput = value; OnPropertyChanged(); } }
        private string _cypherInput = "";

        public string CypherOutput { get { return _cypherOutput; } set { _cypherOutput = value; OnPropertyChanged(); } }
        private string _cypherOutput = "";

        public EncryptionToolWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Decrypt_Clicked(object sender, RoutedEventArgs e)
        {
            try {
            byte[] data = crypto.Decrypt(Convert.FromBase64String(CypherInput));
                MessageOutput = Encoding.UTF8.GetString(data);
            }
            catch(FormatException) {
                string messageBoxText = "Incorrect Format.";
                string caption = "Decryption Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            catch(ArgumentNullException)
            {
                //Already dealt with
            }
        }

        private void Encrypt_Clicked(object sender, RoutedEventArgs e)
        {
            if(MessageInput.Length %2 != 0)
            {
                MessageInput += " ";
            }
            byte[] data = Encoding.UTF8.GetBytes(MessageInput);
            CypherOutput = Convert.ToBase64String(crypto.Encrypt(data));
        }
        
        private void Save1_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                File.WriteAllText(dlg.FileName, CypherOutput);
            }
        }

        private void Save2_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                File.WriteAllText(dlg.FileName, MessageOutput);
            }
        }

        private void Load1_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                MessageInput = File.ReadAllText(dlg.FileName);
            }
        }

        private void Load2_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                CypherInput = File.ReadAllText(dlg.FileName);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void MessageInput_Changed(object sender, TextChangedEventArgs e)
        {
            if(MessageInput !="") {
            Encrypt_Button.IsEnabled= true;
            }
            else
            {
                Encrypt_Button.IsEnabled= false;
            }
        }

        private void CypherOutput_Changed(object sender, TextChangedEventArgs e)
        {
            if (CypherOutput != "")
            {
                Save_Button1.IsEnabled = true;
            }
            else
            {
                Save_Button1.IsEnabled = false;
            }
        }

        private void CypherInput_Changed(object sender, TextChangedEventArgs e)
        {
            if (CypherInput != "")
            {
                Decrypt_Button.IsEnabled = true;
            }
            else
            {
                Decrypt_Button.IsEnabled = false;
            }
        }

        private void MessageOutput_Changed(object sender, TextChangedEventArgs e)
        {
            if (MessageOutput != "")
            {
                Save_Button2.IsEnabled = true;
            }
            else
            {
                Save_Button2.IsEnabled = false;
            }
        }

        private void Back_Button_Clicked(object sender, RoutedEventArgs e)
        {
            KeysWindow keys = new KeysWindow();
            keys.Show();
            MessageInput = "";
            MessageOutput = "";
            CypherInput = "";
            CypherOutput = "";
            this.Close();
        }
    }
}
