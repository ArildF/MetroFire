using System;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ViewModelRegistrationFacility : AbstractFacility
	{
		private readonly ResourceDictionary _resources;

		public ViewModelRegistrationFacility(ResourceDictionary resources)
		{
			_resources = resources;
		}

		public ViewModelRegistrationFacility(Application application) : this(application.Resources)
		{
		}

		protected override void Init()
		{
			Kernel.ComponentRegistered += KernelOnComponentRegistered;
		}

		private void KernelOnComponentRegistered(string key, IHandler handler)
		{
			Type implementation = handler.ComponentModel.Implementation;
			if (implementation.Namespace == null || 
				!implementation.Namespace.EndsWith("ViewModels"))
			{
				return;
			}

			var viewTypeName = implementation.Namespace.Replace("ViewModels", "Views") + "." +
			                    implementation.Name.Replace("ViewModel", "View");

			var qualifiedViewTypeName = implementation.AssemblyQualifiedName.Replace(implementation.FullName, viewTypeName);

			Type viewType = Type.GetType(qualifiedViewTypeName, false);
			if (viewType == null)
			{
				return;
			}

			var presentation = XNamespace.Get("http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			var vm = XNamespace.Get("clr-namespace:" + viewType.Namespace + ";assembly=" + viewType.Assembly);

			var x = new XElement(presentation + "DataTemplate",
				new XElement(vm + viewType.Name)
				);

			var xamlString = x.ToString();

			var dt = (DataTemplate)XamlReader.Parse(xamlString);


			var dataTemplateKey = new DataTemplateKey(implementation);
			if (!_resources.Contains(dataTemplateKey))
			{
				_resources.Add(dataTemplateKey, dt);
			}
		}
	}
}
