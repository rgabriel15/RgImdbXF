namespace RgImdbXF.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        #region Properties
        private string imageSource = null;
        public string ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }
        #endregion

        #region Functions
        public SplashViewModel()
        {
            ImageSource = "tmdb_312x276_primary_green.png";
        }
        #endregion
    }
}
