using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries(); //call to get the list of possible repos.

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add() //this method does not handle post requests
        {
            var entry = new Entry() //instantiate a new Entry on the ViewMethod call
            {
                Date = DateTime.Today
            };


            return View(entry);
        }

        [HttpPost] //need to add these attribute so that MVC can associate this AddPost to an add Method //actioname attrib was removed because we changed the add method below
        public ActionResult Add(Entry entry) //added another Action called AddPost to handle Post Request.
        {
            //extract the date of the form field value
            //string date = Request.Form["Date"]; instead of doing it like this you can specify a parameter in the method call. see up inside AddPost()

            //updated the parameter values in the method from string to appropriate parameters. IntensityLevel is an enum defined else where.
            //ViewBag.Date = ModelState["Date"].Value.AttemptedValue; //doing this lets us carry the value entered but would also keep what you entered.

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                //TODO Display the Entries List Page
            }

            return View(entry);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}