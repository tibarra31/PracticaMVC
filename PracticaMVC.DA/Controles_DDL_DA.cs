using PracticaMVC.EN;
using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.DA
{
    public class Controles_DDL_DA
    {
        /// <summary>
        /// Retorna una lista de los perfiles
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetPerfiles_DDL(string textoInicial)
        {
            var response = new DBResponse<IEnumerable<dynamic>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from f in db.Perfiles
                                 select new
                                 {
                                     Valor = f.IdPerfil,
                                     Texto = f.Perfil
                                 }).Union(from p in db.Perfiles
                                          select new
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.NumRows = query.Count;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Retorna una lista de los perfiles
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<List<ControlDDL>> GetPerfilesApp_DDL(string textoInicial)
        {
            var response = new DBResponse<List<ControlDDL>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from f in db.Perfiles
                                 select new ControlDDL
                                 {
                                     Valor = f.IdPerfil,
                                     Texto = f.Perfil
                                 }).Union(from p in db.Perfiles
                                          select new ControlDDL
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.NumRows = query.Count;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// Retorna una lista de los Roles para el perfil seleccionado
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <param name="idPerfil">Id del Perfil</param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetRolesPerfiles_DDL(string textoInicial, int idPerfil)
        {
            var response = new DBResponse<IEnumerable<dynamic>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from f in db.PerfilesRoles
                                 where f.IdPerfil == idPerfil
                                 select new
                                 {
                                     Valor = f.IdPerfilRol,
                                     Texto = f.PerfilRol
                                 }).Union(from p in db.PerfilesRoles
                                          select new
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.NumRows = query.Count;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Retorna una lista de los permisos existentes
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetPermisos_DDL(string textoInicial)
        {
            var response = new DBResponse<IEnumerable<dynamic>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from f in db.Permisos
                                 select new
                                 {
                                     Valor = f.IdPermiso,
                                     Texto = f.Permiso
                                 }).Union(from p in db.Permisos
                                          select new
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.NumRows = query.Count;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Retorna una lista de los Usuarios existentes
        /// </summary>
        /// <param name="textoInicial"></param>
        /// <returns></returns>
        public DBResponse<IEnumerable<dynamic>> GetUsuarios_DDL(string textoInicial)
        {
            var response = new DBResponse<IEnumerable<dynamic>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from f in db.Usuarios
                                 where f.Activo == true
                                 select new
                                 {
                                     Valor = f.IdUsuario,
                                     Texto = f.Usuario
                                 }).Union(from p in db.Usuarios
                                          select new
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.NumRows = query.Count;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }        
}
