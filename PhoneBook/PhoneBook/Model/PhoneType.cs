using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneBook.Model
{
    /// <summary>
    /// Типы номеров телефона
    /// </summary>
    public enum PhoneType:byte
    {

        /// <summary>
        /// Мобильный
        /// </summary>
        Mobil = 1,
        /// <summary>
        /// Рабочий
        /// </summary>
        Work = 2,
        /// <summary>
        /// Домашний
        /// </summary>
        Home = 3,    
        /// <summary>
        /// Неопределенный
        /// </summary>
        Undefined = 4

    }
}
