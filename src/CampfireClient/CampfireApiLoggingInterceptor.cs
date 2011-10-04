using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Castle.DynamicProxy;

namespace Rogue.MetroFire.CampfireClient
{
	public class CampfireApiLoggingInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			string methodName = invocation.TargetType.FullName + "." + invocation.GetConcreteMethod().Name;

			var arglist = String.Join(", ", invocation.Arguments.Select(arg => arg != null ? arg.ToString() : "<null>"));
			Debug.WriteLine(string.Format("{0}: Entering Campfire API method {1}({2})", Thread.CurrentThread.ManagedThreadId, methodName,
			                              arglist));

			var stopwatch = Stopwatch.StartNew();

			invocation.Proceed();

			Debug.WriteLine("{0}: Leaving Campfire API method {1}({2}). Time taken: {3}", 
			                Thread.CurrentThread.ManagedThreadId, methodName, arglist, stopwatch.Elapsed);
		}
	}
}