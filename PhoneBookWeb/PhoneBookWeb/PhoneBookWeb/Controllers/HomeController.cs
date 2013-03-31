using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using PhoneBookWeb.Model;
using PhoneBookWeb.Extensions;

namespace PhoneBookWeb.Controllers
{
    [Localization]
    public class HomeController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<PhoneBookRecord> data = null;
            try
            {
                data = db.PhoneBookRecords.Include("Phones").Include("Man").ToList();
            }
            catch (EntityException exception)
            {
                ViewBag.Error = exception.Message;
                return View("Error");
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            ViewBag.Home = Resources.Resources.Home;

            //Поиск по searchString
            IEnumerable<PhoneBookRecord> SearchResult;
            string criterion = searchString.Trim();
            //Если пустая
            if (string.IsNullOrWhiteSpace(criterion))
            {
                SearchResult = db.PhoneBookRecords.Include("Phones").Include("Man").ToList();
            }
            else
            {
                try
                {
                    //Если 1 слово [Фамилия или Имя]
                    if (!criterion.Contains(' '))
                    {
                        SearchResult = db.PhoneBookRecords.Include("Phones").Include("Man").Where(c => c.Man.LastName.Contains(criterion) || c.Man.FirstName.Contains(criterion)).ToList();
                    }
                    //Если 2 слова
                    else
                    {
                        int IndexOfSpace = criterion.IndexOf(' ');
                        string FirstWord = criterion.Substring(0, IndexOfSpace).Trim();
                        string SecondWord = criterion.Substring(IndexOfSpace + 1, criterion.Length - (IndexOfSpace + 1)).Trim();
                        SearchResult = db.PhoneBookRecords.Include("Phones").Include("Man")
                                    .Where(c => (c.Man.LastName.Contains(FirstWord) && c.Man.FirstName.Contains(SecondWord))
                                                || (c.Man.LastName.Contains(SecondWord) && c.Man.FirstName.Contains(FirstWord)))
                                    .ToList();
                    }
                }
                catch (EntityException exception)
                {
                    ViewBag.Error = exception.Message;
                    return View("Error");
                }
            }
            return View(SearchResult);
        }

        public ActionResult Details(int id = 0)
        {

            PhoneBookRecord phonebookrecord = null;
            try
            {
                phonebookrecord = db.PhoneBookRecords.Include("Phones").Include("Man").Where(c => c.Id == id).FirstOrDefault();
            }
            catch (EntityException exception)
            {
                ViewBag.Error = exception.Message;
                return View("Error");
            }

            if (phonebookrecord == null)
            {
                return HttpNotFound();
            }
            return View(phonebookrecord);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult CultureChange()
        {
            Session["culture"] = Globals.ChangeCurrentCulture();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}