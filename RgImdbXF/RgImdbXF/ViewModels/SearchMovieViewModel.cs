using RgImdbXF.Models;
using RgImdbXF.Services;
using RgImdbXF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace RgImdbXF.ViewModels
{
    public class SearchMovieViewModel : BaseViewModel
    {
        #region Variables
        private static SearchMovieService _searchMovieService = null;
        #endregion

        #region Properties
        private string searchBar_Text = null;
        public string SearchBar_Text
        {
            get { return searchBar_Text; }
            set { SetProperty(ref searchBar_Text, value); }
        }

        public InfiniteScrollCollection<MovieModel> ListView_ItemsSource { get; set; }
        public Command SearchBar_SearchCommand { get; set; }
        public Command ListView_RefreshCommand { get; set; }

        private MovieModel listView_SelectedItem = null;
        public MovieModel ListView_SelectedItem
        {
            get { return listView_SelectedItem; }
            set
            {
                SetProperty(ref listView_SelectedItem, value);
                ListView_SelectedItem_HandlerAsync();
            }
        }
        #endregion

        #region Functions        
        internal SearchMovieViewModel(SearchMovieService searchMovieService)
        {
            _searchMovieService = searchMovieService ?? throw new ArgumentException("searchMovieService");

            Title = "Search Movie";
            ListView_ItemsSource = new InfiniteScrollCollection<MovieModel>
            {
                OnLoadMore = async () =>
                {
                    try
                    {
                        if (IsBusy)
                            return null;

                        IsBusy = true;
                        var page = ListView_ItemsSource.Count / PageSize;
                        var collection = await GetMovieCollectionAsync(page + 1);
                        IsBusy = false;
                        return collection;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        Debug.WriteLine(ex);
                        throw;
                    }
                }
            };

            SearchBar_SearchCommand = new Command(async() => await ExecuteListView_RefreshCommand_HandlerAsync());
            ListView_RefreshCommand = new Command(async () => await ExecuteListView_RefreshCommand_HandlerAsync());
        }

        private async Task<IEnumerable<MovieModel>> GetMovieCollectionAsync(int page = 1)
        {
            if (string.IsNullOrWhiteSpace(SearchBar_Text))
                return null;
            var model = await _searchMovieService.GetAsync(SearchBar_Text, page);
            return model?.MovieCollection;
        }

        private async Task ExecuteListView_RefreshCommand_HandlerAsync()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                ListView_ItemsSource.Clear();
                var collection = await GetMovieCollectionAsync();
                ListView_ItemsSource.AddRange(collection);

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex);
                throw;
            }
        }

        private async void ListView_SelectedItem_HandlerAsync()
        {
            try
            {
                if (IsBusy
                    || ListView_SelectedItem == null)
                    return;

                IsBusy = true;

                var viewModel = new MovieDetailViewModel(ListView_SelectedItem);
                var page = new MovieDetailPage()
                {
                    BindingContext = viewModel
                };

                await ((NavigationPage)Application.Current.MainPage).Navigation.PushAsync(page);
                ListView_SelectedItem = null;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex);
                throw;
            }
        }
        #endregion
    }
}
