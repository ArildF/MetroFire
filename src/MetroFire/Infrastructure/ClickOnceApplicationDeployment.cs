using System.ComponentModel;
using System.Deployment.Application;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ClickOnceApplicationDeployment : IApplicationDeployment
	{
		private readonly ApplicationDeployment _deployment;


		public ClickOnceApplicationDeployment()
		{
			if (IsNetworkDeployed)
			{
				_deployment = ApplicationDeployment.CurrentDeployment;
			}
			
		}
		public bool IsNetworkDeployed
		{
			get { return ApplicationDeployment.IsNetworkDeployed; }
		}

		public void CheckForUpdateAsync()
		{
			_deployment.CheckForUpdateAsync();
		}

		public void UpdateAsync()
		{
			_deployment.UpdateAsync();
		}

		public event CheckForUpdateCompletedEventHandler CheckForUpdateCompleted
		{
			add { _deployment.CheckForUpdateCompleted += value; }
			remove { _deployment.CheckForUpdateCompleted -= value; }
		}
		
		public event AsyncCompletedEventHandler UpdateCompleted
		{
			add { _deployment.UpdateCompleted += value; }
			remove { _deployment.UpdateCompleted -= value; }
		}

		public event DeploymentProgressChangedEventHandler UpdateProgressChanged
		{
			add { _deployment.UpdateProgressChanged += value; }
			remove { _deployment.UpdateProgressChanged -= value; }
		}
	}
}
