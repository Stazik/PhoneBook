using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Model
{
    /// <summary>
    /// Man
    /// </summary>
    public class Man:Entity
    {
        /// <summary>
        /// Firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Day of Birhtday
        /// </summary>
        public int? BirhtDay { get; set; }

        /// <summary>
        /// Month of Birhtday
        /// </summary>
        public int? BirhtMonth { get; set; }

        /// <summary>
        /// Year of Birhtday
        /// </summary>
        public int? BirhtYear{ get; set; }

        //TODO LOcaliz
        /// <summary>
        /// Represent Date of Birth
        /// </summary>
        [NotMapped]
        public string BirthDate
        {
            get
            {

                if (BirhtDay.HasValue && BirhtDay > 0
                    && BirhtMonth.HasValue && BirhtMonth > 0
                    && BirhtMonth.HasValue && BirhtYear > 0)
                {
                    try
                    {
                        DateTime date = new DateTime(BirhtYear.Value, BirhtMonth.Value, BirhtDay.Value);
                        return date.ToString(Properties.Resources.DateFormat);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return string.Empty;
                    }
                    
                }
                else
                {
                    //day and month
                    if (BirhtDay.HasValue && BirhtDay > 0  && BirhtMonth.HasValue && BirhtMonth > 0)
                    try
                    {
                        //4 год - 1 високосный год
                        DateTime date = new DateTime(4, BirhtMonth.Value, BirhtDay.Value);
                        return date.ToString(Properties.Resources.DateDayMonthFormat);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        StringBuilder format = new StringBuilder(Properties.Resources.DateDayMonthFormat);
                        format.Replace("dd", BirhtDay.Value.ToString(Properties.Resources.DateIntFormat));
                        format.Replace("MM", BirhtMonth.Value.ToString(Properties.Resources.DateIntFormat));

                        return format.ToString();
                    }

                    //year
                    if (BirhtYear.HasValue && BirhtYear > 0)
                    {
                        return BirhtYear.Value.ToString(CultureInfo.CurrentCulture);
                    }
                }
                return string.Empty;
            }
            set 
            { 
            }
        }

        /// <summary>
        /// Создание экземпляра класса "Физ. лицо"
        /// </summary>
        public Man()
        {

        }

        /// <summary>
        /// FullName in regional format
        /// </summary>
        /// <returns>Fullname</returns>
        public string Name()
        {
            return string.Format(CultureInfo.CurrentCulture, Properties.Resources.FIO, this.LastName, this.FirstName, this.MiddleName);
        }

        /// <summary>
        /// Копирование значимых полей объекта в объект copy 
        /// </summary>
        /// <param name="copy"></param>
        public void CopyTo(Man copy)
        {
            if (this != null && copy != null)
            {
                copy.LastName = this.LastName;
                copy.FirstName = this.FirstName;
                copy.MiddleName = this.MiddleName;
                copy.BirhtDay = this.BirhtDay;
                copy.BirhtMonth = this.BirhtMonth;
                copy.BirhtYear = this.BirhtYear;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
