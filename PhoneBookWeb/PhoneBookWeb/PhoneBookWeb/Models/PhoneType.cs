
namespace PhoneBookWeb.Model
{
    /// <summary>
    /// Тип номера телефона
    /// </summary>
    public enum PhoneType: byte
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
        Other = 0
    }
}
