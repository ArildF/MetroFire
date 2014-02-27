using System.Text;
using FluentAssertions;
using Rogue.MetroFire.UI.Notifications;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels.Settings;
using TechTalk.SpecFlow;
using System.Linq;
using TechTalk.SpecFlow.Assist;

namespace MetroFire.Specs.Steps
{
	[Binding]
	public class NotificationSettingsSteps
	{
		private RoomContext _context;
		private NotificationSettingsViewModel _vm;
		private NotificationViewModel _currentNotification;
		private TriggerViewModel _currentTrigger;
		private ActionViewModel _currentAction;

		public NotificationSettingsSteps(RoomContext context)
		{
			_context = context;

			_vm = context.SettingsViewModel.SettingsViewModels.OfType<NotificationSettingsViewModel>().First();
		}

		[Given(@"a new notification")]
		public void GivenANewNotification()
		{
			_vm.AddNotificationCommand.Execute(null);
			_currentNotification = _vm.Notifications.Last();
			_currentNotification.DeleteItemCommand.Execute(_currentNotification.Triggers.First());
			_currentNotification.DeleteItemCommand.Execute(_currentNotification.Actions.First());
		}

		[Given(@"the notification has the following triggers:")]
		public void GivenTheNotificationHasTheFollowingTriggers(Table table)
		{
			var triggers = table.CreateSet<NotificationTrigger>();
			foreach (var notificationTrigger in triggers)
			{
				_currentNotification.AddNewTriggerCommand.Execute(null);
				var trigger = _currentNotification.Triggers.Last();
				trigger.SelectedTriggerType = trigger.TriggerTypes.First(t => t.Data == notificationTrigger.TriggerType);
				trigger.MatchRoom = notificationTrigger.MatchRoom;
				trigger.DoMatchRoom = !string.IsNullOrEmpty(notificationTrigger.MatchRoom);
				trigger.MatchText = notificationTrigger.MatchText;
				trigger.DoMatchText = !string.IsNullOrEmpty(notificationTrigger.MatchText);
				trigger.MatchUser = notificationTrigger.MatchUser;
				trigger.DoMatchUser = !string.IsNullOrEmpty(notificationTrigger.MatchUser);
			}
			_currentNotification.CollapseAll();
		}

		


		[Given(@"the notification has the following actions")]
		public void GivenTheNotificationHasTheFollowingActions(Table table)
		{
			var actions = table.CreateSet<NotificationAction>();
			foreach (var notificationAction in actions)
			{
				_currentNotification.AddNewActionCommand.Execute(null);
				var action = _currentNotification.Actions.Last();
				action.SelectedAction = action.ActionTypes.First(at => at.Data.ActionType == notificationAction.ActionType);
			}
		}

		[When(@"I close notification \#(\d+)")]
		public void WhenICloseNotification1(int num)
		{
			_vm.DeleteNotificationCommand.Execute(_vm.Notifications[num - 1]);
		}

		[When(@"I double click on trigger \#(\d+)")]
		public void WhenIDoubleClickOnTrigger1(int num)
		{
			_vm.ToggleEditCommand.Execute(_currentNotification.Triggers[num - 1]);
		}

		[When(@"I add a new notification")]
		public void WhenIAddANewNotification()
		{
			_vm.AddNotificationCommand.Execute(null);
			_currentNotification = _vm.Notifications.Last();
		}


		[When(@"I add a new trigger")]
		public void WhenIAddANewTrigger()
		{
			_currentNotification.AddNewTriggerCommand.Execute(null);
			_currentTrigger = _currentNotification.Triggers.Last();
		}

		[When(@"I add a new action")]
		public void WhenIAddANewAction()
		{
			_currentNotification.AddNewActionCommand.Execute(null);
			_currentAction = _currentNotification.Actions.Last();
		}


		[Then(@"the new trigger should be editable")]
		public void ThenTheNewTriggerShouldBeEditable()
		{
			_currentTrigger.IsEditing.Should().BeTrue();
		}

		[Then(@"the new action should be editable")]
		public void ThenTheNewActionShouldBeEditable()
		{
			_currentAction.IsEditing.Should().BeTrue();
		}

		[Then(@"the new trigger should be selected")]
		public void ThenTheNewTriggerShouldBeSelected()
		{
			_currentNotification.SelectedTrigger.Should().Be(_currentTrigger);
		}

		[Then(@"the new notification should have a trigger")]
		public void ThenItShouldHaveATrigger()
		{
			_currentNotification.Triggers.Any().Should().BeTrue();
		}

		[Then(@"the new notification should have an action")]
		public void ThenTheNewNotificationShouldHaveAnAction()
		{
			_currentNotification.Actions.Any().Should().BeTrue();
		}



		[Then(@"the new action should be selected")]
		public void ThenTheNewActionShouldBeSelected()
		{
			_currentNotification.SelectedAction.Should().Be(_currentAction);
		}



		[Then(@"there should be (\d+) notifications")]
		public void ThenThereShouldBe0Notifications(int num)
		{
			_vm.Notifications.Count.Should().Be(num);
		}

		[Then(@"trigger \#(\d+) should be editable")]
		public void ThenTrigger1ShouldBeEditable(int num)
		{
			_currentNotification.Triggers[num - 1].IsEditing.Should().BeTrue();
		}

		[Then(@"action \#(\d+) should be editable")]
		public void ThenAction1ShouldBeEditable(int num)
		{
			_currentNotification.Actions[num - 1].IsEditing.Should().BeTrue();
		}


		[Then(@"all triggers except \#(\d+) should not be editable")]
		public void ThenAllTriggersExcept1ShouldNotBeEditable(int num)
		{
			_currentNotification.Triggers.Select((vm, index) => new {Index = index, VM = vm})
				.Where(i => i.Index != num - 1).All(a => a.VM.IsEditing == false).Should().BeTrue();
		}

		[Then(@"all actions should not be editable")]
		public void ThenAllActionsShouldNotBeEditable()
		{
			_currentNotification.Actions.All(a => a.IsEditing == false).Should().BeTrue();
		}




		[Then(@"the screen should read like this:")]
		public void ThenTheScreenShouldReadLikeThis(Table table)
		{
			string text = string.Join("\r\n", table.Rows.Select(r => r["Line"].Trim()));

			var sb = new StringBuilder();

			var actual = string.Join("\r\n", _vm.Notifications.Select(n => "When\r\n" +
				string.Join("\r\n", n.Triggers.Select(t => t.DisplayText)) + "\r\n" + 
				"then\r\n" +
				string.Join("\r\n", n.Actions.Select(a => a.DisplayText))
				));

			actual.Should().Be(text);
		}
	}
}
