using CreaTuWeb0_1.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreaTuWeb0_1.Models
{
    public class MisEntidadesViewModel
    {
        public MisEntidadesViewModel()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Productos = new HashSet<Producto>(db.Productos);
            Categorias = new HashSet<Categoria>(db.Categorias);
        }
        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }
    }
}
public class ProductosAjax
{
    public ProductosAjax(ICollection<Producto> pr)
    {
        this.productos = Rellena(pr);
    }
    private HashSet<ProductoAjax> Rellena(ICollection<Producto> pr)
    {
        HashSet<ProductoAjax> prs = new HashSet<ProductoAjax>();
        foreach(Producto p in pr)
        {
            prs.Add(new ProductoAjax(p));
        }
        return prs;

    }
    public HashSet<ProductoAjax> productos { get; set; }
}
public class ProductoAjax
{
    public ProductoAjax(Producto pr)
    {
            ProductoId = pr.ProductoId;
            if (pr.Imagenes.Where(i => i.ImagenId == pr.ImagenPrincipalId).Count()>0)
            {
                ImagenPrincipal = pr.Imagenes.Where(i => i.ImagenId == pr.ImagenPrincipalId).First().carpeta;
                ImagenPrincipal += pr.Imagenes.Where(i => i.ImagenId == pr.ImagenPrincipalId).First().nombre;
                NombreProducto = pr.NombreProducto;
                DescripcionProducto = pr.DescripcionProducto;
                PrecioProducto = pr.PrecioProducto;
                Fpublicado = pr.Fpublicado;
                FUltimaModificacion = pr.FUltimaModificacion;
                ICollection<Imagen> im = new HashSet<Imagen>(pr.Imagenes.Where(i => i.Producto.ProductoId == pr.ProductoId));
                Imagenes = convierteImagenAImagenAjax(im);
            }
    }
        public int ProductoId { get; set; }
        public string ImagenPrincipal { get;set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public System.DateTime Fpublicado { get; set; }
        public System.DateTime FUltimaModificacion { get; set; }
        public  ICollection<ImagenAjax> Imagenes { get; set; }

        public HashSet<ImagenAjax> convierteImagenAImagenAjax(ICollection<Imagen> imagenes)
        {
            var ImagenesDevueltas = new HashSet<ImagenAjax>();
            foreach (Imagen imagen in imagenes)
            {
                ImagenesDevueltas.Add(new ImagenAjax(imagen));

            }
            return ImagenesDevueltas;
        }

}
public class ImagenAjax
{
    public ImagenAjax(Imagen im)
    {
            ImagenId = im.ImagenId;
            ruta = im.carpeta;
            nombre = im.nombre;;
    }
    public int ImagenId { get; set; }
    public string ruta { get; set; }
    public string nombre { get; set; }
}