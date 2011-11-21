﻿using System;
using System.Windows;
using System.Windows.Input;
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
		private readonly INavigationContent _content;

		private bool _isLoaded;
		private IMainCampfireViewModel _model;

		protected MainCampfireView()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			if (!_isLoaded)
			{
				_isLoaded = true;
				_bus.SendMessage(new ModuleLoaded(ModuleNames.MainCampfireView));
			}
		}

		public MainCampfireView(IMainCampfireViewModel model, IMessageBus bus, INavigationContent content) : this()
		{
			_bus = bus;
			_content = content;
			_model = model;
			DataContext = model;

			bus.Listen<ActivateModuleMessage>()
				.Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(ActivateModule);
		}

		private void ActivateModule(ActivateModuleMessage obj)
		{
			var currentModule = _moduleContainer.Content as IModule;
			if (currentModule != null)
			{
				currentModule.IsActive = false;
			}
			_moduleContainer.Content = obj.Module;

			obj.Module.IsActive = true;

			_bus.SendMessage(new ModuleActivatedMessage(obj.Module, obj.ParentModule));
		}

		public string Caption
		{
			get { return ""; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

		public bool IsActive
		{
			get { return true; }
			set{}
		}

		public int Id
		{
			get { throw new NotImplementedException(); }
		}

		public string Notifications
		{
			get { throw new NotImplementedException(); }
		}

		public bool Closable
		{
			get { return false; }
		}

		public DependencyObject NavigationContent
		{
			get { return _content.Visual; }
		}

		
	}
}
