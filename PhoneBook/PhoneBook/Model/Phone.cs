using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Model
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    public class Phone:Entity
    {
        private int _Type = 1;

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        ///Типа номера
        /// </summary>
        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        /// Наименование типа номера (вычисляемое поля в зависимости от поля Type)
        /// </summary>
        [NotMapped]
        public string TypeName
        {
            get
            {
                Dictionary<int, string> types = Phone.TypesList();
                if (types.ContainsKey(Type))
                {
                    return types[Type];
                }
                return Properties.Resources.OtherNumber;
            }
            set
            {
                Dictionary<int, string> types = Phone.TypesList();
                if (types.ContainsValue(value))
                {
                    Type = types.Where(c => c.Value == value).Select(c => c.Key).FirstOrDefault();
                }

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
            types.Add(1, Properties.Resources.MobilNumber);
            types.Add(2, Properties.Resources.WorkNumber);
            types.Add(3, Properties.Resources.HomeNumber);
            types.Add(0, Properties.Resources.OtherNumber);
            
            return types;
        }

        /// <summary>
        /// Проверка корректности номера
        /// </summary>
        /// <returns>Истина - номер корректно заполнен</returns>
        public static bool Validate(Phone phone, out string error)
        {
            error = Properties.Resources.FieldIsEmpty;
            StringBuilder str = new StringBuilder();
            bool success = true;

            if (string.IsNullOrWhiteSpace(phone.Number))
            {
                str.AppendLine(String.Format(error, Properties.Resources.PhoneNumber));
                success = false;
            }

            if ( phone.Type < 0)
            {
                str.AppendLine(String.Format(error, Properties.Resources.PhoneType));
                success = false;
            }

            error = str.ToString();
            return success;
        }


        /// <summary>
        /// Копирование значимых полей объекта в объект copy 
        /// </summary>
        /// <param name="copy"></param>
        public void CopyTo(Phone copy)
        {
            if (this != null && copy != null)
            {
                copy.Number = this.Number;
                copy.Type = this.Type;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
