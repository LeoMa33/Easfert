using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GridView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ho.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategorieView : ViewCell
    {
        public CategorieView(Categorie categorie)
        {
            InitializeComponent();

            Name.Text = categorie.name;
            
            GridView.GridView gridView = new GridView.GridView(maxRow:(int)Math.Ceiling(categorie.competencesList.Length/2.0));
            

            gridView.ListItems = new List<GridViewItem>(categorie.competencesList);
            
            GridView.Children.Add(gridView);
            
        }
    }
}

