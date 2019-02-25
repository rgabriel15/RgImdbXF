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
        private uint totalPages = 0;
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
                        var page = (uint)(ListView_ItemsSource.Count / PageSize) + 1;

                        if (totalPages > 0
                            && page >= totalPages)
                        {
                            IsBusy = false;
                            return null;
                        }

                        var model = await GetMovieCollectionAsync(page);
                        totalPages = (uint)model.TotalPages;
                        IsBusy = false;
                        return model.MovieCollection;
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

        private async Task<SearchMovieModel> GetMovieCollectionAsync(uint page = 1)
        {
            if (string.IsNullOrWhiteSpace(SearchBar_Text))
                return null;
            var model = await _searchMovieService.GetAsync(SearchBar_Text, page);
            return model;
        }

        private async Task ExecuteListView_RefreshCommand_HandlerAsync()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                ListView_ItemsSource.Clear();
                var model = await GetMovieCollectionAsync();
                ListView_ItemsSource.AddRange(model.MovieCollection);

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
