using System.Diagnostics;
using System.Linq;
using Ho.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Ho
{
    public class User
    {
        public Categorie[] _categoriesList { get; set; }
        public Categorie[] categoriesList
        {
            get
            {
                return this._categoriesList is null ? new Categorie[]{} : this._categoriesList;
            }
            set
            {
                this._categoriesList = value;
                DependencyService.Get<IFileService>().CreateFile(JsonConvert.SerializeObject(Data.data));
            }
        }

        public string name { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        
        public string img { get; set; }

        public User(string name, string title, string phone, string mail, string address)
        {
            this.name = name;
            this.title = title;
            this.phone = phone;
            this.mail = mail;
            this.address = address;
        }

        public View GetProfilView()
        {
            return new ProfilView(this).View;
        }
        
        public View GetLittleProfilView()
        {
            return new LittleProfilView(this).View;
        }
        
    }
}