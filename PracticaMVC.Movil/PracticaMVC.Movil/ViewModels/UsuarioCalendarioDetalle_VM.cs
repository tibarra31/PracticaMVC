using GalaSoft.MvvmLight.Command;
using PracticaMVC.EN;
using PracticaMVC.Movil.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class UsuarioCalendarioDetalle_VM : BaseViewModel
    {
        private ApiService apiService;

        private EN.FechasCalendario fechas;
        public EN.FechasCalendario Fechas
        {
            get { return this.fechas; }
            set { this.SetValue(ref this.fechas, value); }
        }

        #region Singleton
        private static UsuarioCalendarioDetalle_VM instance;

        public static UsuarioCalendarioDetalle_VM GetInstance(EN.FechasCalendario fechasCalendario)
        {
            if (instance == null)
            {
                return new UsuarioCalendarioDetalle_VM(fechasCalendario);
            }

            return instance;
        }
        #endregion

        #region Comandos
        public ICommand CancelarCommand
        {
            get { return new RelayCommand(Regresar); }
        }
        public ICommand RegresarCommand
        {
            get { return new RelayCommand(Regresar); }
        }
        public ICommand SalirCommand
        {
            get { return new RelayCommand(Salir); }
        }

        public ICommand RegistrarCommand
        {
            get { return new RelayCommand(Agregar); }
        }

        public ICommand InicioCommand
        {
            get { return new RelayCommand(Regresar); }
        }

        #endregion

        public UsuarioCalendarioDetalle_VM(EN.FechasCalendario fechasCalendario)
        {
            instance = this;
            apiService = new ApiService();
            Fechas = new FechasCalendario();
            Fechas = fechasCalendario;
        }

        #region Metodos

        public async void Regresar()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }

        public async void Agregar()
        {
            //Revisa la conexión a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.ExecutionOK)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            try
            {
                var fechasCalendarioUpsert_ = new FechasCalendario()
                {
                    Activo = true,
                    IdFechaCalendario = this.Fechas.IdFechaCalendario,
                    Asunto = this.Fechas.Asunto,
                    Fecha = this.Fechas.Fecha,
                    IdUsuario = this.Fechas.IdUsuario
                };
                //Realiza el registro de solicitud
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.PostObj<FechasCalendario>(url, "/api", "/RegisterDatesUser", fechasCalendarioUpsert_);
                if (!response.ExecutionOK)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                    return;
                }

                await Application.Current.MainPage.DisplayAlert("Mensaje de Sistema", response.Message, "Aceptar");

                MainViewModel.GetInstance().UsuariosCalendario.Load();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje de Sistema", ex.Message, "Aceptar");
            }

            return;
        }
        public void Salir()
        {
            MainViewModel.GetInstance().Salir();
            return;
        }
        #endregion
    }
}
