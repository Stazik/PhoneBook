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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhoneBook.Model;
using System.Globalization;
using System.Data;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Текущая телефонная книга
        /// </summary>
        private Model.PhoneBook MyPhoneBook;

        /// <summary>
        /// Текущий контакт
        /// </summary>
        private PhoneBookRecord Contact;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = Properties.Resources.DbConnect;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPhoneBook();
            this.Title = Properties.Resources.MainWindow;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            PhoneBookRecord record = new PhoneBookRecord();
            MyPhoneBook.Contacts.Add(record);
            EditWindow window = new EditWindow(record, MyPhoneBook.Id);
            window.ShowDialog();
            LoadPhoneBook();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {                
            if (Contact != null)
            {
                if (MessageBox.Show(Properties.Resources.DeleteApprove, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Repository Repository = new Repository();
                    string error;
                    if (!Repository.DeleteContact(Contact.Id, out error))
                    {
                        MessageBox.Show(Properties.Resources.DbError + Environment.NewLine +
                                            error + Environment.NewLine + Properties.Resources.Sorry,
                                            Properties.Resources.Warning,
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                    }
                    LoadPhoneBook();
                }
            }
        }

        private void DataGridPhoneBook_CurrentCellChanged(object sender, EventArgs e)
        {
            var record = this.DataGridPhoneBook.CurrentItem as PhoneBookRecord;
            if ( record != null && record != Contact)
            {
                Contact = record;
                this.TextBoxDetail.Text = Contact.ToString();
            }
        }

        private void DataGridPhoneBook_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        /// <summary>
        /// Определение количества дней до следующего дня рождения данной телефонной книги
        /// </summary>
        /// <returns>Пустая строка, если нет записей в книге, число дней если есть</returns>
        protected string NextBirthDay(Model.PhoneBook book)
        {
            var dates = book.Contacts.Where(c => c.Man.BirhtDay.HasValue && c.Man.BirhtDay.Value > 0)
                                .Select(c => new { c.Man.BirhtDay, c.Man.BirhtMonth })
                                .OrderBy(c => c.BirhtMonth)
                                .OrderBy(c => c.BirhtDay);
            int DaysToGo = int.MaxValue;
            double days;
            DateTime TempDate;
            foreach (var date in dates)
            {
                try
                {
                    TempDate = new DateTime(DateTime.Now.Year, date.BirhtMonth.Value, date.BirhtDay.Value);
                    if (TempDate > DateTime.Now)
                    {
                        days = (TempDate - DateTime.Now).TotalDays;
                    }
                    else
                    {
                        days = (TempDate.AddYears(1) - DateTime.Now).TotalDays;
                    }

                    if (days < DaysToGo)
                    {
                        DaysToGo = (int)days;
                    }
                }
                catch(ArgumentOutOfRangeException)
                {
                }
            }
            if (DaysToGo ==  int.MaxValue)
            {
                return string.Empty;
            }
            return DaysToGo.ToString(CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// Загрузка данных из БД.
        /// Возможен перенос в класс репозиторий
        /// </summary>
        protected void LoadPhoneBook()
        {
            Repository Repository = new Repository();
            string error;
            if (!Repository.LoadPhoneBook(out MyPhoneBook, out error))
            {
                    MessageBox.Show(Properties.Resources.DbError + Environment.NewLine +
                            error + Environment.NewLine + Properties.Resources.Sorry,
                            Properties.Resources.Warning,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
            }
         
            if (MyPhoneBook != null)
            {
                this.DataGridPhoneBook.ItemsSource = MyPhoneBook.Contacts.AsEnumerable();
                Contact = MyPhoneBook.Contacts.FirstOrDefault();
                if (Contact != null)
                {
                    this.TextBoxDetail.Text = Contact.ToString();
                }
                this.TextBoxNextBirthDay.Text = String.Format(Properties.Resources.NextBirthDay, NextBirthDay(MyPhoneBook));
            }
        }        
        
        /// <summary>
        /// Редактирование\просмотр данных контакта
        /// </summary>
        protected void Edit()
        {
            if (Contact != null)
            {
                EditWindow window = new EditWindow(Contact, 0);
                window.ShowDialog();
                LoadPhoneBook();
            }
            else
            {
                MessageBox.Show(Properties.Resources.ContactNotSelect, Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
