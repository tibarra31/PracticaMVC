using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PracticaMVC.DA
{
    public class FechasCalendario_DA
    {
        public DBResponse<List<FechasCalendario>> Get_DibujaCalendario(int Mes, int Anio)
        {
            var response = new DBResponse<List<FechasCalendario>>();
            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.FechasCalendario.Include(x=>x.Usuarios)
                                 where u.Fecha.Value.Month == Mes && u.Fecha.Value.Year == Anio
                                 select u).ToList();

                    if (query.Count > 0)
                    {
                        response.Data = query;
                        response.NumRows = 1;
                        response.ExecutionOK = true;
                    }
                    else
                    {
                        response.Data = query;
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

        public DBResponse<List<FechasCalendario>> Get_DibujaCalendarioAll(int IdUsuario)
        {
            var response = new DBResponse<List<FechasCalendario>>();
            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.FechasCalendario.Include(x => x.Usuarios)
                                 where u.IdUsuario == IdUsuario
                                 select u).ToList();

                    if (query.Count > 0)
                    {
                        response.Data = query;
                        response.NumRows = 1;
                        response.ExecutionOK = true;
                    }
                    else
                    {
                        response.Data = query;
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

        /// <summary>
        /// Actualiza/Inserta una fecha calendario
        /// </summary>
        /// <param name="FechasCalendario"></param>
        /// <returns></returns>
        public DBResponse<FechasCalendario> UpsertFechasCalendario(FechasCalendario fechasCalendario)
        {
            var response = new DBResponse<FechasCalendario>();

            using (PracticaMVC_Entities db = new PracticaMVC_Entities())
            {
                var tran = db.Database.BeginTransaction();
                var mensaje = "";
                try
                {
                    if (fechasCalendario.IdFechaCalendario != 0)
                    {
                        var dbItem = db.FechasCalendario.Find(fechasCalendario.IdFechaCalendario);
                        if (dbItem != null)
                        {
                            dbItem.IdFechaCalendario = fechasCalendario.IdFechaCalendario;
                            dbItem.Fecha = fechasCalendario.Fecha;
                            dbItem.Asunto = fechasCalendario.Asunto;
                            dbItem.IdUsuario = fechasCalendario.IdUsuario;
                            dbItem.Activo = dbItem.Activo;
                            mensaje = "Agenda Modificada Correctamente";
                        } 
                    }
                    else
                    {
                        var dbItemMax = db.FechasCalendario.Max(x => x.IdFechaCalendario);
                        dbItemMax++;

                        fechasCalendario.IdFechaCalendario = dbItemMax;
                        fechasCalendario.Activo = true;
                        db.FechasCalendario.Add(fechasCalendario);
                        mensaje = "Agenda Agregada Correctamente";
                    }
                    db.SaveChanges();
                    tran.Commit();

                    response.Message = mensaje;
                    response.Data = fechasCalendario;
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

        public DBResponse<FechasCalendario> GetInfoFechaCalendario(int Id)
        {
            var response = new DBResponse<FechasCalendario>();
            try
            {
                using (PracticaMVC_Entities db = new PracticaMVC_Entities())
                {
                    var query = (from u in db.FechasCalendario.Include(x => x.Usuarios)
                                 where u.IdFechaCalendario == Id
                                 select u).FirstOrDefault();

                    response.Data = query == null ? new FechasCalendario() : query;
                    response.NumRows = query == null ?  0 : 1;
                    response.ExecutionOK = query != null;
                    response.Message = query == null ? "Sin información" : "";
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
    }
}
