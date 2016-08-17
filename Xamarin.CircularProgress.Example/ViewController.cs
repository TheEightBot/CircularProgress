using System;
using Xamarin.CircularProgress.iOS;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Xamarin.CircularProgress.Example
{
	public partial class ViewController : UIViewController
	{
		private iOS.CircularProgress fourColorCircularProgress;
		private uint progress = 0;
		UIView _view;

		protected ViewController(IntPtr handle) : base(handle)
		{
			 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.\

			_view = new UIView
			{
	 
				Frame = new CGRect(0,0,100,100)
			};

			View.AddSubview(_view);
			ConfigureFourColorCircularProgress();

		}

		private void ConfigureFourColorCircularProgress()
		{
			var frame = new CGRect(0, 0, _view.Frame.Width, _view.Frame.Height);
			Debug.WriteLine("frame created");
			fourColorCircularProgress = new iOS.CircularProgress(frame, true);
			Debug.WriteLine("four color circular progress created");
			fourColorCircularProgress.Colors = new List<CGColor> { 
				ColorExtension.RgbaColor(0xA6E39D11).CGColor,
			    ColorExtension.RgbaColor(0xAEC1E355).CGColor,
				ColorExtension.RgbaColor(0xAEC1E3AA).CGColor,
				ColorExtension.RgbaColor(0xF3C0ABFF).CGColor
			}.ToArray();

			_view.AddSubview(fourColorCircularProgress);
		}

		private void updateProgress()
		{
			progress = progress + 1;

			var normalizedProgress = (double)(progress / 255.0);  

			fourColorCircularProgress.Progress = normalizedProgress; 
		} 

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

