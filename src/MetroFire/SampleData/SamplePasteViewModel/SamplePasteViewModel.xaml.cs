﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SamplePasteViewModel
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class SamplePasteViewModel { }
#else

	public class SamplePasteViewModel : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SamplePasteViewModel()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/MetroFire;component/SampleData/SamplePasteViewModel/SamplePasteViewModel.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private string _Caption = string.Empty;

		public string Caption
		{
			get
			{
				return this._Caption;
			}

			set
			{
				if (this._Caption != value)
				{
					this._Caption = value;
					this.OnPropertyChanged("Caption");
				}
			}
		}

		private bool _IsUploading = false;

		public bool IsUploading
		{
			get
			{
				return this._IsUploading;
			}

			set
			{
				if (this._IsUploading != value)
				{
					this._IsUploading = value;
					this.OnPropertyChanged("IsUploading");
				}
			}
		}

		private double _Size = 0;

		public double Size
		{
			get
			{
				return this._Size;
			}

			set
			{
				if (this._Size != value)
				{
					this._Size = value;
					this.OnPropertyChanged("Size");
				}
			}
		}
	}
#endif
}
