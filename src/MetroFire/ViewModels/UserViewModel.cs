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

		public User User
		{
			get {
				return _user;
			}
		}
	}
}