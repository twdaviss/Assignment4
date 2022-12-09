using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
using static Assignment4.Crypto;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for KeysWindow.xaml
    /// </summary>
    public partial class KeysWindow : Window
    {
        public static Crypto crypto = new Crypto();
        public KeysWindow()
        {
            InitializeComponent();
            crypto.Reset();
        }

        

        private void Private_Import_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            dlg.Multiselect= true;
            if (dlg.FileName != "")
            {
                crypto.LoadK1(dlg.FileName);
                Next_Button2.IsEnabled = true;
            }
        }
        private void Public_Import_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                crypto.LoadK2(dlg.FileName);
                Next_Button2.IsEnabled = true;
            }
        }
        private void Private_Export_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                crypto.SaveK1(dlg.FileName);
                Next_Button2.IsEnabled = true;
            }
        }
        private void Public_Export_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML | *.xml;";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                crypto.SaveK1(dlg.FileName);
                Next_Button2.IsEnabled = true;
            }
        }

        private void Next_Button2_Click(object sender, RoutedEventArgs e)
        {
            EncryptionToolWindow encryptWindow = new EncryptionToolWindow();
            encryptWindow.Show();
            this.Close();

        }

        private void Back_Button_Clicked(object sender, RoutedEventArgs e)
        {
            SelectionWindow select = new SelectionWindow();
            select.Show();
            
            this.Close();
        }
    }
}
