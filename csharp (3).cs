using System.Linq;
using System.Web.Mvc;
using YourProjectName.Models;

namespace YourProjectName.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Calculate the quote
                insuree.Quote = CalculateQuote(insuree);
                
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            var quotes = db.Insurees.Select(i => new AdminQuoteViewModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Quote = i.Quote
            }).ToList();

            return View(quotes);
        }

        private decimal CalculateQuote(Insuree insuree)
        {
            decimal quote = 50; // Base price

            // Age logic
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (DateTime.Now < insuree.DateOfBirth.AddYears(age))
                age--;

            if (age <= 18)
                quote += 100;
            else if (age >= 19 && age <= 25)
                quote += 50;
            else
                quote += 25;

            // Car year logic
            if (insuree.CarYear < 2000)
                quote += 25;
            else if (insuree.CarYear > 2015)
                quote += 25;

            // Car make and model logic
            if (insuree.CarMake.ToLower() == "porsche")
            {
                quote += 25;
                
                if (insuree.CarModel.ToLower() == "911 carrera")
                    quote += 25;
            }

            // Speeding tickets
            quote += insuree.SpeedingTickets * 10;

            // DUI - add 25%
            if (insuree.DUI)
                quote *= 1.25m;

            // Full coverage - add 50%
            if (insuree.CoverageType)
                quote *= 1.5m;

            return quote;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}