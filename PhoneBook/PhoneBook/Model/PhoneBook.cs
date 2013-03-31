using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Model
{
    /// <summary>
    /// Телефонная книга
    /// </summary>
    public class PhoneBook  : Entity
    {
        /// <summary>
        /// Наименование телефонной книги
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список контактов
        /// </summary>
        public ICollection<PhoneBookRecord> Contacts { get; set; }
        
        /// <summary>
        /// Создание экземпляра телефонной книги
        /// </summary>
        public PhoneBook()
        {
            Contacts = new List<PhoneBookRecord>();
        }
    }
}
