
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace PhoneBookWeb.Model
{
    /// <summary>
    /// Глобальные настройки
    /// </summary>
    public static class Globals
    {
        private static CultureInfo _DefaultCulture = new CultureInfo("ru-ru");
   
        
        /// <summary>
        /// Культура по умолчанию
        /// </summary>
        public static CultureInfo DefaultCulture
        {
            get { return _DefaultCulture; }
        }

        #region Bad code. Only for example
        /// <summary>
        /// Получение списка поддерживаемфх культур
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<CultureInfo> SupportCultures()
        {
            List<CultureInfo> Cultures = new List<CultureInfo>();
            Cultures.Add(new CultureInfo("ru-ru"));
            Cultures.Add(new CultureInfo("en-us"));
            return Cultures;
        }

        /// <summary>
        /// Изменение культуры по умолчанию
        /// </summary>
        /// <returns></returns>
        public static CultureInfo ChangeCurrentCulture()
        {
            _DefaultCulture = SupportCultures().Where(c => c.Name != _DefaultCulture.Name).FirstOrDefault();
            return _DefaultCulture;
        }
        #endregion

    }
}
