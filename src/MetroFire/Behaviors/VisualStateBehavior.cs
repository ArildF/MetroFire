using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Markup;

namespace Rogue.MetroFire.UI.Behaviors
{
	[ContentProperty("Transitions")]
	public class VisualStateBehavior : Behavior<FrameworkElement>
	{
		private IDisposable _subscription;

		public static readonly DependencyProperty TransitionsProperty =
			DependencyProperty.Register("Transitions", typeof (IList), 
			typeof (VisualStateBehavior), new PropertyMetadata(null));

		public static readonly DependencyProperty DataContextProperty =
			DependencyProperty.Register("DataContext", typeof (object), typeof (VisualStateBehavior), new PropertyMetadata(default(object)));

		public VisualStateBehavior()
		{
			SetValue(TransitionsProperty, new ObservableCollection<Transition>());
		}
		public object DataContext
		{
			get { return GetValue(DataContextProperty); }
			set { SetValue(DataContextProperty, value); }
		}
		
		public IList Transitions
		{
			get { return GetValue(TransitionsProperty) as IList; }
		}


		protected override void OnAttached()
		{
			base.OnAttached();

			HookObservables();

			AssociatedObject.DataContextChanged += AssociatedObjectOnDataContextChanged;
			AssignDataContext(AssociatedObject.DataContext);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			UnhookObservables();

			AssociatedObject.DataContextChanged -= AssociatedObjectOnDataContextChanged;
			AssignDataContext(null);
		}

		private void UnhookObservables()
		{
			if (_subscription != null)
			{
				_subscription.Dispose();
			}
		}


		private void AssociatedObjectOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			AssignDataContext(AssociatedObject.DataContext);
		}

		private void AssignDataContext(object dataContext)
		{
			foreach (Transition transition in Transitions)
			{
				transition.DataContext = dataContext;
			}
		}

		private void HookObservables()
		{
			if (_subscription != null)
			{
				_subscription.Dispose();
			}

			_subscription = Transitions.OfType<Transition>().Select(t => t.StateChange).Merge()
				.Subscribe(t => t.EffectTransition(AssociatedObject));
			
		}
	}
}
