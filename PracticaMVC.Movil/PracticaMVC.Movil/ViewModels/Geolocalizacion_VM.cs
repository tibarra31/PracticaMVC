using GalaSoft.MvvmLight.Command;
using PracticaMVC.Movil.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class Geolocalizacion_VM : BaseViewModel
    {
        private ApiService apiService;

        private string error;
        public string Error
        {
            get { return this.error; }
            set { this.SetValue(ref this.error, value); }
        }

        private double longitud_;
        public double Longitud
        {
            get { return this.longitud_; }
            set { this.SetValue(ref this.longitud_, value); }
        }

        private double latitud_;
        public double Latitud
        {
            get { return this.latitud_; }
            set { this.SetValue(ref this.latitud_, value); }
        }

        #region Singleton
        private static Geolocalizacion_VM instance;

        public static Geolocalizacion_VM GetInstance()
        {
            if (instance == null)
            {
                return new Geolocalizacion_VM();
            }

            return instance;
        }
        #endregion

        public Geolocalizacion_VM()
        {
            instance = this;
            this.apiService = new ApiService();
            this.Error = string.Empty;
            this.Longitud = 0d;
            this.Latitud = 0d;
        }

        #region Comandos

        public ICommand LocalizarCommand
        {
            get { return new RelayCommand(Localizar); }
        }
        public ICommand RegresarCommand
        {
            get { return new RelayCommand(Regresar); }
        }

        public ICommand CancelarCommand
        {
            get { return new RelayCommand(Regresar); }
        }
        public ICommand InicioCommand
        {
            get { return new RelayCommand(Inicio); }
        }

        public ICommand SalirCommand
        {
            get { return new RelayCommand(Salir); }
        }
        #endregion

        #region Metodos
        public async void Regresar()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }
        public async void Inicio()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[2]);
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }

        public void Salir()
        {
            MainViewModel.GetInstance().Salir();
            return;
        }

        private async void Localizar()
        {
            try
            {
                var localizacion = await Geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(25)
                });

                if (localizacion == null)
                {
                    Error = "No se donde estoy";
                    await Application.Current.MainPage.DisplayAlert("Error!", Error, "Aceptar");
                }
                else
                {
                    Longitud = localizacion.Longitude;
                    Latitud = localizacion.Latitude;

                    await Map.OpenAsync(localizacion);
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", e.StackTrace, "Aceptar");
                Console.WriteLine(e.StackTrace);
            }

        }
        #endregion
    }
}
