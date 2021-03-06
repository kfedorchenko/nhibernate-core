﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH687
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH687"; }
		}

		[Test]
		public async Task GetQueryTestAsync()
		{
			//Instantiate and setup associations (all needed to generate the error);
			Bar bar1 = new Bar();
			Bar bar2 = new Bar();
			Foo foo = new Foo();
			bar1.Children.Add(bar2);
			foo.FooBar = new FooBar();
			foo.FooBar.Children.Add(new FooBar());
			foo.FooBar.Children.Add(new FooBar());
			foo.FooBar.Bar = bar1;
			foo.FooBar.Children[0].Bar = bar2;
			foo.FooBar.Children[1].Bar = bar2;

			foo.Children.Add(new Foo());
			foo.Children.Add(new Foo());

			try
			{
				int child1Id, child2Id;
				using (ISession session = Sfi.OpenSession())
				{
					await (session.SaveAsync(foo));

					child1Id = foo.Children[0].Id;
					child2Id = foo.Children[1].Id;
					await (session.FlushAsync());
				}

				using (ISession session = Sfi.OpenSession())
				{
					Foo r = await (session.GetAsync<Foo>(foo.Id));
					Assert.IsNotNull(r);

					Foo child1a = await (session.GetAsync<Foo>(child1Id));
					Assert.IsNotNull(child1a);

					Foo child2a = await (session.GetAsync<Foo>(child2Id));
					Assert.IsNotNull(child2a);
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync(foo));
					await (session.DeleteAsync(foo.FooBar));
					await (session.DeleteAsync(bar1));
					await (session.DeleteAsync(bar2));
					await (session.FlushAsync());
				}
			}
		}
	}
}
