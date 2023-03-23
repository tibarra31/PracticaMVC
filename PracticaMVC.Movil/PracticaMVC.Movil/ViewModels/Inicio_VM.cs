using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class Inicio_VM
    {
        public Inicio_VM()
        {
            instance = this;
        }

        #region Singleton
        private static Inicio_VM instance;

        public static Inicio_VM GetInstance()
        {
            if (instance == null)
            {
                return new Inicio_VM();
            }
            return instance;
        }
        #endregion

        public ICommand UsuariosCommand
        {
            get { return new RelayCommand(Usuarios); }
        }
        public ICommand LocalizacionCommand
        {
            get { return new RelayCommand(GeoLocalizacion); }
        }
        public ICommand UsuariosCalendarioCommand
        {
            get { return new RelayCommand(GeoUsuariosCalendario); }
        }
        private async void Usuarios()
        {
            MainViewModel.GetInstance().Usuarios = new Usuarios_VM();
            await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuarios());
            return;
        }
        private async void GeoLocalizacion()
        {
            MainViewModel.GetInstance().Geolocalizacion = new Geolocalizacion_VM();
            await Application.Current.MainPage.Navigation.PushAsync(new Localizacion());
            return;
        }
        private async void GeoUsuariosCalendario()
        {
            MainViewModel.GetInstance().UsuariosCalendario = new UsuariosCalendario_VM();
            await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuarioCalendario());
            return;
        }
    }
}
