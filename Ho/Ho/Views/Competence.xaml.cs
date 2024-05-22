using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GridView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ho.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompetenceView : ViewCell
    {
        public CompetenceView(Competence competence)
        {
            InitializeComponent();
            Name.Text = competence.name;
            Level.Text = competence.level;
            Image.Source = $"https://github.com/Tenlutshy/test/blob/main/{competence.img}.png?raw=true";
        }
    }
}