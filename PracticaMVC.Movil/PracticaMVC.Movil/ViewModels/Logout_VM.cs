using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class Logout_VM
    {
        public Logout_VM()
        {
            instance = this;
        }

        #region Singleton
        private static Logout_VM instance;

        public static Logout_VM GetInstance()
        {
            if (instance == null)
            {
                return new Logout_VM();
            }
            return instance;
        }
        #endregion

        #region Comandos

        public ICommand CerrarCommand
        {
            get { return new RelayCommand(Cerrar); }
        }

        public ICommand CerrarSesionCommand
        {
            get { return new RelayCommand(CerrarSesion); }
        }

        #endregion

        #region Metodos

        private void Cerrar()
        {
            this.OnClose(null, null);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public async void CerrarSesion()
        {
            MainViewModel.GetInstance().Login.Usuario = string.Empty;
            MainViewModel.GetInstance().Login.Password = string.Empty;
            MainViewModel.GetInstance().Login.Recordarme = false;

            //CrossSettings.Current.Remove("login");
            ////Se elimina el tema para las notificaciones push
            //CrossFirebasePushNotification.Current.UnsubscribeAll();

            Cerrar();
            MainViewModel.GetInstance().Session = null;
            await Application.Current.MainPage.Navigation.PopToRootAsync();
            return;
        }

        #endregion
    }
}
