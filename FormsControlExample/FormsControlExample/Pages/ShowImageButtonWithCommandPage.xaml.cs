using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsControlExample
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShowImageButtonWithCommandPage : ContentPage
	{
		public ShowImageButtonWithCommandPage ()
		{
            BindingContext = new ExampleViewModel();

            InitializeComponent ();
		}
	}
}