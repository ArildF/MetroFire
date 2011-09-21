using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : IMainModule
	{
		public static readonly DependencyProperty IsLoggingInProperty;


		static LoginView()
		{
			var md = new FrameworkPropertyMetadata(false, IsLoggingInPropertyChanged);
			IsLoggingInProperty = DependencyProperty.Register("IsLoggingIn", typeof (bool), typeof (LoginView), md);
			
		}

		/// <summary>
		/// A property wrapper for the <see cref="IsLoggingInProperty"/>
		/// dependency property:<br/>
		/// Description
		/// </summary>
		public bool IsLoggingIn
		{
			get { return (bool) GetValue(IsLoggingInProperty); }
			set { SetValue(IsLoggingInProperty, value); }
		}


		private static void IsLoggingInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (LoginView) d;
			var newValue = (bool) e.NewValue;

			bool retval = VisualStateManager.GoToElementState(owner._root, newValue ? "LoggingInState" : "NormalState", true);
			Debug.WriteLine("RetVal: " + retval);
		}

		public LoginView()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var retval = VisualStateManager.GoToElementState(_root, "NormalState", false);
			Debug.WriteLine("RetVal: " + retval);
		}

		public LoginView(ILoginViewModel viewModel) : this()
		{
			DataContext = viewModel;

			var binding = new Binding("IsLoggingIn") {Mode = BindingMode.OneWay, Source = viewModel};

			SetBinding(IsLoggingInProperty, binding);
		}

		public string Caption
		{
			get { return ""; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

		public bool IsActive
		{
			get { return true; }
			set{}
		}

		public int Id
		{
			get { return -1; }
		}

		public string Notifications
		{
			get { return ""; }
		}
	}
}
