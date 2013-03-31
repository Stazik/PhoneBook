using System.ComponentModel.DataAnnotations;

namespace PhoneBookWeb.Model
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
