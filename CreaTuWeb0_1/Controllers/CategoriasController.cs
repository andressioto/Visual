using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CreaTuWeb0_1.Models;
using IdentitySample.Models;
using System.Collections;

namespace CreaTuWeb0_1.Controllers
{
    public class CategoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categorias
        public ActionResult Index()
        {
            return View(db.Categorias.ToList());
        }

        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            SelectList CategoriaPId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            ViewBag.CategoriaPId = CategoriaPId;
            return View();
        }

        // POST: Categorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriaId,NombreCategoria,DescripcionCategoria")] Categoria categoria,string CategoriaPId)
        {
            if (CategoriaPId!="")
            categoria.CategoriaPId = Int32.Parse(CategoriaPId);
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            SelectList CategoriaPIdt = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            IEnumerable<SelectListItem> CategoriaPId = CategoriaPIdt.Select(x => new SelectListItem()
            {
                //no es nada rarro si el valor de categoriaId es igual al del padre de la categoria seleccionamos el item
                Selected = Int32.Parse(x.Value) == categoria.CategoriaPId,
                Text = x.Text,
                Value = x.Value,
                Disabled = Int32.Parse(x.Value) == categoria.CategoriaId || Categoria.esDescendiente(categoria.CategoriaId, Int32.Parse(x.Value))
            });
            ViewBag.CategoriaPId = CategoriaPId;
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriaId,NombreCategoria,DescripcionCategoria")] Categoria categoria, string CategoriaPId)
        {
            if (CategoriaPId != "")
            categoria.CategoriaPId = Int32.Parse(CategoriaPId);
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            if (Categoria.descendientes(new ArrayList(), id).Count > 0 || Categoria.tieneHijos(id))
            {
                ModelState.AddModelError("categoria.CategoriaPId", "Para borrar esta categoría primero debes borrar todas las categorías descendientes");
            }else
            {
                db.Categorias.Remove(categoria);
                db.SaveChanges();
            }
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
