using Xamarin.Forms;

namespace CroppingIOS
{
    public class ImageDoneCropping
    {
        public byte[] ImageCropped { get; set; }
        public ImageDoneCropping(byte[] imageCropped)
        {
            ImageCropped = imageCropped;
        
            var page = new Page1();
            page.BindingContext = this;
            Application.Current.MainPage = new NavigationPage(page);
        }
    }
}
