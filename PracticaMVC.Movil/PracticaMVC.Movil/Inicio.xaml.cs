using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PracticaMVC.Movil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        public Inicio()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            // true or false to disable or enable the action
            //return base.OnBackButtonPressed();

            //Llama metodo del View Model
            ViewModels.MainViewModel.GetInstance().Salir();

            //Cancela la accion por default del boton back
            return true;
        }
    }
}