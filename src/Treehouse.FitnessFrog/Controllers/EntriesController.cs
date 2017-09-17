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
                Date = DateTime.Today,
  
            };

            SetupActivitiesSelectListItems();


            return View(entry);
        }

        [HttpPost] //need to add these attribute so that MVC can associate this AddPost to an add Method //actioname attrib was removed because we changed the add method below
        public ActionResult Add(Entry entry) //added another Action called AddPost to handle Post Request.
        {
            //extract the date of the form field value
            //string date = Request.Form["Date"]; instead of doing it like this you can specify a parameter in the method call. see up inside AddPost()

            //updated the parameter values in the method from string to appropriate parameters. IntensityLevel is an enum defined else where.
            //ViewBag.Date = ModelState["Date"].Value.AttemptedValue; //doing this lets us carry the value entered but would also keep what you entered.

            //ModelState.AddModelError("","This is a global message.");


            //Inserting the validation rule

            //If there arent any duration field validation errors then make sure the duration is greater than 0
            ValidateEntry(entry);

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);
                //a work around for now

                return RedirectToAction("Index"); //post redirect get pattern... to prevent form duplication

            }

            SetupActivitiesSelectListItems();

            return View(entry);
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //TODO Get the request entry from the repository.
            Entry entry = _entriesRepository.GetEntry((int)id);
            //TODO Return a status of "not found' if the entry was not found.
            if (entry == null)
            {
                return HttpNotFound();
            }
            //TODO Pass the entry into the view

            //TODO Populate the activities select list items ViewBag Property
            SetupActivitiesSelectListItems();
            return View(entry);
        }

        //NEED TO ADD A CORESSPONDING Post list the add view page
        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            //TODO Validate the entry.
            ValidateEntry(entry);
            //TODO if Entry is Valid..
            //1. use the repository to update the entry
            //2. redirect the use to the "entries" list page
            if (ModelState.IsValid)
            {
                _entriesRepository.UpdateEntry(entry);
                return RedirectToAction("Index");
            }
            //TODO Populate the activities select list items ViewBag Property
            SetupActivitiesSelectListItems();
            return View(entry);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //TODO retrieve entry for the provided ID parameter value
            Entry entry = _entriesRepository.GetEntry((int)id);
               
            //TODO Return "not found" if an entry wasnt found
            if(entry == null)
            {
                return HttpNotFound();
            }
            //TODO Pass the entry to the view

            return View(entry);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //TODO Delete the entry
            _entriesRepository.DeleteEntry(id);

            //TODO Redirect the Entries list page

            return RedirectToAction("Index");
        }

        //hitting ctrl and period key creates an intelisence to create method.
        private void ValidateEntry(Entry entry)
        {
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration Field Value must be greater than '0'.");
            }
        }

        private void SetupActivitiesSelectListItems()
        {
            ViewBag.ActivitiesSelectListItems = new SelectList(
                Data.Data.Activities, "Id", "Name");
        }
    }
}