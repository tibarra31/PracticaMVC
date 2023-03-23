

using PracticaMVC.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticaMVC.Web.ViewModels
{
    public class Listado_UsuariosVM
    {
        #region Filtros especiales de la página
        public string Usuario { get; set; }
        public int IdUsuario { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        public string Campo { get; set; } = "Usuario";
        public string Orden { get; set; }
        public int TotalFilas { get; set; } = 50;
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; }
        #endregion

        public List<Listado_Usuarios> Listado { get; set; }
        
        public IEnumerable<dynamic> UsuariosDDL { get; set; }

        [Display(Name = "Carga Masiva")]
        public string CargaMasiva { get; set; }
        public string CargaMasivaLinkMensaje { get; set; }

    }
}