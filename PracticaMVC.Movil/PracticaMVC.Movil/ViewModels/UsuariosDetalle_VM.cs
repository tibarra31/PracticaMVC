using GalaSoft.MvvmLight.Command;
using Plugin.FilePicker;
using PracticaMVC.EN.Generics;
using PracticaMVC.Movil.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PracticaMVC.Movil.ViewModels
{
    public class UsuariosDetalle_VM : BaseViewModel
    {
        private ApiService apiService;

        private string imagenUsuario;
        public string ImagenUsuario
        {
            get { return this.imagenUsuario; }
            set { this.SetValue(ref this.imagenUsuario, value); }
        }

        private Documento imagen;
        public Documento Imagen
        {
            get { return this.imagen; }
            set { this.SetValue(ref this.imagen, value); }
        }

        public bool visibleImagen;
        public bool VisibleImagen
        {
            get { return this.visibleImagen; }
            set { this.SetValue(ref this.visibleImagen, value); }
        }
        private EN.Usuarios usuarios;
        public EN.Usuarios Usuarios
        {
            get { return this.usuarios; }
            set { this.SetValue(ref this.usuarios, value); }
        }
        private bool visibleID;
        public bool VisibleID
        {
            get { return this.visibleID; }
            set { this.SetValue(ref this.visibleID, value); }
        }

        private bool visibleField;
        public bool VisibleField
        {
            get { return this.visibleField; }
            set { this.SetValue(ref this.visibleField, value); }
        }

        private List<ControlDDL> listPerfiles;
        public List<ControlDDL> ListPerfiles
        {
            get { return this.listPerfiles; }
            set { this.SetValue(ref this.listPerfiles, value); }
        }
        private ControlDDL selectedPerfil;
        public ControlDDL SelectedPerfil
        {
            get { return this.selectedPerfil; }
            set { this.SetValue(ref this.selectedPerfil, value); }
        }

        public UsuariosDetalle_VM(EN.Usuarios user)
        {
            instance = this;
            this.apiService = new ApiService();
            this.Usuarios = user;
            this.Imagen = new Documento();
            if (!String.IsNullOrEmpty(user.ImagenUsuario))
            {
                var urlFolder = Application.Current.Resources["UrlFolderImages"].ToString();
                this.ImagenUsuario = urlFolder + user.ImagenUsuario;
            }
            ObtenerDatosInicio();
        }

        #region Singleton
        private static UsuariosDetalle_VM instance;

        public static UsuariosDetalle_VM GetInstance(EN.Usuarios usuario)
        {
            if (instance == null)
            {
                return new UsuariosDetalle_VM(usuario);
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
        public ICommand RegistrarCommand
        {
            get { return new RelayCommand(Registrar); }
        }
        public ICommand CargaImagenCommand
        {
            get { return new RelayCommand(CargaImagen); }
        }
        public ICommand QuitarImagenCommand
        {
            get { return new RelayCommand(QuitarImagen); }
        }
        private async void CargaImagen()
        {
            var file = await CrossFilePicker.Current.PickFile();
            if (file != null)
            {
                string fileExtension = System.IO.Path.GetExtension(file.FileName);
                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".gif" && fileExtension != ".jpeg" && fileExtension != ".bmp")
                {
                    this.VisibleImagen = false;
                    this.Imagen = null;

                    await Application.Current.MainPage.DisplayAlert("Error!", "El archivo no es una imagen válida", "Aceptar");
                }
                else
                {
                    string nombre = "ImagenUsuario_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + fileExtension;
                    this.Imagen = new Documento
                    {
                        Nombre = nombre,
                        DocArray = file.DataArray
                    };

                    this.VisibleImagen = true;
                }
            }
        }

        private void QuitarImagen()
        {
            this.Imagen = new Documento();
            this.VisibleImagen = false;
        }

        #endregion

        #region Metodos
        public async void Registrar()
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
                this.Usuarios.IdPerfil = SelectedPerfil.Valor;
                this.Usuarios.Archivo = this.Imagen;
                //Realiza el registro de solicitud
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.PostObj<int>(url, "/api", "/RegisterUser", this.Usuarios);
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
        private async void GetPerfilesDDL()
        {
            try
            {
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.GetData<List<ControlDDL>>(url, "/api", "/GetPerfilesDDL");
                if (!response.ExecutionOK)
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", response.Message, "Aceptar");
                    return;
                }

                if (response.Data.Count > 0)
                {
                    this.ListPerfiles = response.Data;

                    if (this.Usuarios.IdPerfil > 0)
                    {
                        var item = ListPerfiles.Where(x => x.Valor == this.Usuarios.IdPerfil).FirstOrDefault();
                        this.SelectedPerfil = item;
                    }
                    else
                        this.SelectedPerfil = this.ListPerfiles[0];
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "No se encontraron los perfiles de usuario", "Aceptar");
                    return;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje del sistema", ex.Message, "Aceptar");
                return;
            }
        }

        private void ObtenerDatosInicio()
        {
            this.VisibleField = true;
            this.VisibleID = this.Usuarios.IdUsuario > 0;

            GetPerfilesDDL();
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

        #endregion
    }
}
