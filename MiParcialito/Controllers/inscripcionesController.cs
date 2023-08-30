using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiParcialito.Models;

namespace MiParcialito.Controllers
{
    public class inscripcionesController : Controller
    {
        private BR100720Entities1 db = new BR100720Entities1();

        // GET: inscripciones
        public ActionResult Index()
        {
            var inscripciones = db.inscripciones.Include(i => i.cursos).Include(i => i.user);
            return View(inscripciones.ToList());
        }

        // GET: inscripciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inscripciones inscripciones = db.inscripciones.Find(id);
            if (inscripciones == null)
            {
                return HttpNotFound();
            }
            return View(inscripciones);
        }

        // GET: inscripciones/Create
        public ActionResult Create()
        {
            int userId = (int)Session["UserId"];

            var cursosList = db.cursos.ToList(); 
            var userSelectList = new SelectList(db.user.Where(u => u.idUser == userId), "idUser", "email", userId);

            ViewBag.cursoID = new SelectList(cursosList, "cursoID", "nombreCurso");
            ViewBag.idUser = userSelectList;

            return View();
        }





        // POST: inscripciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idInscripcion,cursoID,idUser")] inscripciones inscripciones)
        {
            if (ModelState.IsValid)
            {
                db.inscripciones.Add(inscripciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cursoID = new SelectList(db.cursos, "cursoID", "nombreCurso", inscripciones.cursoID);
            ViewBag.idUser = new SelectList(db.user, "idUser", "email", inscripciones.idUser);
            return View(inscripciones);
        }

        // GET: inscripciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inscripciones inscripciones = db.inscripciones.Find(id);
            if (inscripciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.cursoID = new SelectList(db.cursos, "cursoID", "nombreCurso", inscripciones.cursoID);
            ViewBag.idUser = new SelectList(db.user, "idUser", "email", inscripciones.idUser);
            return View(inscripciones);
        }

        // POST: inscripciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idInscripcion,cursoID,idUser")] inscripciones inscripciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inscripciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cursoID = new SelectList(db.cursos, "cursoID", "nombreCurso", inscripciones.cursoID);
            ViewBag.idUser = new SelectList(db.user, "idUser", "email", inscripciones.idUser);
            return View(inscripciones);
        }

        // GET: inscripciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inscripciones inscripciones = db.inscripciones.Find(id);
            if (inscripciones == null)
            {
                return HttpNotFound();
            }
            return View(inscripciones);
        }

        // POST: inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inscripciones inscripciones = db.inscripciones.Find(id);
            db.inscripciones.Remove(inscripciones);
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
