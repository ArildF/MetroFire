using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Infrastructure
{
	// Code based on code and discussion from http://stackoverflow.com/questions/210922/how-do-i-get-an-animated-gif-to-work-in-wpf
	public class GifSupportingImage : Image
	{
		Int32Animation _animation;
		GifBitmapDecoder _gifDecoder;
		bool _animationIsWorking;

		private ImageSource[] _cachedFrames;

		public int FrameIndex
		{
			get { return (int)GetValue(FrameIndexProperty); }
			set { SetValue(FrameIndexProperty, value); }
		}

		public static readonly DependencyProperty FrameIndexProperty =
			DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifSupportingImage), new UIPropertyMetadata(0, ChangingFrameIndex));



		public static readonly DependencyProperty UriProperty = DependencyProperty.Register("Uri",
			typeof(string), typeof(GifSupportingImage), new UIPropertyMetadata(null, UriPropertyChanged));


		public static readonly DependencyProperty ShowAnimatedProperty = DependencyProperty.Register("ShowAnimated",
			typeof(bool), typeof(GifSupportingImage), new UIPropertyMetadata(true, ShowAnimatedPropertyChanged));



		public bool ShowAnimated
		{
			get { return (bool)GetValue(ShowAnimatedProperty); }
			set { SetValue(ShowAnimatedProperty, value); }
		}


		private static void ShowAnimatedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (GifSupportingImage)d;

			owner.InitFromUri(owner.Uri != null ? new Uri(owner.Uri) : null);
		}

		public string Uri
		{
			get { return (string)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}


		private static void UriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (GifSupportingImage)d;
			var newValue = (string)e.NewValue;

			owner.InitFromUri(newValue != null ? new Uri(newValue) : null);
		}


		static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
		{
			var ob = (GifSupportingImage)obj;
			ob.Source = ob.GetFrame((int)ev.NewValue);
			ob.InvalidateVisual();
		}

		private ImageSource GetFrame(int index)
		{
			var firstFrame = _gifDecoder.Frames[0];
			if (index == 0)
			{
				return firstFrame;
			}

			var cachedFrame = _cachedFrames[index];

			if (cachedFrame != null)
			{
				return cachedFrame;
			}

			var currentFrame = _gifDecoder.Frames[index];

			var dv = new DrawingVisual();
			using (var dc = dv.RenderOpen())
			{
				dc.DrawImage(Source, new Rect(0, 0, firstFrame.PixelWidth, firstFrame.PixelHeight));
				dc.DrawImage(currentFrame, new Rect(0, 0, firstFrame.PixelWidth, firstFrame.PixelHeight));
			}

			var rtb = new RenderTargetBitmap(firstFrame.PixelWidth, firstFrame.PixelHeight, firstFrame.DpiX,
				firstFrame.DpiY, PixelFormats.Default);
			rtb.Render(dv);

			_cachedFrames[index] = rtb;

			return rtb;
		}


		private void InitFromUri(Uri uri)
		{
			StopExistingAnimation();
			if (uri == null)
			{
				return;
			}

			var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			_gifDecoder = decoder as GifBitmapDecoder;
			if (_gifDecoder != null && ShowAnimated)
			{
				_cachedFrames = new ImageSource[_gifDecoder.Frames.Count];
				_animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1,
											new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count / 10,
																	  (int)((_gifDecoder.Frames.Count / 10.0 - _gifDecoder.Frames.Count / 10) * 1000))))
				{
					RepeatBehavior = RepeatBehavior.Forever
				};

			}
			else
			{
				_animation = null;
				_animationIsWorking = false;
			}

			Source = decoder.Frames[0];
		}

		private void StopExistingAnimation()
		{
			if (_animationIsWorking)
			{
				BeginAnimation(FrameIndexProperty, null);
			}
			_animation = null;
			_animationIsWorking = false;
			_gifDecoder = null;
			_cachedFrames = null;
		}

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			if (!_animationIsWorking && ShowAnimated && _animation != null && _gifDecoder != null)
			{
				BeginAnimation(FrameIndexProperty, _animation);
				_animationIsWorking = true;
			}
		}
	}
}
