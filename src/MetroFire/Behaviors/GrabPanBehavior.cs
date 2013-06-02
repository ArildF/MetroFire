using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class GrabPanBehavior : Behavior<UIElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			var parent = LogicalTreeHelper.GetParent(AssociatedObject) as IInputElement;

			if (parent == null)
			{
				throw new InvalidOperationException("Parent needs to be an input element");
			}

			var down = Observable.FromEventPattern<MouseButtonEventArgs>(AssociatedObject, "PreviewMouseDown")
				.Publish();
			var up = Observable.FromEventPattern<MouseButtonEventArgs>(AssociatedObject, "PreviewMouseUp");
			var move = Observable.FromEventPattern<MouseEventArgs>(AssociatedObject, "MouseMove");

			// we need to multiplex this, since no more handlers will be executed after .Handled = true
			down.Connect();

			var startPoint = new Point();
			TranslateTransform translateTransform = null;

			down.Where(ep => !AssociatedObject.IsMouseCaptured).Subscribe(ep =>
				{
					AssociatedObject.CaptureMouse();

					// use the position relative to the parent as the baseline
					// otherwise the position reported will be impacted by the rendertransform
					startPoint = ep.EventArgs.GetPosition(parent);
					translateTransform = AssociatedObject.RenderTransform as TranslateTransform ?? new TranslateTransform();

					// account for an existing translation (previous drag?)
					startPoint.Offset(-translateTransform.X, -translateTransform.Y);

					AssociatedObject.RenderTransform = translateTransform;
					ep.EventArgs.Handled = true;
				});


			var captured = move.SkipUntil(down).TakeUntil(up).Repeat();
			captured.Subscribe(
				ep =>
				{
					var currentPosition = ep.EventArgs.GetPosition(parent);
					var delta = currentPosition - startPoint;
					translateTransform.X = delta.X;
					translateTransform.Y = delta.Y;
				});

			up.Subscribe(_ => AssociatedObject.ReleaseMouseCapture());

			AssociatedObject.QueryCursor += (_, e) => e.Cursor = Cursors.SizeAll;
		}
	}
}
