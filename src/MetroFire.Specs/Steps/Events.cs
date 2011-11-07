using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using TechTalk.SpecFlow;

namespace MetroFire.Specs.Steps
{
	[Binding]
	public class Events
	{
		public static TestScheduler TestScheduler;
		private IScheduler _previousScheduler;
		private IScheduler _previousTaskPoolScheduler;

		[BeforeStep]
		public void BeforeStep()
		{
			//TODO: implement logic that has to run before each scenario step
			// For storing and retrieving scenario-specific data, 
			// the instance fields of the class or the
			//     ScenarioContext.Current
			// collection can be used.
			// For storing and retrieving feature-specific data, the 
			//     FeatureContext.Current
			// collection can be used.
			// Use the attribute overload to specify tags. If tags are specified, the event 
			// handler will be executed only if any of the tags are specified for the 
			// feature or the scenario.
			//     [BeforeStep("mytag")]
		}

		[AfterStep]
		public void AfterStep()
		{
			//TODO: implement logic that has to run after each scenario step
		}

		[BeforeScenarioBlock]
		public void BeforeScenarioBlock()
		{
			//TODO: implement logic that has to run before each scenario block (given-when-then)
		}

		[AfterScenarioBlock]
		public void AfterScenarioBlock()
		{
			//TODO: implement logic that has to run after each scenario block (given-when-then)
		}

		[BeforeScenario("testscheduler")]
		public void BeforeScenario()
		{
			TestScheduler = new TestScheduler();
			_previousScheduler = RxApp.DeferredScheduler;
			_previousTaskPoolScheduler = RxApp.TaskpoolScheduler;
			RxApp.DeferredScheduler = TestScheduler;
			RxApp.TaskpoolScheduler = TestScheduler;
		}

		[AfterScenario("testscheduler")]
		public void AfterScenario()
		{
			RxApp.DeferredScheduler = _previousScheduler;
			RxApp.TaskpoolScheduler = _previousScheduler;
		}

		[BeforeFeature]
		public static void BeforeFeature()
		{
			//TODO: implement logic that has to run before executing each feature
		}

		[AfterFeature]
		public static void AfterFeature()
		{
			//TODO: implement logic that has to run after executing each feature
		}

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			//TODO: implement logic that has to run before the entire test run
		}

		[AfterTestRun]
		public static void AfterTestRun()
		{
			//TODO: implement logic that has to run after the entire test run
		}
	}
}
