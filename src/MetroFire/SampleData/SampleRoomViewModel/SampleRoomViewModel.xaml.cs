﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SampleRoomViewModel
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class SampleRoomViewModel { }
#else

	public class SampleRoomViewModel : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleRoomViewModel()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/MetroFire;component/SampleData/SampleRoomViewModel/SampleRoomViewModel.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private Users _Users = new Users();

		public Users Users
		{
			get
			{
				return this._Users;
			}
		}

		private string _UserMessage = string.Empty;

		public string UserMessage
		{
			get
			{
				return this._UserMessage;
			}

			set
			{
				if (this._UserMessage != value)
				{
					this._UserMessage = value;
					this.OnPropertyChanged("UserMessage");
				}
			}
		}

		private bool _UserEditingMessage = false;

		public bool UserEditingMessage
		{
			get
			{
				return this._UserEditingMessage;
			}

			set
			{
				if (this._UserEditingMessage != value)
				{
					this._UserEditingMessage = value;
					this.OnPropertyChanged("UserEditingMessage");
				}
			}
		}

		private string _UserEditedMessage = string.Empty;

		public string UserEditedMessage
		{
			get
			{
				return this._UserEditedMessage;
			}

			set
			{
				if (this._UserEditedMessage != value)
				{
					this._UserEditedMessage = value;
					this.OnPropertyChanged("UserEditedMessage");
				}
			}
		}
	}

	public class UsersItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private bool _IsAdmin = false;

		public bool IsAdmin
		{
			get
			{
				return this._IsAdmin;
			}

			set
			{
				if (this._IsAdmin != value)
				{
					this._IsAdmin = value;
					this.OnPropertyChanged("IsAdmin");
				}
			}
		}
	}

	public class Users : System.Collections.ObjectModel.ObservableCollection<UsersItem>
	{ 
	}
#endif
}