using System;
using CoreAnimation;
using UIKit;
using Foundation;
using AVFoundation;
using ObjCRuntime;
using System.Drawing;
using CoreGraphics;
using CoreFoundation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xamarin.CircularProgress.iOS
{
	public class CircularProgress : UIView
	{
		/**
			Main progress view.
		*/
		CircularShapeView progressView;

		/**
 			Gradient mask layer of `progressView`.
 		*/
		private CAGradientLayer gradientLayer;

		/**
    		Guide view of `progressView`.
    	*/
		private CircularShapeView progressGuideView;

		/**
			Mask layer of `progressGuideView`.
		*/
		private CALayer guideLayer;

		/**
    		Current progress value. (0.0 - 1.0)
    	*/
		private double _progress;
		 
		public double Progress { 
			get { return _progress; }
			set {
				_progress = value;

				var clipProgress = Math.Max(Math.Min(_progress, 1.0), 0.0);
				progressView.UpdateProgress(clipProgress);

				//TODO: progressChangedClosure
			}
		}
		/**
    		Progress start angle.
    	*/
		private double _startAngle = 0.0;
		public double StartAngle
		{
			get { return _startAngle; }
			set {
				_startAngle = value;
				progressView.StartAngle = _startAngle;
				progressGuideView.StartAngle = _startAngle;
			}
		}
		/**
    		Progress end angle.
    	*/
		private double _endAngle = 0.0;
		public double EndAngle
		{
			get { return _endAngle; }
			set
			{
				_endAngle = value;
				progressView.EndAngle = _endAngle;
				progressGuideView.EndAngle = _endAngle;
			}
		}
		/**
			Main progress line width.
		*/
		private double _lineWidth = 8.0;
		public double LineWidth
		{
			get { return _lineWidth; }
			set
			{
				_lineWidth = value;
				progressView.ShapeLayer.LineWidth = (nfloat)_lineWidth;
			}
		}

		/**
			Guide progress line width.
		*/
		private double _guideLineWidth = 8.0;
		public double GuideLineWidth
		{
			get { return _guideLineWidth; }
			set
			{
				_guideLineWidth = value;
				progressGuideView.ShapeLayer.LineWidth = (nfloat)_guideLineWidth;
			}
		}

		/**
			Progress bar path. You can create various type of progress bar.
		*/
		private UIBezierPath _path;
		public UIBezierPath Path 
		{
			get { return _path; }
			set 
			{
				_path = value;

				progressView.ShapeLayer.Path = _path.CGPath;
				progressGuideView.ShapeLayer.Path = _path.CGPath;

			}
		}
		/**
			Progress bar colors. You can set many colors in `colors` property, and it makes gradation color in `colors`.
		*/
		private CGColor[] _colors;
		public CGColor[] Colors 
		{
			get { return _colors; }
			set
			{
				_colors = value;
				//TODO: UpdateColors(_colors);
			}
		}

		/**	
		  	Progress guide bar color.
    	*/
		private UIColor _progressGuideColor = new UIColor((nfloat)0.1, (nfloat)0.1, (nfloat)0.1, (nfloat)0.2);
		public UIColor ProgressGuideColor
		{
			get { return _progressGuideColor; }
			set
			{
				_progressGuideColor = value;
				guideLayer.BackgroundColor = ProgressGuideColor.CGColor;
			}
		}

		private bool _showProgressGuide = false;
		public bool ShowProgressGuide 
		{
			get { return _showProgressGuide; }
			set
			{
				_showProgressGuide = value;
				SetNeedsLayout();
				LayoutIfNeeded();
				ConfigureProgressGuideLayer(_showProgressGuide);
			}
		}

		/**
			Create `KYCircularProgress` with progress guide.

			- parameter frame: `KYCircularProgress` frame.
			- parameter showProgressGuide: If you set to `true`, progress guide view is enabled.
		*/

		public CircularProgress(CGRect frame, bool showProgressGuide) :base(frame)
		{ 
			ConfigureProgressLayer();
			ConfigureProgressGuideLayer(showProgressGuide); 
		}

		private void ConfigureProgressLayer() 
		{
			progressView = new CircularShapeView((RectangleF)Bounds);
			progressView.ShapeLayer.FillColor = UIColor.Clear.CGColor;
			progressView.ShapeLayer.Path = Path.CGPath;
			progressView.ShapeLayer.LineWidth = (nfloat)LineWidth;
			progressView.ShapeLayer.StrokeColor = TintColor.CGColor;
			//TODO: how do I pass 'layer' in?
			gradientLayer = new CAGradientLayer();
			gradientLayer.Frame = progressView.Frame;
			gradientLayer.StartPoint = new CGPoint(0, 0.5);
			gradientLayer.EndPoint = new CGPoint(1, 0.5);
			gradientLayer.Mask = progressView.ShapeLayer;

			List<CGColor> defaultColors = new List<CGColor> {
					RgbaColor(rgba: 0x9ACDE7FF).CGColor,
					RgbaColor(rgba: 0xE7A5C9FF).CGColor,

				};

			gradientLayer.Colors = Colors ?? defaultColors.ToArray();

			Layer.AddSublayer(gradientLayer);

		}

		private void ConfigureProgressGuideLayer(bool showProgressGuide)
		{
			if (showProgressGuide && progressGuideView == null) 
			{
				progressGuideView = new CircularShapeView((RectangleF)Bounds);
				progressGuideView.ShapeLayer.FillColor = UIColor.Clear.CGColor;
				progressGuideView.ShapeLayer.Path = progressView.ShapeLayer.Path;
				progressGuideView.ShapeLayer.LineWidth = (System.nfloat)GuideLineWidth;
				progressGuideView.ShapeLayer.StrokeColor = TintColor.CGColor;

				guideLayer = new CAGradientLayer();
				guideLayer.Frame = progressGuideView.Frame;
				guideLayer.Mask = progressGuideView.ShapeLayer;
				guideLayer.BackgroundColor = ProgressGuideColor.CGColor;
				guideLayer.ZPosition = -1;

				progressGuideView.UpdateProgress(1.0);

				Layer.AddSublayer(guideLayer);
			}
		}

		private void UpdateColors(CGColor[] colors)
		{
			List<CGColor> convertedColors = new List<CGColor>();
			if (colors != null)
			{
				foreach (var color in colors)
				{
					convertedColors.Add(color);
				}
				if (convertedColors.Count == 1)
				{
					convertedColors.Add(convertedColors[0]);
				}
			}
			else 
			{
				List<CGColor> defaultColors = new List<CGColor> { 
					RgbaColor(rgba: 0x9ACDE7FF).CGColor,
				    RgbaColor(rgba: 0xE7A5C9FF).CGColor,
					                           
				};

				convertedColors = defaultColors;
			};

			gradientLayer.Colors = convertedColors.ToArray();
		}


		public UIColor RgbaColor(Int64 rgba)
		{
			var red = (nfloat)((rgba & 0xFF000000) >> 24) / 255.0;
			var green = (nfloat)((rgba & 0x00FF0000) >> 16) / 255.0;
			var blue = (nfloat)((rgba & 0x0000FF00) >> 8) / 255.0;
			var alpha = (nfloat)(rgba & 0x000000FF) / 255.0;

			return new UIColor((nfloat)red, (nfloat)green, (nfloat)blue, (nfloat)alpha);

		}

	}
	 
	public class CircularShapeView : UIView
	{
		private double _startAngle;
		public double StartAngle { 
			get { return _startAngle; }
			set { _startAngle = value; }
		}

		private double _endAngle;
		public double EndAngle
		{
			get { return _endAngle; }
			set { _endAngle = value; }
		}

		static Class layerClass;

		[Export("initWithCoder:")]
		public CircularShapeView (NSCoder coder) : base(coder)
		{

		}

		public CircularShapeView(RectangleF frame) : base (frame)
		{
			UpdateProgress(0);
		}

		public static Class LayerClass
		{
			[Export("layerClass")]
			get
			{
				return layerClass = layerClass ?? new Class(typeof(CAShapeLayer));
			}
		}
 
		public CAShapeLayer ShapeLayer
		{
			get
			{
				return (CAShapeLayer)Layer;
			}
		}
 		
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			// TODO: investigate warning  
			if (StartAngle == EndAngle)
				EndAngle = StartAngle + (Math.PI * 2);

			ShapeLayer.Path = ShapeLayer.Path ?? LayoutPath().CGPath;
		}

		private UIBezierPath LayoutPath() 
		{
			nfloat halfWidth = Frame.Width / 2;
			CGPoint pointMake = new CGPoint(halfWidth, halfWidth);
			UIBezierPath bezier = new UIBezierPath();
			bezier.AddArc(pointMake, halfWidth - ShapeLayer.LineWidth, (nfloat)StartAngle, (nfloat)EndAngle, true);

			return bezier; 
		}

		public void UpdateProgress(double progress)
		{ 
			CATransaction.Begin();
			CATransaction.SetValueForKey(new NSNumber(true), CATransaction.DisableActionsKey);
			ShapeLayer.StrokeEnd = (nfloat)progress;
			CATransaction.Commit();
		}
	}

 
}

