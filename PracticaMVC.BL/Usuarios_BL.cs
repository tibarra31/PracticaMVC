using PracticaMVC.DA;
using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL
{
    public class Usuarios_BL
    {
        public DBResponse<Usuarios> ValidaUsuarioLogin(string usuario, string password)
        {
            return new Usuarios_DA().ValidaUsuarioLogin(usuario, password);
        }
        /// <summary>
        /// Devuelve el listado de usuarios
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuarios(string usuario, string sortOrder, int pageNumber, int pageSize)
        {
            return new Usuarios_DA().GetUsuarios(usuario, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Devuelve el listado de usuarios en pantalla Angular
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuariosAngular(Filtros_Usuarios filtrosUsuarios)
        {
            return new Usuarios_DA().GetUsuariosAngular(filtrosUsuarios);
        }
        /// <summary>
        /// Devuelve el listado de usuarios
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuarios_List(string usuario)
        {
            return new Usuarios_DA().GetUsuarios_List(usuario);
        }
        /// <summary>
        /// Devuelve un usuario por su Id
        /// </summary>
        /// <returns></returns>
        public DBResponse<Usuarios> GetUsuarios_ById(int IdUsuario)
        {
            return new Usuarios_DA().GetUsuarios_ById(IdUsuario);
        }
        /// <summary>
        /// Valida que el usuario ingresado no exista ya en la BD
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public DBResponse<Usuarios> ExisteUsuario(string usuario, int idUsuario)
        {
            return new Usuarios_DA().ExisteUsuario(usuario, idUsuario);
        }
        /// <summary>
        /// Actualiza/Inserta un usuario 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public DBResponse<Usuarios> UpsertUsuario(Usuarios usuario)
        {
            var dbResponse = new DBResponse<Usuarios>();
            try
            {
                dbResponse = new Usuarios_DA().UpsertUsuario(usuario);
                if (dbResponse.ExecutionOK)
                {
                    var dbItemUsuarios = new Usuarios_BL().GetUsuarios_List("");
                    var usuarioAdmin = dbItemUsuarios.Data.Where(x => x.IdPerfil == 1);
                    foreach (var item in usuarioAdmin)
                    {
                        var dbInsertaNotificacion = new Notificaciones_BL().InsertNotificacionUsuario(new Notificaciones_Usuarios()
                        {
                            Fecha = DateTime.Now,
                            IdNotificacion = 0,
                            IdUsuarioGeneroNotif = usuario.IdUsuario,
                            IdUsuarioRecibeNotif = item.IdUsuario,
                            Leido = false,
                            Mensaje = "El Usuario " + usuario.Usuario + " a sido dado de alta en el sistema"
                        });
                    }                   
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = ex.Message;
                dbResponse.Data = new Usuarios();
                dbResponse.ExecutionOK = false;
            }
            return dbResponse;
        }
        /// <summary>
        /// Elimina de manera logica al usuario seleccionado
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public DBResponse<int?> InactivarEliminarUsuario(int idUsuario)
        {
            var dbResponse = new DBResponse<int?>();
            try
            {
                var dbItemUsuarioInactivado = new Usuarios_BL().GetUsuarios_ById(idUsuario).Data;

                var dbResponseInactivate = new Usuarios_DA().InactivarEliminarUsuario(idUsuario);
                if (dbResponseInactivate.ExecutionOK)
                {
                    var dbItemUsuarios = new Usuarios_BL().GetUsuarios_List("");
                    var usuarioAdmin = dbItemUsuarios.Data.Where(x => x.IdPerfil == 1);
                    foreach (var item in usuarioAdmin)
                    {
                        var dbInsertaNotificacion = new Notificaciones_BL().InsertNotificacionUsuario(new Notificaciones_Usuarios()
                        {
                            Fecha = DateTime.Now,
                            IdNotificacion = 0,
                            IdUsuarioGeneroNotif = dbItemUsuarioInactivado.IdUsuario,
                            IdUsuarioRecibeNotif = item.IdUsuario,
                            Leido = false,
                            Mensaje = "El Usuario " + dbItemUsuarioInactivado.Usuario + " a sido inactivado del sistema"
                        });
                    }

                    dbResponse.Data = 1;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "";
                }
                else
                {
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "";
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = ex.Message;
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
            }
            return dbResponse;
        }


        /// <summary>
        /// Devuelve un permiso por su Id
        /// </summary>
        /// <returns></returns>
        public DBResponse<Permisos> GetPermiso_ById(int IdPermiso)
        {
            return new Usuarios_DA().GetPermiso_ById(IdPermiso);
        }

        /// <summary>
        /// Función que valida los datos de un Usuario
        /// </summary>
        /// <param name="mensaje">Mensaje que regresa</param>
        /// <param name="Usuario">Entidad de Usuario</param>
        /// <returns>DBResponse con el mensaje de error</returns>
        public bool ValidacionesUsuarios(out string mensaje, Usuarios Usuario)
        {
            bool retorno = true;
            mensaje = String.Empty;

            if (String.IsNullOrEmpty(Usuario.Usuario))
            {
                mensaje += "Capture el Usuario<br>";
                retorno = false;
            }
            if (String.IsNullOrEmpty(Usuario.Password))
            {
                mensaje += "Capture la contraseña<br>";
                retorno = false;
            }

            var existe = ExisteUsuario(Usuario.Usuario, Usuario.IdUsuario);

            if (existe.NumRows > 0)
            {
                if (existe.Data.IdUsuario != Usuario.IdUsuario)
                {
                    if (existe.Data.Usuario == Usuario.Usuario)
                    {
                        mensaje += "El Usuario ya Existe<br>";
                        retorno = false;
                    }
                }
            }

            if (Usuario.IdPerfil == null || Usuario.IdPerfil == 0)
            {
                mensaje += "Capture un perfil válido<br>";
                retorno = false;
            }           

            return retorno;
        }
    }
}
