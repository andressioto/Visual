$(document).ready(productoAjax);
function productoAjax()
{
    $("a.link-p").on("click", function (e) {
        e.preventDefault();
    });
    setInterval(conectaAjax, 3000);
    function conectaAjax()
    {
        $.ajax({
            url: "/Productos/indexAjax",
            method: "Get",
            //data: { Id: nick, password: pass, entrar: "" },
            dataType: "json",
            success: function (data) {
                repinta(data)
            },
            error: function (xhr, status, error) {
                //alert(error);
            },

        });
    }

    function repinta(data)
    {
        item = $(".first-p img").attr("src", data.ima)
        creaProtos(data)
       // console.log(data.productos)
    }
    function creaProtos(data) {
        HTMLDivElement.prototype.identificador = "";
        HTMLDivElement.prototype.descripcion = "";
        HTMLDivElement.prototype.titulo = "";
        HTMLDivElement.prototype.precio = 0;
        console.log(data.productos);
    }
}
