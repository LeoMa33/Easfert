using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ho.BLE;
using Ho.Interfaces;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace Ho
{
    public partial class MainPage : ContentPage
    {
        
        public Command LongPress { get; }
        public Command ChangePage { get; }
        public string CurrentPage { get; set; }
        
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            CurrentPage = "Profil";

            string data = DependencyService.Get<IFileService>().ReadFile();
            if (data != "")
            {
                JsonConvert.DeserializeObject<Data>(data);
                ContentPage.Content = Data.currentUser.GetProfilView();
                Categorie c;
                c = new Categorie("CAO")
                {
                    competencesList = new Competence[]
                    {
                        new Competence("Fusion 360", "Hight", "Fusion360Icon"),
                        new Competence("KiCad", "Medium", "KicadIcon")
                    }
                };
                /*c = new Categorie("Mobile Developpement")
                {
                    competencesList = new Competence[]
                    {
                        new Competence("Java", "Medium", "JavaIcon"),
                        new Competence("Xamarin", "Hight", "XamarinIcon"),
                        new Competence("Flutter", "Medium", "FlutterIcon")
                    }
                };
                Data.currentUser.img = "Leo.png";*/
                
                Categorie d;
                d = new Categorie("UI|UX")
                {
                    competencesList = new Competence[]
                    {
                        new Competence("Figma", "Medium", "FigmaIcon")
                    }
                };

                Data.currentUser.categoriesList = new Categorie[]
                {
                    c,d
                };
                Data.currentUser.img = "Manon.png";

            }
            else
            {
                Navigation.PushAsync(new CreateProfilPage());
            }
            
            MessagingCenter.Subscribe<LittleProfilView, User>(this,"", async (sender, args) =>
            {
                await Navigation.PushAsync(new ProfilDetailPage(args));
            });
            
            MessagingCenter.Subscribe<BLEClient, User>(this, "new user", async (sender, args) =>
            {
                ContentPage.Content = new BoiteView(Data.usersList).View;
                await Navigation.PushAsync(new ProfilDetailPage(args));
            });
            
            

            LongPress = new Command(async () =>
            {
                ActionPop.IsVisible = true;
                if (CurrentPage == "Profil")
                {
                    IBLEServer bleServer = DependencyService.Get<IBLEServer>();
                    bleServer.StartAdvert(JsonConvert.SerializeObject(Data.currentUser));
                    
                    while (bleServer.ConnectionState == BLEClient.ConnectionStates.Waiting)
                    {
                        var a1 = PopImage.ScaleTo(1.5, 500, Easing.Linear);
                    
                        await Task.WhenAll(a1);
                    
                        var a2 = PopImage.ScaleTo(1.0, 500, Easing.Linear);
                    
                        await Task.WhenAll(a2); 
                    }
                    
                    var a5 = PopImage.TranslateTo(0, PopImage.Y-1000, 1000, Easing.Linear);
                    await Task.WhenAll(a5);
                    

                    while (BLEClient.ConnectionState == BLEClient.ConnectionStates.Writing)
                    {
                        var sleep = Task.Delay(200);
                        await Task.WhenAll(sleep);
                    }
                    ActionPop.IsVisible = false;

                }
                else
                {
                    BLEClient.BLEConnection();
                    
                    var a0 = PopImage.TranslateTo(0, PopImage.Y-1000, 0, Easing.Linear);
                    var a1 = PopImage.RotateTo(180,0, Easing.Linear);
                                        
                    await Task.WhenAll(a0);

                    while (BLEClient.ConnectionState == BLEClient.ConnectionStates.Waiting)
                    {
                        var sleep = Task.Delay(200);
                        await Task.WhenAll(sleep);
                    }
                    
                    var sleep1 = Task.Delay(1000);
                    await Task.WhenAll(sleep1);
                    
                    if (BLEClient.ConnectionState == BLEClient.ConnectionStates.End) ActionPop.IsVisible = false;

                    var a5 = PopImage.TranslateTo(0, PopImage.Y-500, 1000, Easing.Linear);
                    await Task.WhenAll(a5);

                    while (BLEClient.ConnectionState == BLEClient.ConnectionStates.Reading)
                    {
                        var a6 = PopImage.ScaleTo(1.5, 1000, Easing.Linear);
                    
                        await Task.WhenAll(a6);
                    
                        var a7 = PopImage.ScaleTo(1.0, 1000, Easing.BounceIn);
                    
                        await Task.WhenAll(a7);
                    }
                    
                    ActionPop.IsVisible = false;
                }
            });

            ChangePage = new Command(() =>
            {
                CurrentPage = CurrentPage == "Profil" ? "Boite" : "Profil";

                if (CurrentPage == "Profil")
                {
                    ProfilBtn.BackgroundColor = Color.FromHex("#725AC1");
                    ProfilBtnImage.Source = "ProfilIcon.png";
                    BoiteBtn.BackgroundColor = Color.FromHex("#CAC4CE");
                    BoiteBtnImage.Source = "BoxIcon.png";
                    ContentPage.Content = Data.currentUser.GetProfilView();
                }
                else
                {
                    BoiteBtn.BackgroundColor = Color.FromHex("#725AC1");
                    BoiteBtnImage.Source = "BoxIcon2.png";
                    
                    ProfilBtn.BackgroundColor = Color.FromHex("#CAC4CE");
                    ProfilBtnImage.Source = "ProfilIcon2.png";
                    ContentPage.Content = new BoiteView(Data.usersList).View;
                }
            });
            
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Data.currentUser != null && CurrentPage == "Profil")
            {
                if (CurrentPage == "Profil")
                {
                    ProfilBtn.BackgroundColor = Color.FromHex("#725AC1");
                    ProfilBtnImage.Source = "ProfilIcon.png";
                    BoiteBtn.BackgroundColor = Color.FromHex("#CAC4CE");
                    BoiteBtnImage.Source = "BoxIcon.png";
                    ContentPage.Content = Data.currentUser.GetProfilView();
                }
                else
                {
                    BoiteBtn.BackgroundColor = Color.FromHex("#725AC1");
                    BoiteBtnImage.Source = "BoxIcon2.png";
                        
                    ProfilBtn.BackgroundColor = Color.FromHex("#CAC4CE");
                    ProfilBtnImage.Source = "ProfilIcon2.png";
                    ContentPage.Content = new BoiteView(Data.usersList).View;
                }
            }
        }
        

        private void StopAction(object sender, EventArgs e)
        {
            ActionPop.IsVisible = false;
            if (CurrentPage == "Profil")
            {
                Debug.WriteLine("Stop Advert");
            }
            else
            {
                Debug.WriteLine("Stop Read");
            }
        }
    }
}