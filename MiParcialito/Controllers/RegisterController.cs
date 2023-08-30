using MiParcialito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiParcialito.Controllers
{
    public class RegisterController : Controller
    {

        private BR100720Entities1 db = new BR100720Entities1();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idUser,email,password,edad,fecha_nacimiento,idroles,idStatus")] user user)
        {
           
            user.idroles = 2; 
            user.idStatus = 1; 

            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                user.password = hashedPassword;

                db.user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idroles = new SelectList(db.roles, "idRoles", "nombre", user.idroles);
            ViewBag.idStatus = new SelectList(db.status, "idStatus", "nombre", user.idStatus);
            return View(user);
        }

    }
}