using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiParcialito.Models;
using BCrypt.Net;

namespace MiParcialito.Controllers
{
    public class usersController : Controller
    {
        private BR100720Entities1 db = new BR100720Entities1();

        // GET: users
        public ActionResult Index(ActionExecutingContext filterContext)
        {
            if (Session["IsAdmin"].ToString() == "True")
            {
                var user = db.user.Include(u => u.roles).Include(u => u.status);
                return View(user.ToList());
            }
            else if (Session["IsAdmin"].ToString() == "False")
            {
                return RedirectToAction("Index", "Access");
            }

            else
            {

                return RedirectToAction("AccessDenied", "Home");
            }

        }

        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.idroles = new SelectList(db.roles, "idRoles", "nombre");
            ViewBag.idStatus = new SelectList(db.status, "idStatus", "nombre");
            return View();
        }

        // POST: users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idUser,email,password,edad,fecha_nacimiento,idroles,idStatus")] user user)
        {
            if (ModelState.IsValid)
            {
                // Encriptar la contraseña antes de guardarla
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

        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.idroles = new SelectList(db.roles, "idRoles", "nombre", user.idroles);
            ViewBag.idStatus = new SelectList(db.status, "idStatus", "nombre", user.idStatus);
            return View(user);
        }

        // POST: users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idUser,email,password,edad,fecha_nacimiento,idroles,idStatus")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idroles = new SelectList(db.roles, "idRoles", "nombre", user.idroles);
            ViewBag.idStatus = new SelectList(db.status, "idStatus", "nombre", user.idStatus);
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.user.Find(id);
            db.user.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
