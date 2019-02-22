using RgImdbXF.Models;
using RgImdbXF.Services;
using RgImdbXF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Sing = RgImdbXF.Singleton.Singleton;

namespace RgImdbXF.ViewModels
{
    public class UpcomingMovieViewModel : BaseViewModel
    {

        #region Variables
        private static UpcomingMovieService _upcomingMovieService = null;
        #endregion

        #region Properties
        public InfiniteScrollCollection<MovieModel> ListView_ItemsSource { get; set; }
        public Command ListView_RefreshCommand { get; set; }
        public Command ToolbarItemSearch_Command { get; set; }

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
        internal UpcomingMovieViewModel(UpcomingMovieService upcomingMovieService)
        {
            _upcomingMovieService = upcomingMovieService ?? throw new ArgumentException("upcomingMovieService");

            Title = "TMDb Upcoming Movies";
            ListView_ItemsSource = new InfiniteScrollCollection<MovieModel>
            {
                OnLoadMore = async() =>
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

            ListView_RefreshCommand = new Command(async() => await ExecuteListView_RefreshCommand_HandlerAsync());
            ToolbarItemSearch_Command = new Command(async() => await ToolbarItemSearch_Command_HandlerAsync());
            _ExecuteListView_RefreshCommand_HandlerAsync();
        }

        private async Task<IEnumerable<MovieModel>> GetMovieCollectionAsync(int page = 1)
        {
            var model = await _upcomingMovieService.GetAsync(page);
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

        private async void _ExecuteListView_RefreshCommand_HandlerAsync()
        {
            await ExecuteListView_RefreshCommand_HandlerAsync();
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

        private async Task ToolbarItemSearch_Command_HandlerAsync()
        {
            var viewModel = new SearchMovieViewModel(Sing.SearchMovieService);
            var page = new SearchMoviePage
            {
                BindingContext = viewModel
            };
            await ((NavigationPage)Application.Current.MainPage).PushAsync(page);
        }
        #endregion
    }
}