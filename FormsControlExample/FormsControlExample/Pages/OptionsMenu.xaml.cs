using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsControlExample
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OptionsMenu : ContentPage
	{
		public OptionsMenu ()
		{
			InitializeComponent ();
		}

        void SelectMenuItem(object sender, ItemTappedEventArgs e)
        {
            switch (e.Item.ToString())
            {
                case "Image Button":
                    Navigation.PushAsync(new ShowImageButtonWithCommandPage());
                    break;
                case "Gauge":
                    Navigation.PushAsync(new DisplayGaugePage());
                    break;
            }
        }
    }
}