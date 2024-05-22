using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilDetailPage : ContentPage
    {
        public ProfilDetailPage(User user)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            ContentPage.Content = user.GetProfilView();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}