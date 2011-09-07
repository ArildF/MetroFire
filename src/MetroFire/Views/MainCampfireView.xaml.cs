﻿using System;
using System.Windows;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for MainCampfireView.xaml
	/// </summary>
	public partial class MainCampfireView : IMainModule
	{
		private readonly IMessageBus _bus;

		protected MainCampfireView()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_bus.SendMessage(new ModuleLoaded(ModuleNames.MainCampfireView));
		}

		public MainCampfireView(IMainCampfireViewModel model, IMessageBus bus) : this()
		{
			_bus = bus;
			DataContext = model;

			bus.Listen<ActivateModuleMessage>()
				.Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg => _moduleContainer.Content = msg.Module);
		}

		public string Caption
		{
			get { return ""; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

	}
}