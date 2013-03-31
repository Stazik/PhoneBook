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
using PhoneBook.Model;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for PhoneWindow.xaml
    /// </summary>
    public partial class PhoneWindow : Window
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        public Phone Phone;

        protected PhoneWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создает форму для редактирования номера телефона
        /// </summary>
        /// <param name="phone">Номер телефона для редактирования</param>
        public PhoneWindow(Phone phone = null):this()
        {

            Phone = (phone == null) 
                        ? new Phone()
                        : phone;
            this.comboBoxTypes.ItemsSource = Model.Phone.TypesList();
            this.DataContext = Phone;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!Phone.Validate(Phone, out error))
            {
                MessageBox.Show(error, Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!this.DialogResult.HasValue || !this.DialogResult.Value)
            {
                this.Phone = null;
            }
        }
    }
}
