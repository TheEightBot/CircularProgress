using System;
using Xamarin.CircularProgress.iOS;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;

namespace Xamarin.CircularProgress.Example
{
	public partial class ViewController : UIViewController
	{
		private iOS.CircularProgress fourColorCircularProgress;

		private iOS.CircularProgress halfCircularProgress;

		private iOS.CircularProgress starProgress;

		private uint progress = 0;
	 

		protected ViewController(IntPtr handle) : base(handle)
		{
			 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.\


			ConfigureFourColorCircularProgress();
			ConfigureHalfCircularProgress();
			ConfigureStarCircularProgress();

			//NSTimer.scheduledTimerWithTimeInterval(0.03, target: self, selector: #selector(ViewController.updateProgress), userInfo: nil, repeats: true)
			NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.03), delegate
			{
				UpdateProgress(); 
			});
		}

		private void ConfigureStarCircularProgress()
		{ 
			var frame = new CGRect(201, 0, 200, 200);
			starProgress = new iOS.CircularProgress(frame);
			starProgress.Colors = new[] 
			{
				UIColor.Purple.CGColor, 
				ColorExtension.RgbaColor(0xFFF77A55).CGColor,
		        UIColor.Orange.CGColor
			};

			starProgress.LineWidth = 3.0;

			var path = new UIBezierPath();
			path.MoveTo(new CGPoint(50.0, 2.0));
			path.AddLineTo(new CGPoint(84.0, 86.0));
			path.AddLineTo(new CGPoint(6.0, 33.0));
			path.AddLineTo(new CGPoint(96.0, 33.0));
			path.AddLineTo(new CGPoint(17.0, 86.0));
			path.ClosePath();
			starProgress.Path = path;


			View.AddSubview(starProgress);
		}

		private void ConfigureHalfCircularProgress() 
		{
			var frame = new CGRect(0, 201, View.Frame.Width, View.Frame.Height / 2);
			halfCircularProgress = new iOS.CircularProgress(frame, true);

			var center = new CGPoint(160, 200);
			var bezier = new UIBezierPath();
			bezier.AddArc(center, halfCircularProgress.Frame.Width / 3, (nfloat)Math.PI, (nfloat)0.0, true);
			halfCircularProgress.Path = bezier;

			halfCircularProgress.Colors = new [] 
			{
				ColorExtension.RgbaColor(0xA6E39DAA).CGColor,
				ColorExtension.RgbaColor(0xAEC1E3AA).CGColor,
				ColorExtension.RgbaColor(0xAEC1E3AA).CGColor,
				ColorExtension.RgbaColor(0xF3C0ABAA).CGColor
			};

			halfCircularProgress.LineWidth = 8.0;
			halfCircularProgress.ProgressGuideColor = new UIColor((nfloat)0.1,(nfloat)0.1,(nfloat)0.1,(nfloat)0.4);

			var textLabel = new UILabel(new CGRect(halfCircularProgress.Frame.X + 120.0, 170.0, 80, 32));
			textLabel.TextAlignment = UITextAlignment.Center;
			textLabel.TextColor = UIColor.Green;
			textLabel.Font = UIFont.FromName("HelveticaNeue-UltraLight", 20f);
			textLabel.Alpha = (System.nfloat)0.5;
			textLabel.Text = "";

			halfCircularProgress.AddSubview(textLabel);

			halfCircularProgress.ProgressChangedClosure((double val, iOS.CircularProgress arg2) =>
			{
 
				textLabel.Text = string.Format("{0}%", val);
			});


			View.AddSubview(halfCircularProgress);
		}

		private void ConfigureFourColorCircularProgress()
		{
			var frame = new CGRect(0, 0, 200, 200);

			fourColorCircularProgress = new iOS.CircularProgress(frame);

			fourColorCircularProgress.Colors = new[] 
			{ 
				ColorExtension.RgbaColor(0xA6E39D11).CGColor,
				ColorExtension.RgbaColor(0xAEC1E355).CGColor,
				ColorExtension.RgbaColor(0xAEC1E3AA).CGColor,
				ColorExtension.RgbaColor(0xF3C0ABFF).CGColor
			};
 
			View.AddSubview(fourColorCircularProgress);
		}

		private void UpdateProgress()
		{
			progress = progress + 1; 
			var normalizedProgress = (double)(progress / 255.0);  

			fourColorCircularProgress.Progress = normalizedProgress;
			halfCircularProgress.Progress = normalizedProgress;
			starProgress.Progress = normalizedProgress;

			if (normalizedProgress >= 1)
				progress = 0;
		} 

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

