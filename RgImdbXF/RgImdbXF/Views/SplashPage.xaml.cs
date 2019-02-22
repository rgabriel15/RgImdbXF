using RgImdbXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RgImdbXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashPage : ContentPage
	{
		public SplashPage ()
		{
            BindingContext = new SplashViewModel();
			InitializeComponent();
		}
	}
}