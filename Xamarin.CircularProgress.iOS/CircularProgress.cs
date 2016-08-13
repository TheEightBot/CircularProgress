using System;
using CoreAnimation;
using UIKit;
using Foundation;
using AVFoundation;
using ObjCRuntime;

namespace Xamarin.CircularProgress.iOS
{
	public class CircularProgress : UIView
	{

		private CAGradientLayer gradientLayer;


		public CircularProgress()
		{
		}
	}

	public class CircularShapeView : UIView
	{
		double _startAngle;

		double _endAngle;

		static Class layerClass;

		[Export("initWithCoder:")]
		public CircularShapeView (NSCoder coder) : base(coder)
		{

		}

		public static Class LayerClass
		{
			[Export("layerClass")]
			get
			{
				return layerClass = layerClass ?? new Class(typeof(CAShapeLayer));
			}
		}

		CAShapeLayer shapeLayer
		{
			get
			{
				return (CAShapeLayer)Layer;
			}
		}
 		
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (_startAngle == _endAngle)
				_startAngle = _startAngle + (Math.PI * 2);

			//shapeLayer.Path = shapeLayer.Path ?? layoutPath()
		}

	}
}

