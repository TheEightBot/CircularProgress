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

		private uint progress = 0;
	 

		protected ViewController(IntPtr handle) : base(handle)
		{
			 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.\

			 
			ConfigureFourColorCircularProgress();

			//NSTimer.scheduledTimerWithTimeInterval(0.03, target: self, selector: #selector(ViewController.updateProgress), userInfo: nil, repeats: true)
			NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.03), delegate
			{
				UpdateProgress(); 
			});
		}

		private void ConfigureFourColorCircularProgress()
		{
			var frame = new CGRect(0, 0, 200, 200);
		 
			fourColorCircularProgress = new iOS.CircularProgress(frame);
 
			fourColorCircularProgress.Colors = new List<CGColor> { 
				ColorExtension.RgbaColor(0xA6E39D11).CGColor,
			    ColorExtension.RgbaColor(0xAEC1E355).CGColor,
				ColorExtension.RgbaColor(0xAEC1E3AA).CGColor,
				ColorExtension.RgbaColor(0xF3C0ABFF).CGColor
			}.ToArray();

			View.AddSubview(fourColorCircularProgress);
		}

		private void UpdateProgress()
		{
			progress = progress + 1; 
			var normalizedProgress = (double)(progress / 255.0);  

			fourColorCircularProgress.Progress = normalizedProgress;

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

