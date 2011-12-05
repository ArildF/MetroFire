using System.Windows;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var bootstrapper = new Bootstrapper();
			var shellView = bootstrapper.Bootstrap();

			var bus = bootstrapper.Resolve<IMessageBus>();

			bus.RegisterMessageSource(this.GetActivated().Select(_ => new ApplicationActivatedMessage()));
			bus.RegisterMessageSource(this.GetDeactivated().Select(_ => new ApplicationDeactivatedMessage()));

			shellView.Window.Show();

			var toastWindow = bootstrapper.Resolve<IToastWindow>();
			toastWindow.Window.Show();

			//int i = 1;
			//bus.RegisterMessageSource(Observable.Interval(TimeSpan.FromSeconds(3)).Select(_ => i++)
			//    .Take(100).Select(num => new ShowToastMessage(
			//    new NotificationMessage(
			//        new Room { Name = "Ohai " + num },
			//        new User { Name = "Arild" },
			//        new Message { Body = "Ohai thar " + num, MessageTypeString = MessageType.TextMessage.ToString() }),
			//        new ShowToastNotificationAction())));
		}
	}
}
