using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookWeb.Model
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
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "PhoneNumbers")]
        public ICollection<Phone> Phones { get; set; }
        
        /// <summary>
        /// Примечание
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "Note")]
        public string Note { get; set; }

        /// <summary>
        /// Создание экземпляра контакта
        /// </summary>
        public PhoneBookRecord()
        {
            Phones = new List<Phone>();
            Man = new Man();
        }
    }
}
