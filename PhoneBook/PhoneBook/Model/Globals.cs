using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneBook.Model
{
    /// <summary>
    /// Глобальные настройки
    /// </summary>
    public static class Globals
    {
        private static string _ServerName = @"localhost\SQLEXPRESS";
        
        /// <summary>
        /// Имя сервера БД
        /// </summary>
        public static string ServerName
        {
          get { return Globals._ServerName; }
          set { Globals._ServerName = value; }
        }

        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            return @"Server=" + ServerName + ";Database=PhoneBook;Integrated Security=True;MultipleActiveResultSets=True;";
        }

    }
}
