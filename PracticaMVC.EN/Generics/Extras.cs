using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.EN
{
    public partial class Usuarios
    {
        public string PerfilUsuario { get; set; }
        public string EstatusUsuario { get; set; }
        public Documento Archivo { get; set; }
    }
}
