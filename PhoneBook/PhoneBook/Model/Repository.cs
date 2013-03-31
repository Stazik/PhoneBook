using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PhoneBook.Model
{
    /// <summary>
    /// Класс-интерфейс работы с БД
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Сохранение данных о контакте
        /// </summary>
        /// <param name="Contact">Контакт</param>
        /// <param name="PhoneBookId">Идентификатор телефонной книги</param>
        /// <param name="error">Сообщение об ошибке</param>
        /// <returns>Успешность</returns>
        public bool SaveContact(PhoneBookRecord Contact, int PhoneBookId, out string error)
        {
            error = string.Empty;
            using (var dbContext = new DataBaseContext())
            {
                try
                {
                    PhoneBookRecord record = dbContext.PhoneBookRecords.Include("Man").Include("Phones").Where(c => c.Id == Contact.Id).FirstOrDefault();
                    if (record != null)
                    {
                        //удаление из БД несуществующих телефонов
                        if (record.Phones != null && record.Phones.Count > 0)
                        {
                            for (int i = record.Phones.Count - 1; i >= 0; i--)
                            {
                                if (!Contact.Phones.Any(c => c.Id == record.Phones.ToArray()[i].Id))
                                {
                                    dbContext.Phones.Remove(record.Phones.ToArray()[i]);
                                }
                            }
                        }
                        //копирование объекта из памяти в объект БД
                        Contact.CopyTo(record);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        PhoneBook book = dbContext.PhoneBooks.Where(c => c.Id == PhoneBookId).FirstOrDefault();
                        if (book != null)
                        {
                            book.Contacts.Add(Contact);
                            dbContext.SaveChanges();
                        }
                    }
                }
                catch (EntityException exception)
                {
                    error = exception.Message;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Загружает из БД или создает при отсутвии Телефонную книгу
        /// </summary>
        /// <param name="book">Телефонная книга</param>
        /// <param name="error">Сообщение об ошибке</param>
        /// <returns>Успешность</returns>
        public bool LoadPhoneBook(out PhoneBook book, out string error)
        {
            error = string.Empty;
            book = null;
            using (var dbContext = new DataBaseContext())
            {
                try
                {
                    book = dbContext.PhoneBooks
                                                    .Include("Contacts")
                                                    .Include("Contacts.Man")
                                                    .Include("Contacts.Phones")
                                                    .Select(c => c).FirstOrDefault();
                    if (book == null)
                    {
                        book = new Model.PhoneBook();
                        book.Name = "Записная книга";
                        dbContext.PhoneBooks.Add(book);
                        dbContext.SaveChanges();
                    }
                }
                catch (EntityException exception)
                {
                    error = exception.Message;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Удаление контакта
        /// </summary>
        /// <param name="id">Идентификатор контакта</param>
        /// <param name="error">Сообщение об ошибке</param>
        /// <returns>Успешность</returns>
        public bool DeleteContact(int id, out string error)
        {
            error = string.Empty;
            using (DataBaseContext dbContext = new DataBaseContext())
            {
                try
                {
                    var records = dbContext.PhoneBookRecords
                                                            .Include("Phones")
                                                            .Include("Man")
                                                            .Where(c => c.Id == id);
                    foreach (var record in records)
                    {
                        if (record != null)
                        {
                            if (record.Man != null)
                            {
                                dbContext.People.Remove(record.Man);
                            }
                            if (record.Phones != null && record.Phones.Count > 0)
                            {
                                for (int i = record.Phones.Count - 1; i >= 0; i--)
                                {
                                    dbContext.Phones.Remove(record.Phones.ToArray()[i]);
                                }
                            }
                            dbContext.PhoneBookRecords.Remove(record);
                        }
                    }
                    dbContext.SaveChanges();
                }
                catch (EntityException exception)
                {
                    error = exception.Message;
                    return false;
                }
            }
            return true;
        }
        
    }
}
