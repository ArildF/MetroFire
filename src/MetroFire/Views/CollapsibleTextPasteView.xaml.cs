using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for CollapsibleTextPasteView.xaml
	/// </summary>
	public partial class CollapsibleTextPasteView : ICollapsibleTextPasteView
	{
		public static readonly int CollapsedHeight = 130;
		private readonly LinearGradientBrush _brush;

		private CollapsibleTextPasteView()
		{	
			InitializeComponent();
			_grid.MaxHeight = CollapsedHeight;

			Loaded += OnLoaded;

			_brush = new LinearGradientBrush
				{
					StartPoint = new Point(0.5, 0), 
					EndPoint = new Point(0.5, 1)
				};
			_brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 0, 0), 0));
			_brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 0, 0), 0.5));
			_brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
		}

		public CollapsibleTextPasteView(Inline inline) : this()
		{
			var paragraph = new Paragraph(inline);
			var document = new FlowDocument(paragraph);
			_block.Document = document;
				//((Paragraph) _block.Document.Blocks.FirstBlock).Inlines.Add(inline);
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_button.Visibility = _block.DesiredSize.Height >= _grid.MaxHeight ? Visibility.Visible : Visibility.Collapsed;
			_innerGrid.OpacityMask = _button.Visibility == Visibility.Visible ? _brush : null;
		}


		public FrameworkElement Element { get { return this; } }

		private void ButtonOnClick(object sender, RoutedEventArgs e)
		{
			_grid.MaxHeight = Double.IsPositiveInfinity(_grid.MaxHeight) ? CollapsedHeight : Double.PositiveInfinity;
			_button.Content = Double.IsPositiveInfinity(_grid.MaxHeight) ? "\u02c4" : "\u02c5";
			_innerGrid.OpacityMask = Double.IsPositiveInfinity(_grid.MaxHeight) ? null : _brush;
		}

		private void CopyButtonOnClick(object sender, RoutedEventArgs e)
		{
			_block.SelectAll();
			_block.Copy();
		}
	}
}
