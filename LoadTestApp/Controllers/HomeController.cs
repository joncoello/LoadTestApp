using LoadTestApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoadTestApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult People()
        {
            ViewBag.Message = "Your people page.";

            var repo = new Repositories.PersonRepository();
            var people = repo.GetPeople();

            return View(people);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Person newPerson)
        {
            if (ModelState.IsValid)
            {
                var repo = new Repositories.PersonRepository();
                repo.CreatePerson(newPerson);

                return RedirectToAction("Details", new { id = newPerson.PersonID });
            }
            else
            {
                return View(newPerson);
            }
            
        }
        
        public ActionResult Delete(int id)
        {

            var repo = new Repositories.PersonRepository();
            repo.DeletePerson(id);

            return RedirectToAction("People");
        }

        public ActionResult Edit(int id)
        {

            var repo = new Repositories.PersonRepository();
            var person = repo.GetPerson(id);

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(Person person)
        {

            var repo = new Repositories.PersonRepository();
            repo.UpdatePerson(person);

            return RedirectToAction("Details", new { id = person.PersonID });
        }

        public ActionResult Details(int id)
        {

            var repo = new Repositories.PersonRepository();
            var person = repo.GetPerson(id);

            return View(person);
        }

    }
}