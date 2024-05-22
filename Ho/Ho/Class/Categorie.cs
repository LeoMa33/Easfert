using System.Linq;
using Ho.Views;
using Xamarin.Forms;

namespace Ho
{
    public class Categorie
    {
        public string name { get; set; }
        private Competence[] _competencesList { get; set; }

        public Competence[] competencesList
        {
            get { return _competencesList ?? new Competence[] { }; }
            set => _competencesList = value;
        }

        public Categorie(string name)
        {
            this.name = name;
        }

        public void AddCompetence(Competence competence)
        {
            this.competencesList = (Competence[])this.competencesList.Append(competence);
        }

        public View GetCategorieView()
        {
            return new CategorieView(this).View;
        }
    }
}