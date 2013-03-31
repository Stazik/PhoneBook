using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PhoneBookWeb.Extensions
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string culture = (filterContext.HttpContext.Session["culture"] != null)
                                 ? filterContext.HttpContext.Session["culture"].ToString()
                                 : PhoneBookWeb.Model.Globals.DefaultCulture.ToString();

            CultureInfo cultureInfo = new CultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
