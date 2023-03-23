using PracticaMVC.DA;
using PracticaMVC.EN;
using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL
{
    public class Controles_DDL_BL
    {
        /// <summary>
        /// Retorna una lista de los perfiles
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetPerfiles_DDL(string textoInicial)
        {
            return new Controles_DDL_DA().GetPerfiles_DDL(textoInicial);
        }
        /// <summary>
        /// Retorna una lista de los perfiles
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<List<ControlDDL>> GetPerfilesApp_DDL(string textoInicial)
        {
            return new Controles_DDL_DA().GetPerfilesApp_DDL(textoInicial);
        }
        /// <summary>
        /// Retorna una lista de los Roles para el perfil seleccionado
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <param name="idPerfil">Id del Perfil</param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetRolesPerfiles_DDL(string textoInicial, int idPerfil)
        {
            return new Controles_DDL_DA().GetRolesPerfiles_DDL(textoInicial, idPerfil);
        }
        /// <summary>
        /// Retorna una lista de los permisos existentes
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetPermisos_DDL(string textoInicial)
        {
            return new Controles_DDL_DA().GetPermisos_DDL(textoInicial);
        }
        /// <summary>
        /// Retorna una lista de los Usuarios existentes
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetUsuarios_DDL(string textoInicial)
        {
            return new Controles_DDL_DA().GetUsuarios_DDL(textoInicial);
        }
    }
}
