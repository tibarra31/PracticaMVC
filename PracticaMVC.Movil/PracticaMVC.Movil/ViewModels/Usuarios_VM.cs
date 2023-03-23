using GalaSoft.MvvmLight.Command;
using PracticaMVC.EN;
using PracticaMVC.Movil.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class Usuarios_VM : BaseViewModel
    {
        private ApiService apiService;
        public string NombreUsuario { get; set; }
        public List<Usuarios> ListaUsuarios { get; set; }

        private ObservableCollection<UsuarioItem_VM> getUsuarios;
        public ObservableCollection<UsuarioItem_VM> GetUsuariosList
        {
            get { return this.getUsuarios; }
            set { this.SetValue(ref this.getUsuarios, value); }
        }


        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public Usuarios_VM()
        {
            instance = this;
            this.apiService = new ApiService();
            this.Load();
        }
        #region Singleton
        private static Usuarios_VM instance;

        public static Usuarios_VM GetInstance()
        {
            if (instance == null)
            {
                return new Usuarios_VM();
            }

            return instance;
        }
        #endregion

        #region Comandos

        public ICommand RegresarCommand
        {
            get { return new RelayCommand(Regresar); }
        }

        public ICommand AgregarCommand
        {
            get { return new RelayCommand(Agregar); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand(Load); }
        }

        public ICommand InicioCommand
        {
            get { return new RelayCommand(Regresar); }
        }
        public ICommand UsuariosGraficasCommand
        {
            get { return new RelayCommand(UsuariosGraficas); }
        }

        public ICommand SalirCommand
        {
            get { return new RelayCommand(Salir); }
        }

        #endregion

        #region Metodos
        public async void UsuariosGraficas()
        {
            MainViewModel.GetInstance().UsuariosGraficas = new UsuariosGraficas_VM(ListaUsuarios);
            await Application.Current.MainPage.Navigation.PushAsync(new UsuariosGraficas());
            return;
        }
        public async void Regresar()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }

        public async void Agregar()
        {
            MainViewModel.GetInstance().UsuariosDetalle = new UsuariosDetalle_VM(new EN.Usuarios { IdUsuario = 0 });
            await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuario());
            return;
        }

        public async void Load()
        {
            this.IsRefreshing = true;

            //Revisa la conexión a internet            
            var connection = await this.apiService.CheckConnection();
            if (!connection.ExecutionOK)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error!", connection.Message, "Aceptar");
                return;
            }

            //Obtenemos el Usuario
            FiltroUsuario filtros = new FiltroUsuario
            {
                NombreUsuario = this.NombreUsuario,
                IdUsuario = 0
            };

            //Realiza la consulta
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostObj<List<EN.Usuarios>>(url, "/api", "/GetUsuarios", filtros);
            if (!response.ExecutionOK)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                return;
            }

            this.ListaUsuarios = response.Data;

            this.RefreshList();
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            var list = this.ListaUsuarios.Select(s => new UsuarioItem_VM
            {
                IdUsuario = s.IdUsuario,
                Activo = s.Activo,
                PerfilUsuario = s.Perfiles.Perfil,
                EstatusUsuario = s.EstatusRegistros.EstatusRegistro,
                Usuario = s.Usuario,
                IdEstatusRegistro = s.IdEstatusRegistro,
                IdPerfil = s.IdPerfil,
                Password = s.Password,
                IdPerfilRol = s.IdPerfilRol,
                ImagenUsuario = s.ImagenUsuario
            });

            this.GetUsuariosList = new ObservableCollection<UsuarioItem_VM>(list.OrderByDescending(o => o.IdUsuario));
        }

        public void Salir()
        {
            MainViewModel.GetInstance().Salir();
            return;
        }



        #endregion

    }
}
