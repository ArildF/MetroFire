using System;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace MetroFire.Specs
{
	[TestFixture]
	public class Misc
	{
		[Test]
		public void TestScheduler()
		{
			var scheduler = new TestScheduler();

			int val = 0;

			var observable = Observable.Return(42)
				.Delay(TimeSpan.FromSeconds(10), scheduler);

			observable.Subscribe(v => val = v);

			scheduler.AdvanceBy(TimeSpan.FromSeconds(15).Ticks);

			val.Should().Be(42);
		}
	}
}
