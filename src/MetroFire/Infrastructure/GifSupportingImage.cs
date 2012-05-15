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

		public int FrameIndex
		{
			get { return (int)GetValue(FrameIndexProperty); }
			set { SetValue(FrameIndexProperty, value); }
		}

		public static readonly DependencyProperty FrameIndexProperty =
			DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifSupportingImage), new UIPropertyMetadata(0, ChangingFrameIndex));



		public static readonly DependencyProperty UriProperty = DependencyProperty.Register("Uri",
			typeof(string), typeof(GifSupportingImage), new UIPropertyMetadata(null, UriPropertyChanged));



		public string Uri
		{
			get { return (string) GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}


		private static void UriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (GifSupportingImage) d;
			var newValue = (string) e.NewValue;

			owner.InitFromUri(newValue != null ? new Uri(newValue) : null);
		}


		static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
		{
			var ob = (GifSupportingImage)obj;
			ob.Source = ob._gifDecoder.Frames[(int)ev.NewValue];
			ob.InvalidateVisual();
		}


		private void InitFromUri(Uri uri)
		{
			if (uri == null)
			{
				_gifDecoder = null;
				_animation = null;
				_animationIsWorking = false;

				return;
			}

			var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			_gifDecoder = decoder as GifBitmapDecoder;
			if (_gifDecoder != null)
			{
				_animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1,
			                                new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count/10,
			                                                          (int) ((_gifDecoder.Frames.Count/10.0 - _gifDecoder.Frames.Count/10)*1000))))
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

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			if (!_animationIsWorking && _animation != null && _gifDecoder != null)
			{
				BeginAnimation(FrameIndexProperty, _animation);
				_animationIsWorking = true;
			}
		}
	}
}
