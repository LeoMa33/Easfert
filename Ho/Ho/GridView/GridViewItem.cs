using Xamarin.Forms;

namespace GridView
{
    public class GridViewItem
    {
        private View _view = new StackLayout();
        
        public View GetView()
        {
            return _view;
        }

        public void SetView(View view)
        {
            this._view = view;
        }
    }
}