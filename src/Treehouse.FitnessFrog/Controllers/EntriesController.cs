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

            return View();
        }

        [HttpPost] //need to add these attribute so that MVC can associate this AddPost to an add Method //actioname attrib was removed because we changed the add method below
        public ActionResult Add(DateTime? date, int? activityId, double? duration, Entry.IntensityLevel? intensity, bool? exclude, string notes) //added another Action called AddPost to handle Post Request.
        {
            //extract the date of the form field value
            //string date = Request.Form["Date"]; instead of doing it like this you can specify a parameter in the method call. see up inside AddPost()
            
            //updated the parameter values in the method from string to appropriate parameters. IntensityLevel is an enum defined else where.
            ViewBag.Date = date;
            ViewBag.ActivityID = activityId;
            ViewBag.Duration = duration;
            ViewBag.Intensity = intensity;
            ViewBag.Exclude = exclude;
            ViewBag.Notes = notes;
            
            return View();
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