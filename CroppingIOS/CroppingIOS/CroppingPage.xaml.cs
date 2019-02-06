using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CroppingIOS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CroppingPage : ContentPage
	{
		public CroppingPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
	}
}