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

namespace CreaTuWeb0_1.Controllers
{
    public class ProductoCategoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductoCategorias
        public ActionResult Index()
        {
            var productoCategorias = db.ProductoCategorias.Include(p => p.Categoria).Include(p => p.Producto);
            return View(productoCategorias.ToList());
        }

        // GET: ProductoCategorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoCategoria productoCategoria = db.ProductoCategorias.Find(id);
            if (productoCategoria == null)
            {
                return HttpNotFound();
            }
            return View(productoCategoria);
        }

        // GET: ProductoCategorias/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            ViewBag.ProductoProductoId = new SelectList(db.Productos, "ProductoId", "NombreProducto");
            return View();
        }

        // POST: ProductoCategorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoCategoriaId,ProductoProductoId,CategoriaCategoriaId")] ProductoCategoria productoCategoria)
        {
            if (ModelState.IsValid)
            {
                db.ProductoCategorias.Add(productoCategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", productoCategoria.CategoriaCategoriaId);
            ViewBag.ProductoProductoId = new SelectList(db.Productos, "ProductoId", "NombreProducto", productoCategoria.ProductoProductoId);
            return View(productoCategoria);
        }

        // GET: ProductoCategorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoCategoria productoCategoria = db.ProductoCategorias.Find(id);
            if (productoCategoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", productoCategoria.CategoriaCategoriaId);
            ViewBag.ProductoProductoId = new SelectList(db.Productos, "ProductoId", "NombreProducto", productoCategoria.ProductoProductoId);
            return View(productoCategoria);
        }

        // POST: ProductoCategorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoCategoriaId,ProductoProductoId,CategoriaCategoriaId")] ProductoCategoria productoCategoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productoCategoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", productoCategoria.CategoriaCategoriaId);
            ViewBag.ProductoProductoId = new SelectList(db.Productos, "ProductoId", "NombreProducto", productoCategoria.ProductoProductoId);
            return View(productoCategoria);
        }

        // GET: ProductoCategorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoCategoria productoCategoria = db.ProductoCategorias.Find(id);
            if (productoCategoria == null)
            {
                return HttpNotFound();
            }
            return View(productoCategoria);
        }

        // POST: ProductoCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductoCategoria productoCategoria = db.ProductoCategorias.Find(id);
            db.ProductoCategorias.Remove(productoCategoria);
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
