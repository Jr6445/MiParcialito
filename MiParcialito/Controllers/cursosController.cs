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
    public class cursosController : Controller
    {
        private BR100720Entities1 db = new BR100720Entities1();

        // GET: cursos
        public ActionResult Index()
        {
            if (Session["IsAdmin"] != null)
            {
                if (Session["IsAdmin"].ToString() == "True")
                {
                    var cursos = db.cursos.Include(c => c.docentes);
                    return View(cursos.ToList());
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
            else
            {

                return RedirectToAction("Index", "Access");
            }
        }

        // GET: cursos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cursos cursos = db.cursos.Find(id);
            if (cursos == null)
            {
                return HttpNotFound();
            }
            return View(cursos);
        }

        // GET: cursos/Create
        public ActionResult Create()
        {
            ViewBag.idDocente = new SelectList(db.docentes, "idDocente", "nombre");
            return View();
        }

        // POST: cursos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cursoID,nombreCurso,idDocente")] cursos cursos)
        {
            if (ModelState.IsValid)
            {
                db.cursos.Add(cursos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idDocente = new SelectList(db.docentes, "idDocente", "nombre", cursos.idDocente);
            return View(cursos);
        }

        // GET: cursos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cursos cursos = db.cursos.Find(id);
            if (cursos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idDocente = new SelectList(db.docentes, "idDocente", "nombre", cursos.idDocente);
            return View(cursos);
        }

        // POST: cursos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cursoID,nombreCurso,idDocente")] cursos cursos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cursos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idDocente = new SelectList(db.docentes, "idDocente", "nombre", cursos.idDocente);
            return View(cursos);
        }

        // GET: cursos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cursos cursos = db.cursos.Find(id);
            if (cursos == null)
            {
                return HttpNotFound();
            }
            return View(cursos);
        }

        // POST: cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cursos cursos = db.cursos.Find(id);
            db.cursos.Remove(cursos);
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
