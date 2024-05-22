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
    public partial class LittleProfilView : ViewCell
    {
        public User user { get; set; }
        
        public LittleProfilView(User user)
        {
            this.user = user;
            InitializeComponent();
            Name.Text = user.name;
            Title.Text = user.title;
                
            string picName = user.img is null ? "DefaultPic.jpg" : user.img;
            ProfilImage.Source = $"https://github.com/Tenlutshy/test/blob/main/{picName}?raw=true";
        }

        private void GoProfil(object sender, EventArgs e)
        {
            MessagingCenter.Send<LittleProfilView, User>(this,"",this.user);
        }
    }
}