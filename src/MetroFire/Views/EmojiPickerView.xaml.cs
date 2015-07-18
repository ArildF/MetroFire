using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for EmojiPickerView.xaml
	/// </summary>
	public partial class EmojiPickerView 
	{
		public EmojiPickerView()
		{
			InitializeComponent();

			SetBinding(FocusSelectedCommandProperty, new Binding("FocusSelectedCommand"));
		}

		static EmojiPickerView()
		{
			FrameworkPropertyMetadata md = new FrameworkPropertyMetadata(null, FocusSelectedCommandPropertyChanged);
			FocusSelectedCommandProperty = DependencyProperty.Register("FocusSelectedCommand", typeof (ICommand), typeof (EmojiPickerView), md);                                                      
		}

		public static readonly DependencyProperty FocusSelectedCommandProperty;



		public ICommand FocusSelectedCommand
		{
			get { return (ICommand) GetValue(FocusSelectedCommandProperty); }
			set { SetValue(FocusSelectedCommandProperty, value); }
		}


		private static void FocusSelectedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}



		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (e.Key.In(Key.Up, Key.Left, Key.Down, Key.Right, Key.Enter))
			{
				if (FocusSelectedCommand != null && FocusSelectedCommand.CanExecute(null))
				{
					FocusSelectedCommand.Execute(null);
				}
			}
			else
			{
				SearchTextBox.Focus();
			}
		}
	}
}