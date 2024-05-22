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
    public partial class BoiteView : ViewCell
    {
        
        public User[] usersList { get; set; }
        public BoiteView(User[] usersList)
        {
            this.usersList = usersList;
            InitializeComponent();

            if (usersList != null)
                foreach (User user in this.usersList)
                {
                    if (user != null)
                        UsersProfilList.Children.Add(user.GetLittleProfilView());
                }
        }
    }
}