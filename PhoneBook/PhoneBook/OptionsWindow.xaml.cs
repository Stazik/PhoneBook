using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            this.TextBoxConnectionString.Text = PhoneBook.Model.Globals.ServerName;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            PhoneBook.Model.Globals.ServerName = this.TextBoxConnectionString.Text;
            MainWindow wnd = new MainWindow();
            wnd.Show();
            this.Close();
        } 
    }
}
