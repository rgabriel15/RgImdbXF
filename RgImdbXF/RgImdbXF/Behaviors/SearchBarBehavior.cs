using Xamarin.Forms;

namespace RgImdbXF.Behaviors
{
    public class SearchBarBehavior : Behavior<SearchBar>
    {
        #region Constants
        private const byte MaxLenth = 50;
        #endregion

        #region Functions
        protected override void OnAttachedTo(SearchBar searchBar)
        {
            searchBar.TextChanged += OnSearchBarTextChanged;
            base.OnAttachedTo(searchBar);
        }

        protected override void OnDetachingFrom(SearchBar searchBar)
        {
            searchBar.TextChanged -= OnSearchBarTextChanged;
            base.OnDetachingFrom(searchBar);
        }

        void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            if (args.NewTextValue.Length > MaxLenth)
            {
                try
                {
                    ((SearchBar)sender).TextChanged -= OnSearchBarTextChanged;
                    ((SearchBar)sender).Text = args.NewTextValue.Substring(0, MaxLenth);
                }
                finally
                {
                    ((SearchBar)sender).TextChanged += OnSearchBarTextChanged;
                }
            }
        }
        #endregion
    }
}
