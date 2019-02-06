using CoreGraphics;
using CroppingIOS;
using CroppingIOS.iOS;
using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(CroppingPage), typeof(CroppingPageRenderer))]
namespace CroppingIOS.iOS
{
    class CroppingPageRenderer : PageRenderer
    {
        CropperView cropperView;
        ImageViewToMove imageViewToMove;
        UIPanGestureRecognizer pan1;
        UIPinchGestureRecognizer pinch1;
        UITapGestureRecognizer doubleTap;
        UITapGestureRecognizer retake;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var page = e.NewElement as CroppingPage;
            var view = NativeView;
        }
        UIImage resultImage;
        UIImage startImage;
        nfloat ratio;
        double height;
        double width;
        double center;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            using (var data = NSData.FromFile("picture.jpg"))
            {
                startImage = UIImage.LoadFromData(data);
                ratio = startImage.Size.Width / startImage.Size.Height;
                height = (App.ScreenWidth) / ratio;
                center = (App.ScreenHeight - height) / 2;
                width = App.ScreenWidth;
                UIGraphics.BeginImageContextWithOptions(new CGSize(App.ScreenWidth, App.ScreenHeight), true, 1.0f);
                startImage.Draw(new CGRect(0, center, width, height));
                resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();


                var testh2 = resultImage.Size.Height;
                var testw2 = resultImage.Size.Width;
                cropperView = new CropperView();
                var test = new UIImageView(new CGRect(0, 0, App.ScreenWidth, App.ScreenHeight));
                test.Image = resultImage;
                imageViewToMove = new ImageViewToMove();
                imageViewToMove.Frame = test.Frame;
                cropperView.Frame = View.Frame;
                var h = (App.ScreenWidth * 0.9);
                if (h > height)
                    h = height - 10;
                cropperView.CropSize = new CGSize(h, h);
                cropperView.Origin = new CGPoint((App.ScreenWidth - h) / 2, center + ((height - h) / 2));

                imageViewToMove.ImgX = 0;
                imageViewToMove.ImgY = 0;
                imageViewToMove.ImgW = App.ScreenWidth;
                imageViewToMove.ImgH = App.ScreenHeight;
                imageViewToMove.ImageViewCrop = test;
            }
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            var bottomButtonY = View.Bounds.Bottom - 70;
            var btn = new UIButton()
            {
                Frame = new CGRect(0, bottomButtonY, App.ScreenWidth / 2, 70)
            };
            btn.BackgroundColor = UIColor.FromRGB(219, 179, 74);
            btn.SetTitle("Done", UIControlState.Normal);
            btn.SetTitleColor(UIColor.White, UIControlState.Normal);

            View.AddSubviews(imageViewToMove, cropperView, btn);
            View.BackgroundColor = UIColor.Black;
            nfloat dx = 0;
            nfloat dy = 0;
            pan1 = new UIPanGestureRecognizer(() =>
            {
                if ((pan1.State == UIGestureRecognizerState.Began ||
                pan1.State == UIGestureRecognizerState.Changed) && (pan1.NumberOfTouches == 1))
                {
                    var p0 = pan1.LocationInView(View);

                    if (dx == 0)
                        dx = p0.X - (nfloat)imageViewToMove.ImgX;

                    if (dy == 0)
                        dy = p0.Y - (nfloat)imageViewToMove.ImgY;
                    imageViewToMove.ImgX = p0.X - dx;
                    imageViewToMove.ImgY = p0.Y - dy;
                    var test = new UIImageView(new CGRect(imageViewToMove.ImgX, imageViewToMove.ImgY, imageViewToMove.ImgW, imageViewToMove.ImgH));
                    test.Image = resultImage;
                    imageViewToMove.ImageViewCrop = test;
                }
                else if (pan1.State == UIGestureRecognizerState.Ended)
                {
                    dx = 0;
                    dy = 0;
                }
            });

            nfloat s0 = 1;
            pinch1 = new UIPinchGestureRecognizer(() =>
            {
                nfloat s = pinch1.Scale;
                nfloat ds = (nfloat)Math.Abs(s - s0);
                nfloat sf = 0;
                const float rate = 0.5f;
                if (s >= s0)
                {
                    sf = 1 + ds * rate;
                }
                else if (s < s0)
                {
                    sf = 1 - ds * rate;
                }
                s0 = s;
                imageViewToMove.ImgW = imageViewToMove.ImgW * sf;
                imageViewToMove.ImgH = imageViewToMove.ImgH * sf;
                imageViewToMove.ImgX = imageViewToMove.ImgX * sf;
                imageViewToMove.ImgY = imageViewToMove.ImgY * sf;
                height = height * sf;
                center = center * sf;
                var test = new UIImageView(new CGRect(imageViewToMove.ImgX, imageViewToMove.ImgY, imageViewToMove.ImgW, imageViewToMove.ImgH));
                test.Image = resultImage;
                imageViewToMove.ImageViewCrop = test;
                if (pinch1.State == UIGestureRecognizerState.Ended)
                {
                    s0 = 1;
                }
            });

            doubleTap = new UITapGestureRecognizer((gesture) => Crop())
            {
            };


            btn.AddGestureRecognizer(doubleTap);
            cropperView.AddGestureRecognizer(pan1);
            cropperView.AddGestureRecognizer(pinch1);
        }
        void Crop()
        {
            UIGraphics.BeginImageContextWithOptions(new CGSize(App.ScreenWidth, App.ScreenHeight), true, 1.0f);
            startImage.Draw(new CGRect(imageViewToMove.ImgX, imageViewToMove.ImgY + center, imageViewToMove.ImgW, height));
            resultImage = UIGraphics.GetImageFromCurrentImageContext();
            var inputCGImage = resultImage.CGImage;
            var image = inputCGImage.WithImageInRect(cropperView.CropRect);
            using (var croppedImage = UIImage.FromImage(image))
            {
                //scretching the size of the cropped View
                if (croppedImage.Size.Height < 968)
                {
                    var testh = croppedImage.Size.Height;
                    var testw = croppedImage.Size.Width;
                    UIGraphics.BeginImageContextWithOptions(new CGSize(968, 968), true, 1.0f);
                    croppedImage.Draw(new CGRect(0, 0, 968, 968));
                    resultImage = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    var testh1 = resultImage.Size.Height;
                    var testw1 = resultImage.Size.Width;

                    using (NSData imageData = resultImage.AsPNG())
                    {
                        Byte[] myByteArray = new Byte[imageData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                        new ImageDoneCropping(myByteArray);

                    }
                } //EndScretching
                else
                {
                    using (NSData imageData = croppedImage.AsPNG())
                    {
                        Byte[] myByteArray = new Byte[imageData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                        new ImageDoneCropping(myByteArray);
                    }
                }

            }
        }
    }
}