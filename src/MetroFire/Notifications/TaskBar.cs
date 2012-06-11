using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Rogue.MetroFire.UI.Notifications
{
	public class TaskBar : ITaskBar
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			public UInt32 cbSize;
			public IntPtr hwnd;
			public Int32 dwFlags;
			public UInt32 uCount;
			public Int32 dwTimeout;
		}

		[Flags]
		private enum FLASHWINFOFLAGS
		{
			FLASHW_STOP = 0,
			FLASHW_CAPTION = 0x00000001,
			FLASHW_TRAY = 0x00000002,
			FLASHW_ALL = (FLASHW_CAPTION | FLASHW_TRAY),
			FLASHW_TIMER = 0x00000004,
			FLASHW_TIMERNOFG = 0x0000000C
		}

		[DllImport("user32.dll")]

		private static extern int FlashWindowEx(ref FLASHWINFO pfwi);

		public void Flash()
		{
			var info = new FLASHWINFO
				{
					cbSize = (uint) Marshal.SizeOf(typeof (FLASHWINFO)),
					hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle,
					dwFlags = (int) (FLASHWINFOFLAGS.FLASHW_ALL | FLASHWINFOFLAGS.FLASHW_TIMERNOFG),
					dwTimeout = 0
				};
			FlashWindowEx(ref info);
		}
	}
}
