using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using PracticaMVC.DA;
using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL
{
    public class Notificaciones_BL
    {
        Notificaciones_DA response = new Notificaciones_DA();
        public DBResponse<List<Notificaciones_Usuarios>> GetListNotificaUsuarioRecibe(int idUsuarioRecibeNotificacion)
        {
            string error = "";
            var dbRespone = new DBResponse<List<Notificaciones_Usuarios>>();
            try
            {
                dbRespone = response.GetListNotificaUsuarioRecibe(idUsuarioRecibeNotificacion);
                if (dbRespone.ExecutionOK)
                {
                    foreach (var item in dbRespone.Data)
                    {
                        var enviaNotificacion = Enviar_Notificacion(item.Mensaje, item.Usuarios, item.IdNotificacion, out error);
                        if (enviaNotificacion)
                        {
                            var marcaEnviada = (new Notificaciones_BL()).UpdateNotificacionUsuarioMarcaLeida(item.IdNotificacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbRespone.ExecutionOK = false;
                dbRespone.Message = ex.Message;
                dbRespone.Data = new List<Notificaciones_Usuarios>();
            }
            return dbRespone;
        }

        public DBResponse<Boolean> InsertNotificacionUsuario(Notificaciones_Usuarios notificaciones)
        { 
            return response.InsertNotificacionUsuario(notificaciones);
        }
        public DBResponse<Boolean> UpdateNotificacionUsuarioMarcaLeida(int IdNotificacion)
        {
            return response.UpdateNotificacionUsuarioMarcaLeida(IdNotificacion);
        }

        public bool Enviar_Notificacion(string mensaje, Usuarios usuario, int idNotificacion, out string error)
        {
            bool result = false;
            string msj = "";
            error = "";

            try
            {
                string condicion = "'UsuarioID_" + usuario.IdUsuario + "' in topics";
                var task = Task.Run(() => Send_NotificationPush(condicion, idNotificacion, mensaje));
                task.Wait();
                if (!task.Result)
                    msj += "- " + usuario.Usuario + "<br />";


                if (string.IsNullOrEmpty(msj))
                    result = true;
                else
                    error = "No se pudo enviar la notificación a los siguientes clientes: <br />" + msj;

            }
            catch (Exception ex)
            {
                result = false;
                error = ex.Message;
            }

            return result;
        }

        private async Task<bool> Send_NotificationPush(string condicion, int idNotificacion, string mensaje)
        {
            bool result = true;

            try
            {
                FirebaseApp fcm;

                var instanceFireBase = FirebaseApp.GetInstance("[DEFAULT]");

                if (instanceFireBase == null)
                {
                    fcm = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.GetApplicationDefault(),
                        ProjectId = "practicamvc-xamarin"
                    });
                }
                else
                    fcm = instanceFireBase;

                string condition = condicion;

                var message = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = "Notificación del Sistema!",
                        Body = mensaje,
                    },
                    Condition = condition,
                    Data = new Dictionary<string, string>()
                    {
                        { "IdNotificacion" , idNotificacion.ToString() }
                    }
                };

                // Send a message to devices subscribed to the combination of topics
                // specified by the provided condition.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
