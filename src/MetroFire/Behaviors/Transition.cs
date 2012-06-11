using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class Transition : FrameworkElement
	{
		public static readonly DependencyProperty VisualStateProperty =
			DependencyProperty.Register("VisualState", typeof (string), typeof (Transition), new PropertyMetadata(default(string)));

		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof (object), typeof (Transition),
			                            new PropertyMetadata(default(object), OnSourceChanged));


		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof (object), typeof (Transition), new PropertyMetadata(default(object)));

		public static readonly DependencyProperty UseTransitionsProperty =
			DependencyProperty.Register("UseTransitions", typeof (bool), typeof (Transition), new PropertyMetadata(true));


		private readonly Subject<Transition> _subject = new Subject<Transition>();


		public Subject<Transition> StateChange { get { return _subject; } }

		public bool UseTransitions
		{
			get { return (bool) GetValue(UseTransitionsProperty); }
			set { SetValue(UseTransitionsProperty, value); }
		}

		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public object Source
		{
			get { return GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public string VisualState
		{
			get { return (string) GetValue(VisualStateProperty); }
			set { SetValue(VisualStateProperty, value); }
		}

		public void EffectTransition(FrameworkElement associatedObject)
		{
			VisualStateManager.GoToElementState(associatedObject, VisualState, UseTransitions);
		}

		private static void OnSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var transition = (Transition) dependencyObject;
			transition.CheckForTransition();
		}

		private void CheckForTransition()
		{
			if ((Source == null && Value == null))
			{
				_subject.OnNext(this);
			}
			else if (Source != null && Value != null)
			{
				var val = Convert.ChangeType(Value, Source.GetType());
				if (Source.Equals(val))
				{
					_subject.OnNext(this);
				}
			}
		}
	}
}
