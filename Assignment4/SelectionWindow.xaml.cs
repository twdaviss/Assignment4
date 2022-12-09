using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
        }

        private void Radio_Button_Changed(object sender, RoutedEventArgs e)
        {
            Next_Button.IsEnabled = true;
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            if (RSA_Button.IsChecked == true)
            {
                SetMode(true);
            }
            if (Aes_Button.IsChecked == true)
            {
                SetMode(false);
            }
            KeysWindow keyWindow = new KeysWindow();
            keyWindow.Show();
            this.Close();
        }
    }
}
