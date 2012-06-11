using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ReactiveUI;
using System.Reactive.Linq;
using Rogue.MetroFire.UI.ViewModels;

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
		private readonly IMainCampfireViewModel _model;
		private readonly TranslateTransform _xform;

		protected MainCampfireView()
		{
			InitializeComponent();

			_xform = new TranslateTransform();
			RegisterName("_xform", _xform);

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
				.Where(msg => msg.Module != _moduleContainer.Content)
				.SubscribeUI(ActivateModule);
		}

		private void ActivateModule(ActivateModuleMessage obj)
		{
			var currentModule = _moduleContainer.Content as IModule;
			if (currentModule != null)
			{
				currentModule.IsActive = false;
			}

			RunTransitionAnimation(obj);

			obj.Module.IsActive = true;

			_bus.SendMessage(new ModuleActivatedMessage(obj.Module, obj.ParentModule));
		}

		private void RunTransitionAnimation(ActivateModuleMessage obj)
		{
			var direction = Direction.Left;

			var oldContent = _moduleContainer.Content as UIElement;
			var oldModule = oldContent as IModule;

			if (oldModule != null)
			{
				direction = _model.ModuleDirectionRelativeTo(oldModule, obj.Module);
			}

			var gridColumn = direction == Direction.Right ? 0 : 2;
			if (oldContent != null)
			{
				_moduleContainer.Content = null;
				_animationGrid.Children.Add(oldContent);
				Grid.SetColumn(oldContent, gridColumn);
			}

			_animationGrid.ColumnDefinitions[gridColumn].Width = new GridLength(1, GridUnitType.Star);

			_animationGrid.Margin = direction == Direction.Right
			                        	? new Thickness(0, 0, -ActualWidth, 0)
			                        	: new Thickness(-ActualWidth, 0, 0, 0);

			_animationGrid.RenderTransform = _xform;

			var animation = new DoubleAnimationUsingKeyFrames();
			
			Storyboard.SetTargetName(animation, "_xform");

			Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.XProperty));
			animation.Duration = TimeSpan.FromSeconds(0.16);

			if (direction == Direction.Right)
			{
				animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
				animation.KeyFrames.Add(new EasingDoubleKeyFrame(-ActualWidth, KeyTime.FromPercent(1.0)));
			}
			else
			{
				animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
				animation.KeyFrames.Add(new EasingDoubleKeyFrame(ActualWidth, KeyTime.FromPercent(1.0)));
			}

			var sb = new Storyboard();
			sb.Children.Add(animation);

			animation.Completed += (sender, args) =>
			{
				_animationGrid.Children.Remove(oldContent);
				_animationGrid.ColumnDefinitions[gridColumn].Width = new GridLength(0, GridUnitType.Star);
				_animationGrid.RenderTransform = null;
				_animationGrid.Margin = new Thickness(0, 0, 0, 0);
			};

			_moduleContainer.Content = obj.Module.Visual;

			sb.Begin(_animationGrid);
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
