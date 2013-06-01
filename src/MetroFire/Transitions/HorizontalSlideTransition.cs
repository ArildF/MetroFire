using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Rogue.MetroFire.UI.ViewModels;
using Transitionals;

namespace Rogue.MetroFire.UI.Transitions
{
	public class HorizontalSlideTransition : Transition
	{
		public static readonly DependencyProperty DirectionProperty;

		static HorizontalSlideTransition()
		{
			var md = new FrameworkPropertyMetadata(Direction.Left, DirectionPropertyChanged);
			DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(HorizontalSlideTransition), md);
			
		}


		public Direction Direction
		{
			get { return (Direction) GetValue(DirectionProperty); }
			set { SetValue(DirectionProperty, value); }
		}

		protected override void BeginTransition(Transitionals.Controls.TransitionElement transitionElement, System.Windows.Controls.ContentPresenter oldContent, System.Windows.Controls.ContentPresenter newContent)
		{
			var ttNew = new TranslateTransform();
			var ttOld = new TranslateTransform();

			oldContent.RenderTransform = ttOld;
			newContent.RenderTransform = ttNew;

			var oldAnimation = new DoubleAnimationUsingKeyFrames {Duration = TimeSpan.FromMilliseconds(160)};
			Storyboard.SetTarget(oldAnimation, oldContent);
			Storyboard.SetTargetProperty(oldAnimation, new PropertyPath("(0).(1)", 
				UIElement.RenderTransformProperty, TranslateTransform.XProperty));

			var newAnimation = new DoubleAnimationUsingKeyFrames {Duration = TimeSpan.FromMilliseconds(160)};
			Storyboard.SetTarget(newAnimation, newContent);
			Storyboard.SetTargetProperty(newAnimation, new PropertyPath("(0).(1)", 
				UIElement.RenderTransformProperty, TranslateTransform.XProperty));

			if (Direction == Direction.Right)
			{
				ttNew.X = -newContent.ActualWidth;
				oldAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
				oldAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(oldContent.ActualWidth, KeyTime.FromPercent(1.0)));

				newAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(-newContent.ActualWidth, KeyTime.FromPercent(0)));
				newAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));
			}
			else
			{
				ttNew.X = oldContent.ActualWidth;
				oldAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
				oldAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(-oldContent.ActualWidth, KeyTime.FromPercent(1.0)));

				newAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(oldContent.ActualWidth, KeyTime.FromPercent(0)));
				newAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));
			}

			var storyBoard = new Storyboard();
			storyBoard.Children.Add(oldAnimation);
			storyBoard.Children.Add(newAnimation);

			var sw = Stopwatch.StartNew();

			storyBoard.Completed += (sender, args) =>
				{
					EndTransition(transitionElement, oldContent, newContent);
					Debug.WriteLine(ttNew.X);
					Debug.WriteLine(ttOld.Y);
					Debug.WriteLine(sw.Elapsed);
				};
			storyBoard.Begin();
		}


		private static void DirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}


	}

}
