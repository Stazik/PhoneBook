using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Model
{
    /// <summary>
    /// Контакт телефонной книги
    /// </summary>
    public class PhoneBookRecord: Entity
    {
        /// <summary>
        /// физ. лицо
        /// </summary>
        public Man Man { get; set; }
        
        /// <summary>
        /// Номера телефонов
        /// </summary>
        public ICollection<Phone> Phones { get; set; }
        
        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Создание экземпляра контакта
        /// </summary>
        public PhoneBookRecord()
        {
            Phones = new List<Phone>();
            Man = new Man();
        }

        /// <summary>
        /// Проверка корректности Контакта
        /// </summary>
        /// <param name="record">Контакт</param>
        /// <param name="error">Сообщение об ошибке</param>
        /// <returns>Успешность</returns>
        public static bool Validate(PhoneBookRecord record, out string error, out string warning)
        {
            error = Properties.Resources.FieldIsEmpty;
            warning = string.Empty;
            StringBuilder str = new StringBuilder();
            bool success = true;

            if (String.IsNullOrWhiteSpace(record.Man.LastName))
            {
                str.AppendLine(String.Format(error, Properties.Resources.LastName));
                success = false;
            }
            if (String.IsNullOrWhiteSpace(record.Man.FirstName))
            {
                str.AppendLine(String.Format(error, Properties.Resources.FirstName));
                success = false;
            }

            //Дату рождения должно быть можно задать в одном из трех вариантов:
            // Дата полностью;
            //Только год (без дня и месяца);
            //День и месяц (без года);
            int day = 0;
            if (record.Man.BirhtDay.HasValue)
            {
                day = record.Man.BirhtDay.Value;
            }
            int month = 0;
            if (record.Man.BirhtMonth.HasValue)
            {
                month = record.Man.BirhtMonth.Value;
            }
            int year = 0;
            if (record.Man.BirhtYear.HasValue)
            {
                year = record.Man.BirhtYear.Value;
            }

            if (year <= 0)
            {
                if (day <= 0 || month <= 0)
                {
                    str.AppendLine(String.Format(error, Properties.Resources.Birthday));
                    success = false;
                }
                else
                {
                    try
                    {
                        DateTime date = new DateTime(DateTime.Now.Year, month, day);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        warning = Properties.Resources.BirthDayMayBeNoCorrect;
                    }
                }

            }

            if (record.Phones.Count < 1)
            {
                str.AppendLine(String.Format(error, Properties.Resources.PhoneNumbers));
                success = false;
            }
            else
            {
                //Должен быть заполнен как минимум один из двух телефонов [мобильный, рабочий] (ну или оба).
                var phones = record.Phones.Where(c => c.TypeName == Properties.Resources.WorkNumber || c.TypeName == Properties.Resources.MobilNumber).ToList();
                if (phones == null || phones.Count <= 0)
                {
                    str.AppendLine(String.Format(error, Properties.Resources.PhoneNumbers) + " " + Properties.Resources.PhoneInCorrect);
                    success = false;
                }
            }

            error = str.ToString();
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine(Properties.Resources.ContactInfo);

            info.AppendLine(Properties.Resources.Name + Properties.Resources.Separator + this.Man.Name());
            info.AppendLine(Properties.Resources.Birthday + Properties.Resources.Separator + this.Man.BirthDate);

            info.AppendLine(Properties.Resources.PhoneNumbers + Properties.Resources.Separator);
            foreach (Phone phone in this.Phones)
            {
                info.AppendLine(phone.ToString());
            }
            info.AppendLine(Properties.Resources.Note + Properties.Resources.Separator + this.Note);

            return info.ToString();
        }

        /// <summary>
        /// Копирование значимых полей объекта в объект copy 
        /// </summary>
        /// <param name="copy"></param>
        public void CopyTo(PhoneBookRecord copy)
        {
            if (this != null && copy != null)
            {
                this.Man.CopyTo(copy.Man);
                copy.Note = this.Note;

                //Удаленные номера телефонов
                if (copy.Phones != null && copy.Phones.Count > 0)
                {
                    for (int i = copy.Phones.Count - 1; i >= 0; i--)
                    {
                        if (!this.Phones.Any(c => c.Id == copy.Phones.ToArray()[i].Id))
                        {
                            copy.Phones.Remove(copy.Phones.ToArray()[i]);
                        }
                    }
                }
                //
                Phone FindPhone;
                foreach (Phone phone in this.Phones)
                {
                    FindPhone = copy.Phones.Where(c => c.Id == phone.Id).FirstOrDefault();
                    //существующий
                    if (FindPhone != null)
                    {
                        phone.CopyTo(FindPhone);
                    }
                    //новый
                    else
                    {
                        copy.Phones.Add(phone);
                    }
                }

            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
