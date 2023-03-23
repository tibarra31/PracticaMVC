using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PracticaMVC.DA
{
    public class Notificaciones_DA
    {
        public DBResponse<List<Notificaciones_Usuarios>> GetListNotificaUsuarioRecibe(int idUsuarioRecibeNotificacion)
        {
            var response = new DBResponse<List<Notificaciones_Usuarios>>();
            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.Notificaciones_Usuarios.Include(x => x.Usuarios)
                                 where u.IdUsuarioRecibeNotif == idUsuarioRecibeNotificacion
                                 && u.Leido == false
                                 select u).ToList();

                    if (query.Count > 0)
                    {
                        response.Data = query;
                        response.NumRows = query.Count;
                        response.ExecutionOK = true;
                    }
                    else
                    {
                        response.Data = new List<Notificaciones_Usuarios>();
                        response.NumRows = 0;
                        response.ExecutionOK = false;
                        response.Message = "Sin información";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public DBResponse<Boolean> InsertNotificacionUsuario(Notificaciones_Usuarios notificaciones)
        {
            var response = new DBResponse<Boolean>();

            using (PracticaMVC_Entities db = new PracticaMVC_Entities())
            {
                var tran = db.Database.BeginTransaction();
                var mensaje = "";
                try
                {
                    notificaciones.Leido = false;
                    db.Notificaciones_Usuarios.Add(notificaciones);
                   
                    db.SaveChanges();
                    tran.Commit();

                    response.Message = mensaje;
                    response.Data = true;
                    response.NumRows = 1;
                    response.ExecutionOK = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Data = false;
                    response.ExecutionOK = false;
                    response.Message = ex.Message;
                }
            }

            return response;
        }
        public DBResponse<Boolean> UpdateNotificacionUsuarioMarcaLeida(int IdNotificacion)
        {
            var response = new DBResponse<Boolean>();

            using (PracticaMVC_Entities db = new PracticaMVC_Entities())
            {
                var tran = db.Database.BeginTransaction();
                var mensaje = "";
                try
                {
                    var dbItem = db.Notificaciones_Usuarios.Find(IdNotificacion);
                    dbItem.Leido = true;

                    db.SaveChanges();
                    tran.Commit();

                    response.Message = mensaje;
                    response.Data = true;
                    response.NumRows = 1;
                    response.ExecutionOK = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    response.Data = true;
                    response.ExecutionOK = false;
                    response.Message = ex.Message;
                }
            }

            return response;
        }



    }
}
