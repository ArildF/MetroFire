using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class UserCache : IStartable, IUserCache
	{
		private readonly IMessageBus _bus;
		private readonly HashSet<User> _users = new HashSet<User>(UserComparer.Instance);
		private readonly HashSet<int> _pendingUserInformationRequests = new HashSet<int>();

		public UserCache(IMessageBus bus)
		{
			_bus = bus;
		}

		public void Start()
		{
			_bus.Listen<RoomInfoReceivedMessage>().SubscribeThreadPool(UpdateFromRoomInfoReceived);
			_bus.Listen<UserInfoReceivedMessage>().SubscribeThreadPool(UpdateFromUserInfoReceived);
		}

		private void UpdateFromUserInfoReceived(UserInfoReceivedMessage obj)
		{
			lock (_pendingUserInformationRequests)
			{
				Update(obj.User);
				if (_pendingUserInformationRequests.Contains(obj.User.Id))
				{
					_pendingUserInformationRequests.Remove(obj.User.Id);
				}
			}
		}

		private void UpdateFromRoomInfoReceived(RoomInfoReceivedMessage roomInfoReceivedMessage)
		{
			Update(roomInfoReceivedMessage.Room.Users);
		}

		private void Update(params User[] users)
		{
			List<User> usersToUpdate;

			lock (_users)
			{
				usersToUpdate = users
					.Where(user => !_users.Contains(user) || UserHasChanged(user, _users.First(u => u.Id == user.Id))).ToList();
				foreach (var user in usersToUpdate)
				{
					_users.Remove(user);
					_users.Add(user);
				}
			}

			if (usersToUpdate.Any())
			{
				_bus.SendMessage(new UsersUpdatedMessage(usersToUpdate));
			}
		}

		private bool UserHasChanged(User newUser, User original)
		{
			return newUser.Admin != original.Admin ||
			       newUser.AvatarUrl != original.AvatarUrl ||
			       newUser.Name != original.Name ||
			       newUser.Type != original.Type;

		}

		public void Stop()
		{
			
		}

		private class UserComparer : IEqualityComparer<User>
		{
			public static readonly UserComparer Instance = new UserComparer();

			public bool Equals(User x, User y)
			{
				return x.Id == y.Id;
			}

			public int GetHashCode(User obj)
			{
				return obj.Id.GetHashCode();
			}
		}

		public User GetUser(int id, User existingUser)
		{
			var user = _users.Where(u => u.Id == id).FirstOrDefault();
			if (user == null)
			{
				if (existingUser == null)
				{
					lock (_pendingUserInformationRequests)
					{
						if (!_pendingUserInformationRequests.Contains(id))
						{
							_pendingUserInformationRequests.Add(id);

							_bus.SendMessage(new RequestUserInfoMessage(id));
						}
						
					}
				}
				else
				{
					_users.Add(existingUser);
					user = existingUser;
				}
			}

			return user;
		}

		public bool UserUpdated(User user, User newUser)
		{
			return UserHasChanged(newUser, user);
		}
	}

	
}
