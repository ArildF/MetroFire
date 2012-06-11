using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public static class WpfExtensions
	{
		public static T FindVisualChild<T>(this UIElement element) where T : FrameworkElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				var elt = child as T;
				if (elt != null)
				{
					return elt;
				}
				if (child is UIElement)
				{
					elt = FindVisualChild<T>(child as UIElement);
				}
				if (elt != null)
				{
					return elt;
				}
			}
			return null;
		}

		public static FrameworkElement FindVisualChild<T>(this DependencyObject element, string name) where T : FrameworkElement
		{
			if (element == null)
			{
				return null;
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				FrameworkElement elt = child as T;
				if (elt != null && elt.Name == name)
				{
					return elt;
				}
				elt = FindVisualChild<T>(child, name);
				if (elt != null)
				{
					return elt;
				}
			}
			return null;
		}

		
	}


}
