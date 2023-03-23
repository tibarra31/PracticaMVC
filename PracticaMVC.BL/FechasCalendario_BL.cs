using Newtonsoft.Json;
using PracticaMVC.DA;
using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL
{
    public class FechasCalendario_BL
    {
        public DBResponse<List<CalendarDTO>> Get_DibujaCalendario(int Mes, int Anio)
        {
            var dbResponse = new DBResponse<List<CalendarDTO>>();
            IList<CalendarDTO> taskSalidaList = new List<CalendarDTO>();
            try
            {
                var foreColor = "#035bb9";
                var responseBD = new FechasCalendario_DA().Get_DibujaCalendario(Mes, Anio);
                if (responseBD.ExecutionOK)
                {
                    foreach (var item in responseBD.Data)
                    {
                        foreColor = "#035bb9";
                        if (!(bool)item.Activo)
                        {
                            foreColor = "#ff0000";
                        }

                        CalendarDTO calendario = new CalendarDTO()
                        {
                            id = item.IdFechaCalendario,
                            title = item.Usuarios.Usuario,
                            end = item.Fecha.Value.ToString("yyyy-MM-dd"),
                            start = item.Fecha.Value.ToString("yyyy-MM-dd"),
                            url = "",
                            color = foreColor
                        };
                        taskSalidaList.Add(calendario);
                    }

                    dbResponse.Data = taskSalidaList.ToList();
                    dbResponse.ExecutionOK = taskSalidaList.Count > 0;
                    dbResponse.NumRows = taskSalidaList.Count;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Data = new List<CalendarDTO>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<List<FechasCalendario>> Get_UsuariosCalendario(int idUsuario)
        {
            return new FechasCalendario_DA().Get_DibujaCalendarioAll(idUsuario);
        }

        public DBResponse<FechasCalendario> UpsertFechasCalendario(FechasCalendario fechasCalendario)
        {
            return new FechasCalendario_DA().UpsertFechasCalendario(fechasCalendario);
        }
        public DBResponse<FechasCalendario> GetInfoFechaCalendario(int Id)
        {
            return new FechasCalendario_DA().GetInfoFechaCalendario(Id);
        }
    }
}
