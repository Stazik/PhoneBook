using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace PhoneBook.Model
{
    /// <summary>
    /// "Репозиторий" БД
    /// При росте приложения - такой DAL не пойдет - необходим полнофункциональный Repository\UnityOfWork и т.д.
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// Физ. лицо
        /// </summary>
        public DbSet<Man> People { get; set; }
        /// <summary>
        /// Телефоны
        /// </summary>
        public DbSet<Phone> Phones { get; set; }
        /// <summary>
        /// Телефонные книги
        /// </summary>
        public DbSet<PhoneBook> PhoneBooks { get; set; }
        /// <summary>
        /// Контакты
        /// </summary>
        public DbSet<PhoneBookRecord> PhoneBookRecords { get; set; }

        /// <summary>
        /// Создание экземпляра класса
        /// </summary>
        public DataBaseContext()
            : base(DataBaseContext.GetConnectionString())
        {

        }

        public static string GetConnectionString()
        {
            return Globals.ConnectionString();
        }

    }
}
