using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GridView : StackLayout
    {
        public new enum _Orientation
        {
            Horizontal,
            Vertical
        }
        
        private List<GridViewItem> _listItems;
        
        public List<GridViewItem> ListItems
        {
            get => _listItems;
            set
            {
                _listItems = value;
                this.AddChildren(value);
            }
        }

        public int? MaxRow { get; set; }
        public int? MaxColumn { get; set; }
        
        public GridView(int? maxRow = null, int? maxColumn = null)
        {

            this.MaxRow = maxColumn is null && maxRow is null ? null:maxRow;
            this.MaxColumn = maxColumn;
            
            InitializeComponent();


            if (maxRow != null) InitView(_Orientation.Horizontal, (int)maxRow);
            
            if (maxColumn != null) InitView(_Orientation.Vertical, (int)maxColumn);

        }

        private void AddChildren(List<GridViewItem> listItems)
        {
            for (int index = 0; index < listItems.Count(); index++)
            {
                int layoutn =  (index) % (this.Children.Count());
                StackLayout stackLayout = (StackLayout)(this.Children[layoutn]);
                stackLayout.Children.Add(listItems[index].GetView());
            }
        }

        private void InitView(_Orientation orientation, int n)
        {
            switch (orientation)
            {
                case _Orientation.Horizontal:
                    this.Orientation = StackOrientation.Vertical;
                    for (int _ = 0; _ < n; _++)
                    {
                        this.Children.Add(new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal
                        });
                    }
                    break;
                case _Orientation.Vertical:
                    for (int _ = 0; _ < n; _++)
                    {
                        Debug.WriteLine("Column Add");
                        this.Children.Add(new StackLayout
                        {
                            Orientation = StackOrientation.Vertical
                        });
                    }
                    break;
            }
        }
        
    }
}