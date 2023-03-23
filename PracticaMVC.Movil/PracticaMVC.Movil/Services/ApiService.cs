 
namespace PracticaMVC.Movil.Services
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using PracticaMVC.EN;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {
        /*
         * Hace uso del Nuget Xam.Plugin.Connectivity - Se instala en el Proyecto compartido y en las plataformas
         * https://www.nuget.org/packages/Xam.Plugin.Connectivity
         * https://github.com/jamesmontemagno/ConnectivityPlugin
         */
        public async Task<DBResponse<bool>> CheckConnection()
        {
            try
            {
                //Revisa si esta encendida la conexión a internet
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return new DBResponse<bool>
                    {
                        ExecutionOK = false,
                        Message = "Favor de verificar que su conexión a internet este encendida."
                    };
                }

                //Verifica que de verdad hay internet haciendo una consulta a una pagina web
                var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
                if (!isReachable)
                {
                    return new DBResponse<bool>
                    {
                        ExecutionOK = false,
                        Message = "Verifique su conexión a internet."
                    };
                }

                //Si hubo conexion
                return new DBResponse<bool>
                {
                    ExecutionOK = true
                };
            }
            catch (Exception ex)
            {
                return new DBResponse<bool>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }            
        }

        public async Task<DBResponse<T>> PostObj<T>(string urlBase, string prefix, string controller, object model)
        {
            try
            {
                //Convertimos el objeto en un string json
                var request = JsonConvert.SerializeObject(model);
                //Contenido json para el metodo post que se enviara al servicio
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                //Envio de la solicitud al servicio
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();

                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objeto
                var obj = JsonConvert.DeserializeObject<DBResponse<T>>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DBResponse<T>> Get<T>(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                //Envio de la solicitud al servicio
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.GetAsync(url);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();

                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objecto
                var obj = JsonConvert.DeserializeObject<DBResponse<T>>(answer);
                return obj;

            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DBResponse<T>> GetData<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                //Envio de la solicitud al servicio
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.GetAsync(url);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();

                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objecto
                var obj = JsonConvert.DeserializeObject<DBResponse<T>>(answer);
                return obj;

            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }
    }
}
