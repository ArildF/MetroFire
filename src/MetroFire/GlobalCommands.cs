using ReactiveUI.Xaml;

namespace Rogue.MetroFire.UI
{
	public class GlobalCommands
	{
		public static ReactiveCommand NextModuleCommand = new ReactiveCommand();
		public static ReactiveCommand PreviousModuleCommand = new ReactiveCommand();
	}



	public class GlobalCommandsImpl : IGlobalCommands
	{
		public ReactiveCommand NextModuleCommand{get{ return GlobalCommands.NextModuleCommand;}}
		public ReactiveCommand PreviousModuleCommand{get{ return GlobalCommands.PreviousModuleCommand;}}
	}
}
