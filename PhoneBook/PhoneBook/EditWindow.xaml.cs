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
using System.Data;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        /// <summary>
        /// Контакт
        /// </summary>
        private PhoneBookRecord Contact { get; set; }
        /// <summary>
        /// Идентификатор телефонной книги
        /// </summary>
        private int PhoneBookId { get; set; }

        protected EditWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создает окно редактирования\просмотра контактных данных
        /// </summary>
        /// <param name="record">Контакт</param>
        /// <param name="phoneBookId">Идентификатор телефонной книги</param>
        public EditWindow(PhoneBookRecord record, int phoneBookId):this()
        {
            Contact = record;
            PhoneBookId = phoneBookId;

            this.Title = (Contact.Man != null && !string.IsNullOrWhiteSpace(Contact.Man.Name()))
                            ? String.Format(Properties.Resources.EditWindowWithName, Contact.Man.Name())
                            : Properties.Resources.EditWindowNew;

            this.DataContext = Contact;
            this.ListBoxPhone.SelectedIndex = 0;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!Save(out error))
            {
                MessageBox.Show(error, Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                this.Close();
            }

        }

        private void DatePickerBirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = this.DatePickerBirthDate.SelectedDate;
            if (date.HasValue)
            {
                this.TextBoxBirthDay.Text = date.Value.Day.ToString();
                this.TextBoxBirthMonth.Text = date.Value.Month.ToString();
                this.TextBoxBirhtYear.Text = date.Value.Year.ToString();
            }
            else
            {
                this.TextBoxBirthDay.Clear();
                this.TextBoxBirthMonth.Clear();
                this.TextBoxBirhtYear.Clear();
            }
        }

        private void DatePickerBirthDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            DateTime date;
            if (DateTime.TryParse(Contact.Man.BirthDate, out date))
            {
                this.DatePickerBirthDate.SelectedDate = date;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PhoneEdit();
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            if (this.ListBoxPhone.SelectedItem != null)
            {
                PhoneEdit(this.ListBoxPhone.SelectedItem as Phone);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.ListBoxPhone.SelectedItem != null)
            {
                Phone Phone = this.ListBoxPhone.SelectedItem as Phone;
                if (Phone != null)
                {
                    if (MessageBox.Show(Properties.Resources.PhoneDeleteApprove, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        this.Contact.Phones.Remove(Phone);
                        ListBoxPhone.Items.Refresh();
                    }
                }
            }
        }
       
        /// <summary>
        /// Редактирование номера телефона
        /// </summary>
        /// <param name="phone"></param>
        protected void PhoneEdit(Phone phone = null)
        {
            PhoneWindow wnd = new PhoneWindow(phone);
            bool? result = wnd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (phone != null)
                {
                    Phone PhoneSource = this.Contact.Phones.Where(c => c.Id == wnd.Phone.Id).FirstOrDefault();
                    PhoneSource.Number = wnd.Phone.Number;
                    PhoneSource.TypeName = wnd.Phone.TypeName;
                }
                else
                {
                    this.Contact.Phones.Add(wnd.Phone);
                }
                ListBoxPhone.Items.Refresh();
            }
        }

        /// <summary>
        /// Сохранение данных с формы
        /// </summary>
        protected bool Save(out string error)
        {
            string warning;
            if (!PhoneBookRecord.Validate(Contact, out error, out warning))
            {
                return false;
            }
            //Провверяем есть ли предупреждения
            if (!string.IsNullOrWhiteSpace(warning))
            {
                MessageBox.Show(warning, Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Repository Repository = new Repository();
            if (!Repository.SaveContact(Contact, PhoneBookId, out error))
            {
                MessageBox.Show(Properties.Resources.DbError + Environment.NewLine +
                        error + Environment.NewLine + Properties.Resources.Sorry,
                        Properties.Resources.Warning,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            return true;
        }


    }
}
