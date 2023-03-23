namespace PracticaMVC.Movil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using PracticaMVC.EN;
    using Rg.Plugins.Popup.Services;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class MainViewModel : BaseViewModel
    {
        public UsuarioSesion Session { get; set; }
        public MainViewModel()
        {
            instance = this;
            this.Login = new Login_VM();
        }

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion
        public Login_VM Login { get; set; }
        public Inicio_VM Inicio { get; set; }
        public Logout_VM Logout { get; set; }
        public Usuarios_VM Usuarios { get; set; }
        public UsuariosGraficas_VM UsuariosGraficas { get; set; }
        public UsuariosDetalle_VM UsuariosDetalle { get; set; }
        public Geolocalizacion_VM Geolocalizacion { get; set; }
        public UsuariosCalendario_VM UsuariosCalendario { get; set; }
        public UsuarioCalendarioDetalle_VM UsuariosCalendarioDetalle { get; set; }

        #region Comandos
        public ICommand InicioCommand
        {
            get { return new RelayCommand(Inicioc); }
        }
        public ICommand SalirCommand
        {
            get { return new RelayCommand(Salir); }
        }

        #endregion

        public async void Inicioc()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
            return;
        }
        public async void Salir()
        {
            this.Logout = new Logout_VM();
            await PopupNavigation.Instance.PushAsync(new Logout());
            return;
        }

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is EventModel eventModel)
            {
                MainViewModel.GetInstance().UsuariosCalendarioDetalle = new UsuarioCalendarioDetalle_VM(new EN.FechasCalendario
                {
                    IdFechaCalendario = eventModel.IdFechaCalendario,
                    Asunto = eventModel.Description,
                    IdUsuario = eventModel.IdUsuario,
                    Usuarios = new Usuarios() { Usuario = eventModel.Name },
                    Fecha = eventModel.Fecha
                });
                await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuarioCalendarioFecha());
                return;
            }
        }
    }
}
