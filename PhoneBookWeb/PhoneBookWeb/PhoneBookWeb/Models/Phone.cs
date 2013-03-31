
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace PhoneBookWeb.Model
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    public class Phone:Entity
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "PhoneNumber")]
        public string Number { get; set; }

        /// <summary>
        /// Тип номера
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// Наименование типа номера
        /// </summary>
        [Display(ResourceType = typeof(PhoneBookWeb.Resources.Resources), Name = "PhoneType")]
        public string TypeName
        {
            get
            {
                if (Type > 0 && Type <= 3)
                {
                    return Phone.TypesList()[Type].ToString();
                }
                return PhoneBookWeb.Resources.Resources.OtherNumber;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TypeName + " - " + Number;
        }

        /// <summary>
        /// Хард-код - нужно получать из БД
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> TypesList()
        {
            Dictionary<int, string> types = new Dictionary<int, string>();
            types.Add((byte)PhoneType.Mobil, PhoneBookWeb.Resources.Resources.MobilNumber);
            types.Add((byte)PhoneType.Work, PhoneBookWeb.Resources.Resources.WorkNumber);
            types.Add((byte)PhoneType.Home, PhoneBookWeb.Resources.Resources.HomeNumber);
            types.Add((byte)PhoneType.Other, PhoneBookWeb.Resources.Resources.OtherNumber);

            return types;
        }
    }
}
