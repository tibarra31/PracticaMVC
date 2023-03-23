using Acr.Settings;
using GalaSoft.MvvmLight.Command;
using PracticaMVC.EN;
using PracticaMVC.Movil.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class Login_VM : BaseViewModel
    {
        private ApiService apiService;

        private string usuario;
        public string Usuario
        {
            get { return this.usuario; }
            set { this.SetValue(ref this.usuario, value); }
        }

        private string password;
        public string Password
        {
            get { return this.password; }
            set { this.SetValue(ref this.password, value); }
        }

        private bool recordarme;
        public bool Recordarme
        {
            get { return this.recordarme; }
            set { this.SetValue(ref this.recordarme, value); }
        }

        private bool activity;
        public bool Activity
        {
            get { return this.activity; }
            set { this.SetValue(ref this.activity, value); }
        }

        private bool editable;
        public bool Editable
        {
            get { return this.editable; }
            set { this.SetValue(ref this.editable, value); }
        }
        public Login_VM()
        {
            instance = this;
            this.apiService = new ApiService();
            this.Usuario = string.Empty;
            this.Password = string.Empty;
            RevisaDatos();
            Bloqueo(false);
        }

        #region Singleton
        private static Login_VM instance;

        public static Login_VM GetInstance()
        {
            if (instance == null)
            {
                return new Login_VM();
            }
            return instance;
        }
        #endregion

        #region Comandos

        public ICommand IngresarCommand
        {
            get { return new RelayCommand(Ingresar); }
        }
        #endregion

        #region Metodos

        private void Bloqueo(bool bloquear)
        {
            this.Activity = bloquear;
            this.Editable = !bloquear;
        }
        private async void Ingresar()
        {
            //Variable para el tema de las notificaciones push
            string topic = "";
            Bloqueo(true);

            //Implementación de logica de login a la API
            //Revisa la conexión a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.ExecutionOK)
            {
                Bloqueo(false);
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            //Armamos el objeto a enviar
            var usuario = new Usuarios
            {
                Usuario = this.Usuario,
                Password = this.Password
            };

            //Realiza la consulta
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostObj<Usuarios>(url, "/api", "/Login", usuario);
            if (!response.ExecutionOK)
            {
                Bloqueo(false);
                await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                return;
            }
            //seteamos respuesta
            var userdb = response.Data;
            //Obtenemos el usuario
            Bloqueo(false);
            var session = new UsuarioSesion()
            {
                IdPerfil = (int)userdb.IdPerfil,
                IdUsuario = userdb.IdUsuario,
                Perfil = userdb.Perfiles.Perfil,
                Usuario = userdb.Usuario
            };
            MainViewModel.GetInstance().Session = session;

            string datosSesion = userdb.Usuario + "|" + userdb.Password + "|" + userdb.IdUsuario;

            if (this.Recordarme)
                CrossSettings.Current.Set("login", datosSesion);

            //Registramos el tema del usuario para notificaciones push
            topic = "UsuarioID_" + session.IdUsuario;
            //CrossFirebasePushNotification.Current.UnsubscribeAll();
            //CrossFirebasePushNotification.Current.Subscribe(topic);

            MainViewModel.GetInstance().Inicio = new Inicio_VM();
            await Application.Current.MainPage.Navigation.PushAsync(new Inicio());
            return;
        }


        public void RevisaDatos()
        {
            if (CrossSettings.Current.Contains("login"))
            {
                if (CrossSettings.Current.Get<string>("login") != null)
                {
                    string[] datos = CrossSettings.Current.Get<string>("login").ToString().Split('|');
                    this.Usuario = datos[0];
                    this.Password = datos[1];
                    Ingresar();
                }
            }
        }
        #endregion
    }
}
