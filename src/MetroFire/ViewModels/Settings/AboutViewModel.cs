﻿using System.Reflection;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class AboutViewModel : ISettingsSubPage
	{
		public string AppVersion
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public string Title
		{
			get { return "A_bout"; }
		}

		public void Commit()
		{
			// this space intentionally left blank
		}
	}
}
