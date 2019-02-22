using RgImdbXF.ViewModels;
using RgImdbXF.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sing = RgImdbXF.Singleton.Singleton;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RgImdbXF
{
    public partial class App : Application
    {
        #region Functions
        public App()
        {
            InitializeComponent();
            MainPage = new SplashPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await Sing.InitAsync();
            var upcomingMovieViewModel = new UpcomingMovieViewModel(Sing.UpcomingMovieService);
            var upcomingMoviePage = new UpcomingMoviePage
            {
                BindingContext = upcomingMovieViewModel
            };

            MainPage = new NavigationPage(upcomingMoviePage);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion
    }
}
