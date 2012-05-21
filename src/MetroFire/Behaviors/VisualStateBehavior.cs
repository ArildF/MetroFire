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
			typeof (VisualStateBehavior), new PropertyMetadata(new ObservableCollection<Transition>()));

		public static readonly DependencyProperty DataContextProperty =
			DependencyProperty.Register("DataContext", typeof (object), typeof (VisualStateBehavior), new PropertyMetadata(default(object)));

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
			AssignDataContext();
		}


		private void AssociatedObjectOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			AssignDataContext();
		}

		private void AssignDataContext()
		{
			foreach (Transition transition in Transitions)
			{
				transition.DataContext = AssociatedObject.DataContext;
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
