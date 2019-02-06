using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CroppingIOS.iOS
{
    class CropperView : UIView
    {
        CGPoint origin;
        CGSize cropSize;

        public CropperView()
        {
            origin = new CGPoint((App.ScreenWidth - (App.ScreenWidth * 0.8)) / 2, (App.ScreenHeight - 250) / 2);
            cropSize = new CGSize((App.ScreenWidth * 0.8), (App.ScreenWidth * 0.8));
            BackgroundColor = UIColor.Clear;
            Opaque = false;
            Alpha = 0.7f;
        }

        public CGPoint Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                SetNeedsDisplay();
            }
        }

        public CGSize CropSize
        {
            get
            {
                return cropSize;
            }
            set
            {
                cropSize = value;
                SetNeedsDisplay();
            }
        }

        public CGRect CropRect
        {
            get
            {
                return new CGRect(Origin, CropSize);
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            try
            {
                using (var g = UIGraphics.GetCurrentContext())
                {
                    g.SetLineWidth(1);
                    g.SetFillColor(UIColor.Black.CGColor);
                    g.FillRect(rect);
                    g.SetBlendMode(CGBlendMode.Clear);
                    g.AddRect(new CGRect(origin, cropSize));
                    g.DrawPath(CGPathDrawingMode.FillStroke);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

    }


}