using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Model
{
    /// <summary>
    /// Базовый класс для БД
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
