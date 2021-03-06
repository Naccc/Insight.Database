﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Insight.Database;
using System.Data.SqlClient;

#pragma warning disable 0649

namespace Insight.Tests
{
    public class DynamicConnectionTest : BaseDbTest
    {
		class Data
		{
			public int Value;
		}

		[Test]
		public void Test()
		{
			using (SqlTransaction t = _connection.BeginTransaction())
			{
				_connection.ExecuteSql("CREATE PROC InsightTestProc (@Value int = 5) AS SELECT Value=@Value", transaction: t);

				List<Data> result = _connection.Dynamic<Data>().InsightTestProc(transaction: t, value: 5);
			}
		}

		[Test]
		public void TestUnnamedParameter()
		{
			using (SqlTransaction t = _connection.BeginTransaction())
			{
				_connection.ExecuteSql("CREATE PROC InsightTestProc (@Value int = 5) AS SELECT Value=@Value", transaction: t);

				List<Data> result = _connection.Dynamic<Data>().InsightTestProc(5, transaction: t);
			}
		}

		[Test]
		public void TestExecute()
		{
			using (SqlTransaction t = _connection.BeginTransaction())
			{
				_connection.ExecuteSql("CREATE PROC InsightTestProc (@Value int = 5) AS PRINT 'foo'", transaction: t);

				_connection.Dynamic().InsightTestProc(transaction: t, value: 5);
			}
		}

		[Test]
		public void TestObjectParameter()
		{
			using (SqlTransaction t = _connection.BeginTransaction())
			{
				_connection.ExecuteSql("CREATE PROC InsightTestProc (@Value int = 5) AS SELECT Value=@Value", transaction: t);

				Data d = new Data() { Value = 10 };
				IList<Data> list = _connection.Dynamic<Data>().InsightTestProc(d, transaction: t);
				Data result = list.First();

				Assert.AreEqual(d.Value, result.Value);
			}
		}

		[Test]
		public void TestSingleStringParameter()
		{
			using (SqlTransaction t = _connection.BeginTransaction())
			{
				_connection.ExecuteSql("CREATE PROC InsightTestProc (@Value varchar(128)) AS SELECT Value=@Value", transaction: t);

				string value = "foo";
				IList<string> list = _connection.Dynamic<string>().InsightTestProc(value, transaction: t);
				string result = list.First();

				Assert.AreEqual(value, result);
			}
		}
	}
}
