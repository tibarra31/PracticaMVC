using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PracticaMVC.DA
{
    public class Usuarios_DA
    {
        public DBResponse<Usuarios> ValidaUsuarioLogin(string usuario, string password)
        {
            var dbResponse = new DBResponse<Usuarios>();
            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var dbItem = (from tbl in db.Usuarios.Include("EstatusRegistros").Include("Perfiles")
                                  where tbl.Usuario == usuario && tbl.Password == password
                                  select tbl).FirstOrDefault();

                    dbResponse.ExecutionOK = dbItem != null;
                    dbResponse.Message = dbItem == null ? "Usuario y/o Contraseña incorrectas" : "Información Encontrada";
                    dbResponse.Data = dbItem != null ? dbItem : new Usuarios() ;
                };
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
                dbResponse.Data = null;
            }
            return dbResponse;
        }
        /// <summary>
        /// Devuelve el listado de usuarios
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuarios(string usuario, string sortOrder, int pageNumber, int pageSize)
        {
            var response = new DBResponse<List<Usuarios>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    sortOrder = string.IsNullOrEmpty(sortOrder) ? "IdUsuario" : sortOrder;
                    usuario = string.IsNullOrEmpty(usuario)  ? "" : usuario;

                    var query = (from u in db.Usuarios.Include(x => x.EstatusRegistros)
                                 .Include(x=>x.PerfilesRoles)
                                 .Include(x=>x.Perfiles)
                                 where u.Usuario.Contains(usuario) && u.Activo == true
                                 select u).OrderBy(sortOrder).Skip(pageSize * pageNumber)
                                 .Take(pageSize).ToList();

                    int totalCount = (from u in db.Usuarios
                                      where  u.Activo == true
                                      select u).ToList().Count;

                    response.Data = query;
                    response.NumRows = totalCount;
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
        /// Devuelve el listado de usuarios en pantalla Angular
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuariosAngular(Filtros_Usuarios filtrosUsuarios)
        {
            var response = new DBResponse<List<Usuarios>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var nombre = string.IsNullOrEmpty(filtrosUsuarios.filtroNombreUsuario) ? "" : filtrosUsuarios.filtroNombreUsuario;
                    var usuario = string.IsNullOrEmpty(filtrosUsuarios.filtroUsuarios) ? "" : filtrosUsuarios.filtroUsuarios;

                    var query = (from u in db.Usuarios.Include(x => x.EstatusRegistros)
                                 .Include(x => x.PerfilesRoles)
                                 .Include(x => x.Perfiles)
                                 where u.Usuario.Contains(usuario) && u.Activo == true
                                 select u).ToList();

                    int totalCount = (from u in db.Usuarios
                                      where  u.Activo == true
                                      select u).ToList().Count;

                    response.Data = query;
                    response.NumRows = totalCount;
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
        /// Devuelve el listado de usuarios
        /// </summary>
        /// <returns></returns>
        public DBResponse<List<Usuarios>> GetUsuarios_List(string usuario)
        {
            var response = new DBResponse<List<Usuarios>>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    usuario = string.IsNullOrEmpty(usuario)  ? "" : usuario;
                    var query = (from u in db.Usuarios.Include(x => x.EstatusRegistros)
                                 .Include(x => x.PerfilesRoles)
                                 .Include(x => x.Perfiles)
                                 .Include(x => x.Usuarios_Visitados)
                                 where u.Usuario.Contains(usuario)
                                 select u).ToList();

                    int totalCount = (from u in db.Usuarios
                                      select u).ToList().Count;

                    response.Data = query;
                    response.NumRows = totalCount;
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
        /// Devuelve un usuario por su Id
        /// </summary>
        /// <returns></returns>
        public DBResponse<Usuarios> GetUsuarios_ById(int IdUsuario)
        {
            var response = new DBResponse<Usuarios>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.Usuarios.Include(x => x.EstatusRegistros)
                                 .Include(x => x.PerfilesRoles)
                                 .Include(x => x.Perfiles)
                                 .Include(x => x.Usuarios_Permisos)
                                 .Include("Usuarios_Permisos.Permisos")
                                 where u.IdUsuario == IdUsuario && u.Activo == true
                                 select u).FirstOrDefault();

                    response.Data = query;
                    response.NumRows = query == null ? 0 : 1;
                    response.ExecutionOK = query == null ? false : true;
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
        /// Valida que el usuario ingresado no exista ya en la BD
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public DBResponse<Usuarios> ExisteUsuario(string usuario, int idUsuario)
        {
            var response = new DBResponse<Usuarios>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var users = (from u in db.Usuarios
                                 where u.Usuario == usuario
                                    && u.IdUsuario != idUsuario
                                 select u).FirstOrDefault();

                    response.Data = users;
                    response.NumRows = users == null ? 0 : 1;
                    response.ExecutionOK = users != null;
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
        /// Actualiza/Inserta un usuario 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public DBResponse<Usuarios> UpsertUsuario(Usuarios usuario)
        {
            var response = new DBResponse<Usuarios>();

            using (PracticaMVC_Entities db = new PracticaMVC_Entities())
            {
                var tran = db.Database.BeginTransaction();
                var mensaje = "";
                try
                {
                    if (usuario.IdUsuario != 0)
                    {
                        var dbItem = db.Usuarios.Find(usuario.IdUsuario);
                        if (dbItem != null)
                        {
                            dbItem.Usuario = usuario.Usuario;
                            dbItem.Password = usuario.Password;
                            dbItem.IdPerfil = usuario.IdPerfil;
                            dbItem.IdPerfilRol = usuario.IdPerfilRol;
                            dbItem.ImagenUsuario = usuario.ImagenUsuario;
                            if (dbItem.IdEstatusRegistro != usuario.IdEstatusRegistro)
                            {
                                var dbItemEstatus = db.EstatusRegistros.Find(usuario.IdEstatusRegistro).EstatusRegistro;
                                mensaje = "Usuario " + dbItemEstatus + " Correctamente";
                            }
                            dbItem.IdEstatusRegistro = usuario.IdEstatusRegistro;
                            dbItem.Activo = usuario.Activo;

                            var deleteItems = (from permisos in db.Usuarios_Permisos
                                               where permisos.IdUsuario == dbItem.IdUsuario
                                               select permisos).ToList();

                            foreach (var item in deleteItems)
                            {
                                var delete = db.Usuarios_Permisos.Remove(item);
                            }
                            foreach (var item in usuario.Usuarios_Permisos)
                            {
                                db.Usuarios_Permisos.Add(item);
                            }
                            mensaje = "Usuario Modificado Correctamente";
                        }
                        else
                        {
                            throw new Exception("Usuario no encontrado");
                        }
                    }
                    else
                    {
                        usuario.Activo = true;
                        usuario.IdEstatusRegistro = 1;
                        db.Usuarios.Add(usuario);
                        mensaje = "Usuario Agregado Correctamente";
                    }
                    db.SaveChanges();
                    tran.Commit();

                    response.Message = mensaje;
                    response.Data = usuario;
                    response.NumRows = 1;
                    response.ExecutionOK = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Data = null;
                    response.ExecutionOK = false;
                    response.Message = ex.Message;
                }
            }

            return response;
        }
        /// <summary>
        /// Elimina de manera logica al usuario seleccionado
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public DBResponse<DBNull> InactivarEliminarUsuario(int idUsuario)
        {
            var response = new DBResponse<DBNull>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var mensaje = "";
                    var query = (from u in db.Usuarios.Include(x => x.EstatusRegistros)
                                 where u.IdUsuario == idUsuario //&& u.Activo == true
                                 select u).FirstOrDefault();

                    var dbItemResponse = db.Usuarios.Find(idUsuario);

                    int idEstatusRegistro = 0;
                    if (query.IdEstatusRegistro == 1)
                    {
                        idEstatusRegistro = 2;
                        var dbItemEstatus = db.EstatusRegistros.Find(idEstatusRegistro).EstatusRegistro;
                        mensaje = "Usuario " + dbItemEstatus + " Correctamente";
                        dbItemResponse.Activo = false;
                        dbItemResponse.IdEstatusRegistro = 2;
                    }
                    else
                    {
                        idEstatusRegistro = 1;
                        var dbItemEstatus = db.EstatusRegistros.Find(idEstatusRegistro).EstatusRegistro;
                        mensaje = "Usuario " + dbItemEstatus + " Correctamente";
                        dbItemResponse.Activo = true;
                        dbItemResponse.IdEstatusRegistro = 1;
                    }                   

                    db.SaveChanges();

                    response.ExecutionOK = true;
                    response.Data = null;
                    response.Message = mensaje;
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
        /// Devuelve un permiso por su Id
        /// </summary>
        /// <returns></returns>
        public DBResponse<Permisos> GetPermiso_ById(int IdPermiso)
        {
            var response = new DBResponse<Permisos>();

            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.Permisos
                                 where u.IdPermiso == IdPermiso 
                                 select u).FirstOrDefault();

                    response.Data = query;
                    response.NumRows = query == null ? 0 : 1;
                    response.ExecutionOK = query == null ? false : true;
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
