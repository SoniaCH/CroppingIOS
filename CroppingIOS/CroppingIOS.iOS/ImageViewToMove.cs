using CoreGraphics;
using UIKit;

namespace CroppingIOS.iOS
{
    class ImageViewToMove : UIView
    {
        public ImageViewToMove()
        {
            Opaque = false;
            Alpha = 0.7f;
        }
        private UIImageView imageViewCrop;

        public UIImageView ImageViewCrop
        {
            get { return imageViewCrop; }
            set { imageViewCrop = value; SetNeedsDisplay(); }
        }
        private double imgX;

        public double ImgX
        {
            get { return imgX; }
            set { imgX = value; SetNeedsDisplay(); }
        }
        private double imgY;

        public double ImgY
        {
            get { return imgY; }
            set { imgY = value; SetNeedsDisplay(); }
        }
        private double imgW;

        public double ImgW
        {
            get { return imgW; }
            set { imgW = value; SetNeedsDisplay(); }
        }
        private double imgH;

        public double ImgH
        {
            get { return imgH; }
            set { imgH = value; SetNeedsDisplay(); }
        }


        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            try
            {
                foreach (var item in this.Subviews)
                {
                    item.RemoveFromSuperview();
                }
                this.Add(imageViewCrop);
            }
            catch (System.Exception ex)
            {
            }
        }

    }
}
