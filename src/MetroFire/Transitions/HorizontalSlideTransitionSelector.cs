using System.Collections;
using System.Windows;
using Rogue.MetroFire.UI.ViewModels;
using Transitionals;
using System.Linq;

namespace Rogue.MetroFire.UI.Transitions
{
	public class HorizontalSlideTransitionSelector : TransitionSelector
	{
		public static readonly DependencyProperty CollectionProperty;

		private static readonly Transition Left = new HorizontalSlideTransition {Direction = Direction.Left};
		private static readonly Transition Right = new HorizontalSlideTransition {Direction = Direction.Right};


		static HorizontalSlideTransitionSelector()
		{

			var md = new FrameworkPropertyMetadata(null, PropertyChangedCallback);
			CollectionProperty = DependencyProperty.Register("Collection", typeof(IEnumerable), typeof(HorizontalSlideTransitionSelector), md);                                                      
		}

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			
		}


		public IEnumerable Collection
		{
			get { return (IEnumerable) GetValue(CollectionProperty); }
			set { SetValue(CollectionProperty, value); }
		}


		public override Transition SelectTransition(object oldContent, object newContent)
		{
			if (Collection == null)
			{
				return null;
			}

			var idx1 = Find(oldContent);
			var idx2 = Find(newContent);

			if (idx1 < idx2)
			{
				return Left;
			}
			return Right;

		}

		private int Find(object oldContent)
		{
			var idx1 = Collection.Cast<object>().Select((item, index) => new {item, index})
			                     .Where(e => e.item == oldContent).Select(t => t.index).FirstOrDefault();
			return idx1;
		}
	}
}
