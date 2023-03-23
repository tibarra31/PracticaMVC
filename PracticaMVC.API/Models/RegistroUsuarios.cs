using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticaMVC.API.Models
{
    public class RegistroUsuarios
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public int IdPerfil { get; set; }
        public int IdEstatusRegistro { get; set; }
        public bool Activo { get; set; }
        public Documento Archivo { get; set; }

    }
}