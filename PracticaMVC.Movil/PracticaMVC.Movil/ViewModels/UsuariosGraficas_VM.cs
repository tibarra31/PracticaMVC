using GalaSoft.MvvmLight.Command;
using Microcharts;
using PracticaMVC.EN;
using PracticaMVC.Movil.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class UsuariosGraficas_VM : BaseViewModel
    {
        private ApiService apiService;
        private List<EN.Usuarios> usuarios;
        public List<EN.Usuarios> Usuarios
        {
            get { return this.usuarios; }
            set { this.SetValue(ref this.usuarios, value); }
        }

        private ObservableCollection<ChartEntry> listEntriesChart;
        public ObservableCollection<ChartEntry> ListEntriesChart
        {
            get { return this.listEntriesChart; }
            set { this.SetValue(ref this.listEntriesChart, value); }
        }

        private DonutChart donutChart;
        public DonutChart DonutChart
        {
            get { return this.donutChart; }
            set { this.SetValue(ref this.donutChart, value); }
        }
        private LineChart barChartLine;
        public LineChart BarChartLine
        {
            get { return this.barChartLine; }
            set { this.SetValue(ref this.barChartLine, value); }
        }

        private BarChart barChartSimple;
        public BarChart BarChartSimple
        {
            get { return this.barChartSimple; }
            set { this.SetValue(ref this.barChartSimple, value); }
        }

        private PointChart pointChart;
        public PointChart PointChart
        {
            get { return this.pointChart; }
            set { this.SetValue(ref this.pointChart, value); }
        }

        public UsuariosGraficas_VM(List<EN.Usuarios> user)
        {
            instance = this;
            Random rnd = new Random();

            this.apiService = new ApiService();
            this.Usuarios = user;
            ListEntriesChart = new ObservableCollection<ChartEntry>();
            var allVisited = user.Sum(x => x.Usuarios_Visitados.Sum(y => y.NumVisitas));

            foreach (var itemUser in user)
            {
                Color randomColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                var colorValue_ = randomColor.ToHex();

                var visitados = itemUser.Usuarios_Visitados.Sum(x => x.NumVisitas);
                float percent = float.Parse(visitados.ToString()) / float.Parse(allVisited.ToString()) * 100;
                float percentDecimal = float.Parse(percent.ToString("0.000"));

                var newEntry = new ChartEntry(percentDecimal)
                {
                    Color = SKColor.Parse(colorValue_),
                    TextColor = SKColor.Parse(colorValue_),
                    Label = itemUser.Usuario.Substring(0, 5),
                    ValueLabel = percentDecimal + "%",
                    ValueLabelColor = SKColor.Parse(colorValue_)
                };

                ListEntriesChart.Add(newEntry);
            }
            BarChartLine = new LineChart
            {
                Entries = ListEntriesChart,
                LabelTextSize = 12,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true,
                AnimationDuration = new TimeSpan(9000)
            };

            BarChartSimple = new BarChart
            {
                Entries = ListEntriesChart,
                AnimationDuration = new TimeSpan(9000),
                IsAnimated = true,
                AnimationProgress = 6000,
                LabelTextSize = 12,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };

            PointChart = new PointChart
            {
                Entries = ListEntriesChart,
                AnimationDuration = new TimeSpan(6000),
                IsAnimated = true,
                LabelTextSize = 12,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };

            DonutChart = new DonutChart
            {
                Entries = ListEntriesChart,
                AnimationDuration = new TimeSpan(6000),
                IsAnimated = true,
                LabelTextSize = 12,
                HoleRadius = 0.5f
            };
        }


        #region Singleton
        private static UsuariosGraficas_VM instance;
        public static UsuariosGraficas_VM GetInstance(List<EN.Usuarios> usuarios)
        {
            if (instance == null)
            {
                return new UsuariosGraficas_VM(usuarios);
            }

            return instance;
        }
        #endregion

        #region Comandos

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
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(Load); }
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

        public async void Load()
        {
            //Revisa la conexión a internet            
            var connection = await this.apiService.CheckConnection();
            if (!connection.ExecutionOK)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", connection.Message, "Aceptar");
                return;
            }

            //Obtenemos el Usuario
            FiltroUsuario filtros = new FiltroUsuario
            {
                NombreUsuario = ""
            };

            //Realiza la consulta
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostObj<List<EN.Usuarios>>(url, "/api", "/GetUsuarios", filtros);
            if (!response.ExecutionOK)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                return;
            }

            this.Usuarios = response.Data;


            MainViewModel.GetInstance().UsuariosGraficas = new UsuariosGraficas_VM(this.Usuarios);
        }


        #endregion

    }



}
