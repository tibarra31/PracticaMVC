using GalaSoft.MvvmLight.Command;
using PracticaMVC.EN;
using PracticaMVC.Movil.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace PracticaMVC.Movil.ViewModels
{
    public class UsuariosCalendario_VM : BaseViewModel
    {
        private ApiService apiService;

        private ObservableCollection<UsuarioCalendarioItem_VM> getUsuariosCalendario;
        public ObservableCollection<UsuarioCalendarioItem_VM> GetUsuariosCalendarioList
        {
            get { return this.getUsuariosCalendario; }
            set { this.SetValue(ref this.getUsuariosCalendario, value); }
        }
        public List<FechasCalendario> fechasCalendarios { get; set; }
        public EventCollection Events { get; } = new EventCollection();

        private int _month = DateTime.Today.Month;
        public int Month
        {
            get { return _month; }
            set { SetValue(ref _month, value); }
        }
        private int _year = DateTime.Today.Year;
        public int Year
        {
            get { return _year; }
            set { SetValue(ref _year, value); }
        }

        private DateTime? _selectedDate = DateTime.Today;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { SetValue(ref _selectedDate, value); }
        }

        private DateTime _minimumDate = new DateTime(2019, 4, 29);

        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { SetValue(ref _minimumDate, value); }
        }

        private DateTime _maximumDate = DateTime.Today.AddMonths(5);

        public DateTime MaximumDate
        {
            get { return _maximumDate; }
            set { SetValue(ref _maximumDate, value); }
        }

        #region Comandos
        public ICommand TodayCommand => new Command(() =>
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        });


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
        public ICommand AgregarCommand
        {
            get { return new RelayCommand(Agregar); }
        }
        #endregion
        #region Singleton
        private static UsuariosCalendario_VM instance;

        public static UsuariosCalendario_VM GetInstance()
        {
            if (instance == null)
            {
                return new UsuariosCalendario_VM();
            }

            return instance;
        }
        #endregion
        #region Metodos
        public UsuariosCalendario_VM() : base()
        {
            instance = this;
            this.fechasCalendarios = new List<FechasCalendario>();
            this.apiService = new ApiService();
            this.Load();
        }
        public async void Load()
        {
            try
            {
                //Device.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert("Info", "Loading events with delay, and changeing current view.", "Ok"));

                //Revisa la conexión a internet            
                var connection = await this.apiService.CheckConnection();
                if (!connection.ExecutionOK)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", connection.Message, "Aceptar");
                    return;
                }
                var sesion_ = MainViewModel.GetInstance().Session;
                //Obtenemos el Usuario
                var filtros = new FiltroUsuario()
                {
                    NombreUsuario = "",
                    IdUsuario = sesion_.IdUsuario
                };

                //Realiza la consulta
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.PostObj<List<EN.FechasCalendario>>(url, "/api", "/GetUsuarioCalendario", filtros);
                if (!response.ExecutionOK)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                    return;
                }

                this.fechasCalendarios = response.Data;

                this.RefreshList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "Aceptar");
                return;
            }

        }

        public void RefreshList()
        {
            var list = this.fechasCalendarios.Select(s => new UsuarioCalendarioItem_VM
            {
                IdUsuario = s.IdUsuario,
                Activo = s.Activo,
                Asunto = s.Asunto,
                Fecha = DateTime.Parse(s.Fecha.Value.ToShortDateString()),
                IdFechaCalendario = s.IdFechaCalendario,
                Usuarios = s.Usuarios
            });

            this.GetUsuariosCalendarioList = new ObservableCollection<UsuarioCalendarioItem_VM>(list.OrderByDescending(o => o.IdUsuario));
            if (GetUsuariosCalendarioList.Count > 0)
            {

                var listDate_ = GetUsuariosCalendarioList.GroupBy(x => new { x.Fecha.Value })
                                                        .Select(w => new EventModelDate
                                                        {
                                                            Fecha = DateTime.Parse(w.FirstOrDefault().Fecha.Value.ToShortDateString())
                                                        }).ToList();

                // indexer - update later
                foreach (var item in listDate_)
                {
                    var listItems = GetUsuariosCalendarioList.Where(x => x.Fecha == item.Fecha).ToList();
                    Events.Add(item.Fecha, new ObservableCollection<EventModel>(GenerateEvents(listItems)));
                }
            }
        }

        private IEnumerable<EventModel> GenerateEvents(List<UsuarioCalendarioItem_VM> fechaCalendario)
        {
            var list = fechaCalendario.Select(s => new EventModel
            {
                Name = $"{s.Usuarios.Usuario}",
                Description = $"{s.Asunto}",
                IdFechaCalendario = s.IdFechaCalendario,
                IdUsuario = (int)s.IdUsuario,
                Fecha = DateTime.Parse(s.Fecha.Value.ToShortDateString())
            });

            return list;
        }

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
        public async void Agregar()
        {
            var sesion_ = MainViewModel.GetInstance().Session;

            MainViewModel.GetInstance().UsuariosCalendarioDetalle = new UsuarioCalendarioDetalle_VM(new EN.FechasCalendario
            {
                IdFechaCalendario = 0,
                Asunto = "",
                IdUsuario = sesion_.IdUsuario,
                Usuarios = new Usuarios() { Usuario = sesion_.Usuario },
                Fecha = DateTime.Parse(DateTime.Now.ToShortDateString())
            });
            await Application.Current.MainPage.Navigation.PushAsync(new AdminUsuarioCalendarioFecha());
            return;
        }


        #endregion
    }
}
