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
    public partial class AdminUsuarios : ContentPage
    {
        public AdminUsuarios()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void lstEjemplo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstEjemplo.SelectedItem = null;
        }
    }
}