using IdentitySample.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreaTuWeb0_1.Models
{
    public class Categoria
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categoria()
        {
            this.ProductoCategorias = new HashSet<ProductoCategoria>();
        }
        [Key]
        public int CategoriaId { get; set; }
        [Display(Name = "Nombre")]
        public string NombreCategoria { get; set; }
        [Display(Name = "Descripción")]
        public string DescripcionCategoria { get; set; }
        [Display(Name = "Categoría padre")]
        public int CategoriaPId { get; set; }
        public virtual ICollection<ProductoCategoria> ProductoCategorias { get; set; }
       /// <summary>
       /// devuelve true si el posibleHijo es descendiente del padre y false si no es su descendiente
       /// </summary>
       /// <param name="padre"></param>
       /// <param name="posibleHijo"></param>
       /// <returns>
       /// bool a la pregunta es descendiente: verdadero o falso
       /// </returns>
        public static bool esDescendiente(int padre, int posibleHijo)
        {
            ApplicationDbContext db=new ApplicationDbContext();
            Categoria catPosibleHijo = db.Categorias.Find(posibleHijo);
            if (catPosibleHijo.CategoriaPId == padre)
            {
                //es padre directo, no hay que comprobar nada más
                return true;
            }else
            {
                //no es hijo directo tendremos que comprobar si es desciendiente
                //para eyo buscamos a todos sus descendientes, los guardamos en un arraylist y luego comprobamos si el hijo esta entre los 
                //los valores de éste
                return arrayListContiene(descendientes(new ArrayList(), padre), posibleHijo);
            }
        }
        /// <summary>
        /// devuelve un ArrayList con todos los descendientes de la categoria con Id IdP
        /// </summary>
        /// <param name="desc"> arrayList que colecciona los descendientes</param>
        /// <param name="IdP"> Id del actual padre irá cambiando cada ves que bajemos de nivel(podra ser hijo y padre)</param>
        /// <returns>ArrayList con todos los descendientes</returns>
        public static ArrayList descendientes(ArrayList desc,int IdP)
        {
            //si el padre actual tiene hijos
            if(tieneHijos(IdP))
            {
                foreach(int item in hijos(IdP))
                {
                    //para cada uno de sus hijos, lo añadimos y llamamos recursivamente para comprobar si el hijo tiene hijos
                    desc.Add(item);
                    descendientes(desc, item);
                }
            }
            return desc;

        }
        /// <summary>
        /// comprueba si el Id introducido tiene hijos
        /// </summary>
        /// <param name="IdP">Id de padre actual</param>
        /// <returns>A la pregunta tieneHijos responde verdadero o falso</returns>
        public static bool tieneHijos(int IdP)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Categorias.Where(c => c.CategoriaPId == IdP).Count()>0;
        }
        /// <summary>
        /// Devuelve los hijos de un Id determinado
        /// </summary>
        /// <param name="IdP">Id del padre</param>
        /// <returns>ArrayList de todos sus hijos</returns>
        public static ArrayList hijos(int IdP)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var hs = db.Categorias.Where(c => c.CategoriaPId == IdP);
            ArrayList his = new ArrayList();
            foreach(var item in hs)
            {
                his.Add(item.CategoriaId);
            }
            return his;
        }
        /// <summary>
        /// se encarga de comprobar si un numero e¡entero esta entr los valores del arrayList
        /// </summary>
        /// <param name="a">ArrayList</param>
        /// <param name="n">Número a comprobar</param>
        /// <returns>A la pregunta el arraylist a contiene este nuero n devuelve la respuesta true o false</returns>
        public static bool arrayListContiene(ArrayList a,int n)
        {
            foreach(int item in a)
            {
                if (item == n)
                    return true;
            }
            return false;
        }
    }

    public class DetalleFactura
    {
        public int DetalleFacturaId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int descuento { get; set; }
        public int FacturaFacturaId { get; set; }
        public int ProductoProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual Factura Factura { get; set; }
    }
    public class DetalleOferta
    {
        public int DetalleOfertaId { get; set; }
        public int OfertaOfertaId { get; set; }
        public int ProductoProductoId { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Producto Producto { get; set; }
    }
    public class Factura
    {
        public Factura()
        {
            this.DetalleFacturas = new HashSet<DetalleFactura>();
        }
        public int FacturaId { get; set; }
        public String ClienteClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public bool Pagada { get; set; }
        public virtual IdentitySample.Models.ApplicationUser Cliente { get; set; }
        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
    public class Imagen
    {
        public int ImagenId { get; set; }
        public string carpeta { get; set; }
        public string nombre { get; set; }
        public virtual Producto Producto { get; set; }
    }
    public class Metadato
    {
        public int MetadatoId { get; set; }
        public string rutaCompletaLogo { get; set; }
        public string rutaCompletafavicon { get; set; }
        public string TitleGlobal { get; set; }
        public string MetaWebMaster { get; set; }
        public string MetaAuthor { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Analytics { get; set; }
        public string Licencia { get; set; }
        public string NombreWeb { get; set; }
        public string ruTaImagen { get; set;}
    }
    public class Oferta
    {
        public Oferta()
        {
            this.DetalleOfertas = new HashSet<DetalleOferta>();
        }
        public int OfertaId { get; set; }
        public int Descuento { get; set; }
        public int Duracion { get; set; }
        public string Titulo { get; set; }
        public string Comentario { get; set; }
        public DateTime Fcomienzo { get; set; }
        public DateTime Ffin { get; set; }
        public int OfertataOfertaId { get; set; }
        public virtual ICollection<DetalleOferta> DetalleOfertas { get; set; }

    }
    public class Producto
    {
        public Producto()
        {
            this.ProductoCategorias = new HashSet<ProductoCategoria>();
            this.Imagenes = new HashSet<Imagen>();
        }
        [Key]
        public int ProductoId { get; set; }
        public int ImagenPrincipalId { get;set; }
        [Required]
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        [Required]
        public decimal PrecioProducto { get; set; }
        public System.DateTime Fpublicado { get; set; }
        public System.DateTime FUltimaModificacion { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
        public virtual ICollection<Imagen> Imagenes { get; set; }
        public virtual ICollection<ProductoCategoria> ProductoCategorias { get; set; }
    }
    public class ProductoCategoria
    {
        public int ProductoCategoriaId { get; set; }
        public int ProductoProductoId { get; set; }
        public int CategoriaCategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Producto Producto { get; set; }
    }
    public class Slider
    {
        public int SliderId { get; set; }
        public string Texto { get; set; }
        public string titulo { get; set; }
        public string RutaCompletaImagen { get; set; }
    }
}