using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateProfilPage : ContentPage
    {

        public string state = "name";
        
        public string name { get; set; }
        public string status { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        
        public CreateProfilPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private void Next(object sender, EventArgs e)
        {
            if (state == "name")
            {
                Title.Text = "What's your status ?";
                this.name = InputText.Text;
                InputText.Text = "";
                InputText.Placeholder = "Your Status";
                state = "status";
            }
            else if (state == "status")
            {
                Title.Text = "What's your phone number ?";
                this.status = InputText.Text;
                InputText.Text = "";
                InputText.Placeholder = "Your Phone Number";
                state = "phone";
            }
            else if (state == "phone")
            {
                Title.Text = "What's your mail ?";
                this.phone = InputText.Text;
                InputText.Text = "";
                InputText.Placeholder = "Your Mail";
                state = "mail";
            }
            else if (state == "mail")
            {
                Title.Text = "What's your address ?";
                this.mail = InputText.Text;
                InputText.Text = "";
                InputText.Placeholder = "Your Address";
                state = "address";
                BtnText.Text = "Confirm";
            }
            else if (state == "address")
            {
                this.address = InputText.Text;
                Data.currentUser = new User(name, status, phone, mail, address);
                Navigation.PushAsync(new MainPage());
            }
        }
    }
}