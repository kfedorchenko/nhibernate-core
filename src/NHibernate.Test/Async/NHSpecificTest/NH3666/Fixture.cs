﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH3666
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			using (var session = this.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var entity1 = new Entity { Id = 1, Property = "Test1" };
				var entity2 = new Entity { Id = 2, Property = "Test2" };
				var entity3 = new Entity { Id = 3, Property = "Test3" };

				session.Save(entity1);
				session.Save(entity2);
				session.Save(entity3);

				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = this.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Delete("from Entity");
				transaction.Commit();
			}
		}

		[Test]
		public async Task CacheableDoesNotThrowExceptionWithNativeSQLQueryAsync()
		{
			using (var session = this.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var result = await (session.CreateSQLQuery("SELECT * FROM Entity WHERE Property = 'Test2'")
				                    .AddEntity(typeof(Entity))
				                    .SetCacheable(true)
				                    .ListAsync<Entity>());

				CollectionAssert.IsNotEmpty(result);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(2, result[0].Id);
			}
		}

		[Test]
		public async Task CacheableDoesNotThrowExceptionWithNamedQueryAsync()
		{
			using (var session = this.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var result = await (session.GetNamedQuery("QueryName")
				                    .SetCacheable(true)
				                    .SetString("prop", "Test2")
				                    .ListAsync<Entity>());

				CollectionAssert.IsNotEmpty(result);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(2, result[0].Id);
			}
		}
	}
}
