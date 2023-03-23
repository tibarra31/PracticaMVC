using GalaSoft.MvvmLight.Command;
using PracticaMVC.Movil.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class UsuarioItem_VM : EN.Usuarios
    {
        private ApiService apiService;

        public ICommand DetallesCommand
        {
            get { return new RelayCommand(Detalles); }
        }

        public ICommand EliminarCommand
        {
            get { return new RelayCommand(Eliminar); }
        }

        private async void Detalles()
        {
            MainViewModel.GetInstance().UsuariosDetalle = new UsuariosDetalle_VM(this);
            await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuario());
            return;
        }

        private async void Eliminar()
        {
            this.apiService = new ApiService();

            MainViewModel.GetInstance().UsuariosDetalle = new UsuariosDetalle_VM(this);
            //Revisa la conexión a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.ExecutionOK)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            try
            {
                var user = MainViewModel.GetInstance().UsuariosDetalle.Usuarios;
                //Realiza el registro de solicitud
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.PostObj<int>(url, "/api", "/InactiveUser", user);
                if (!response.ExecutionOK)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                    return;
                }

                await Application.Current.MainPage.DisplayAlert("Mensaje de Sistema", response.Message, "Aceptar");

                MainViewModel.GetInstance().Usuarios.Load();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje de Sistema", ex.Message, "Aceptar");
            }

            return;
        }
    }
}
