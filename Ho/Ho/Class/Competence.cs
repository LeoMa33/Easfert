using GridView;
using Ho.Views;

namespace Ho
{
    public class Competence:GridViewItem
    {
        public string name { get; set; }
        public string level { get; set; }
        
        public string img { get; set; }

        public Competence(string name, string level, string img)
        {
            this.name = name;
            this.level = level;
            this.img = img;
            this.SetView(new CompetenceView(this).View);
        }
    }
}