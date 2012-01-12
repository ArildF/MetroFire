using System;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class UserViewModel
	{
		private readonly User _user;

		public UserViewModel(User user)
		{
			_user = user;
		}

		public string Name
		{
			get { return _user.Name; }
		}

		public string AvatarUrl
		{
			get { return _user.AvatarUrl; }
		}

		public string EmailAddress
		{
			get { return _user.EmailAddress; }
		}

		public DateTime CreatedDate
		{
			get { return _user.CreatedAt; }
		}

		public User User
		{
			get {
				return _user;
			}
		}
	}
}