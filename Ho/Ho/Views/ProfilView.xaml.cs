using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ho.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilView : ViewCell
    {
        public User user { get; set; }
        public ProfilView(User user)
        {
            InitializeComponent();
            this.user = user;

            Name.Text = user.name;
            Title.Text = user.title;
            Phone.Text = user.phone;
            Mail.Text = user.mail;
            Address.Text = user.address;
            string picName = user.img is null ? "DefaultPic.jpg" : user.img;
            ProfilImage.Source = $"https://github.com/Tenlutshy/test/blob/main/{picName}?raw=true";

            foreach (var categorie in user.categoriesList)
            {
                CaterogiesList.Children.Add(new CategorieView(categorie).View);
            }
            
        }
    }
}