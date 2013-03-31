using System;
using System.Text;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookWeb.Model
{
    /// <summary>
    /// Man
    /// </summary>
    public class Man:Entity
    {
        /// <summary>
        /// Firstname
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "MiddleName")]
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
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "Birthday")]
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
                        return date.ToString(PhoneBookWeb.Resources.Resources.DateFormat);
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
                        return date.ToString(PhoneBookWeb.Resources.Resources.DateDayMonthFormat);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        StringBuilder format = new StringBuilder(PhoneBookWeb.Resources.Resources.DateDayMonthFormat);
                        format.Replace("dd", BirhtDay.Value.ToString(PhoneBookWeb.Resources.Resources.DateIntFormat));
                        format.Replace("MM", BirhtMonth.Value.ToString(PhoneBookWeb.Resources.Resources.DateIntFormat));

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
        /// 
        /// </summary>
        public Man()
        {

        }

        /// <summary>
        /// FullName in regional format
        /// </summary>
        /// <returns>Fullname</returns>
        public string Name
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, PhoneBookWeb.Resources.Resources.FIO, this.LastName, this.FirstName, this.MiddleName);
            }
        }
    }
}
