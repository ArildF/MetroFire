using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for PasteView.xaml
	/// </summary>
	public partial class PasteView : IPasteView
	{

		/// <summary>
		/// IsUploading Dependency Property
		/// </summary>
		public static readonly DependencyProperty IsUploadingProperty =
			DependencyProperty.Register("IsUploading", typeof(bool), typeof(PasteView),
				new FrameworkPropertyMetadata(false,
					OnIsUploadingChanged));

		/// <summary>
		/// Gets or sets the IsUploading property.  This dependency property 
		/// indicates ....
		/// </summary>
		public bool IsUploading
		{
			get { return (bool)GetValue(IsUploadingProperty); }
			set { SetValue(IsUploadingProperty, value); }
		}

		/// <summary>
		/// Handles changes to the IsUploading property.
		/// </summary>
		private static void OnIsUploadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PasteView)d).OnIsUploadingChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the IsUploading property.
		/// </summary>
		protected virtual void OnIsUploadingChanged(DependencyPropertyChangedEventArgs e)
		{
			var val = (bool) e.NewValue;
			var retval = VisualStateManager.GoToElementState(this, val ? "UploadingState" : "BaseState", true);
			Debug.WriteLine(retval);
		}


		/// <summary>
		/// IsFinished Dependency Property
		/// </summary>
		public static readonly DependencyProperty IsFinishedProperty =
			DependencyProperty.Register("IsFinished", typeof(bool), typeof(PasteView),
				new FrameworkPropertyMetadata((bool)false,
					new PropertyChangedCallback(OnIsFinishedChanged)));

		/// <summary>
		/// Gets or sets the IsFinished property.  This dependency property 
		/// indicates ....
		/// </summary>
		public bool IsFinished
		{
			get { return (bool)GetValue(IsFinishedProperty); }
			set { SetValue(IsFinishedProperty, value); }
		}

		/// <summary>
		/// Handles changes to the IsFinished property.
		/// </summary>
		private static void OnIsFinishedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PasteView)d).OnIsFinishedChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the IsFinished property.
		/// </summary>
		protected virtual void OnIsFinishedChanged(DependencyPropertyChangedEventArgs e)
		{
			var val = (bool) e.NewValue;
			if (val)
			{
				Close();
			}
		}
		
		public PasteView()
		{
			InitializeComponent();
		}

		public PasteView(IPasteViewModel vm) : this()
		{
			DataContext = vm;

			var binding = new Binding("IsUploading") { Mode = BindingMode.OneWay, Source = vm };

			SetBinding(IsUploadingProperty, binding);


			binding = new Binding("IsFinished"){Mode = BindingMode.OneWay, Source=vm};
			SetBinding(IsFinishedProperty, binding);

			progressBar.ValueChanged += ProgressBarOnValueChanged;
		}

		private void ProgressBarOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
		{
			Debug.WriteLine(progressBar.Value);
		}
	}

}
