using System.ComponentModel.DataAnnotations;

namespace PracticaMVC.Web.ViewModels
{
    public class InicioSesionVM
    {
        [Required(ErrorMessage = "Este dato es requerido")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
    }
}