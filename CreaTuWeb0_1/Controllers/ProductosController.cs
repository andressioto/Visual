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
using System.IO;
using Microsoft.AspNet.Identity;

namespace CreaTuWeb0_1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //Get:productos ajax
        public ActionResult indexAjax()
        {
            return Json(new ProductosAjax(db.Productos.ToList()), JsonRequestBehavior.AllowGet);
        }
        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,NombreProducto,DescripcionProducto,PrecioProducto")] Producto producto,int[] categoriasSeleccionadas, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid&&categoriasSeleccionadas!=null)
            {
                producto.Fpublicado = DateTime.Now;
                producto.FUltimaModificacion = DateTime.Now;
                //directorio dende se guardan las imagenes 
                string carpeta = @"\upload";
                string directorio = @"A:\DAW\2\servidor\trim2\CreaTuWeb0_1\CreaTuWeb0_1"+carpeta;

                foreach(var file in files)
                {
                    if(file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        Imagen im = new Imagen();
                        im.nombre = fileName;
                        im.carpeta = carpeta;
                        im.Producto = producto;
                        db.Imagenes.Add(im);
                        file.SaveAs(Path.Combine(directorio, fileName));
                    }
                }
                ApplicationUser us = db.Users.Find(User.Identity.GetUserId());
                us.Id = User.Identity.GetUserId();
                producto.Usuario = us;
                db.Productos.Add(producto);
                int idP=producto.ProductoId;
                foreach (int x in categoriasSeleccionadas)
                {
                    ProductoCategoria pc = new ProductoCategoria();
                    pc.ProductoProductoId = idP;
                    pc.CategoriaCategoriaId = x;
                    db.ProductoCategorias.Add(pc);
                }
                db.SaveChanges();
                ponerPrincipal(producto);
                return RedirectToAction("Index");
            }
            if(categoriasSeleccionadas==null)
            {
                //si estamos aqui es por que no ha elegido categoría para el producto
                ModelState.AddModelError("", "No puedes insertar un producto sin categoría");
                ViewBag.CategoriaCategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");

            }
            return View(producto);
        }
        public void ponerPrincipal(Producto pr)
        {
            int idPr = pr.Imagenes.First().ImagenId;
            pr.ImagenPrincipalId = idPr;
            db.Productos.Attach(pr);
            db.Entry(pr).Property("ImagenPrincipalId").IsModified = true;
            db.SaveChanges();
        }
        //metodo que comprueba si una coleccion contiene un valor
        public bool contiene(string cadena, SelectList catesel)
        {
            foreach (var item in catesel)
            {
                bool result = cadena == item.Text;
                if (result)
                {
                    return true;
                }
            }
            return false;
        }
        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            var categoriasDelProducto = from x in db.ProductoCategorias where x.ProductoProductoId==id select x;
            SelectList catesel = new SelectList(categoriasDelProducto, "ProductoProductoId", "CategoriaCategoriaId");
            SelectList cateenv = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
           IEnumerable<SelectListItem> envio= cateenv.Select(x => new SelectListItem()
            {
                Selected = contiene(x.Value, catesel),
                Text=x.Text,
                Value=x.Value
            });

           ViewBag.CategoriaCategoriaId = envio;
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,NombreProducto,DescripcionProducto,PrecioProducto,Fpublicado")] Producto producto, string[] categoriasProducto,string Principal)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated && User.IsInRole("Admin"))
               {
                   db.Entry(producto).State = EntityState.Modified;
                   producto.FUltimaModificacion = DateTime.Now;
                   producto.ImagenPrincipalId = Int32.Parse(Principal);
                    ApplicationUser us=new ApplicationUser();
                    us.Id = User.Identity.GetUserId();
                   //producto.Usuario = us;
                   //intento borrar las categorias y volcar las nuevas
                   //productos actuales en ProductoCategorias
                   var categoriasDelProducto = from x in db.ProductoCategorias where x.ProductoProductoId == producto.ProductoId select x;
                   foreach (var it in categoriasDelProducto)
                   {
                       ProductoCategoria pcb = db.ProductoCategorias.Find(it.ProductoCategoriaId);
                       db.ProductoCategorias.Remove(pcb);

                   }

                   foreach (var item in categoriasProducto)
                   {
                       ProductoCategoria pcp = new ProductoCategoria();
                       pcp.CategoriaCategoriaId = Int32.Parse(item);
                       pcp.ProductoProductoId = producto.ProductoId;
                       db.ProductoCategorias.Add(pcp);
                   }
                   db.SaveChanges();
                   return RedirectToAction("Edit", new { id =producto.ProductoId });
               }

               }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            if (db.Imagenes.Where(i => i.ImagenId == producto.ImagenPrincipalId).Count()>0)
            {
                db.Imagenes.Remove(db.Imagenes.Where(i => i.ImagenId == producto.ImagenPrincipalId).First());
            }
            db.Productos.Remove(producto);
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
