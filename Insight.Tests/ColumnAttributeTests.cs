﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using NUnit.Framework;

#pragma warning disable 0649

namespace Insight.Tests
{
	/// <summary>
	/// Tests the ColumnAttribute overrides.
	/// </summary>
	class ColumnAttributeTests : BaseDbTest
	{
		class Data
		{
			[Column("Column")]
			public int ColumnA;

			public int Column;

			[Column("OtherProperty")]
			public int Property { get; set; }

			public int OtherProperty { get; set; }
		}

		[Test]
		public void ColumnMappingFillsInField()
		{
			Data data = _connection.QuerySql<Data>("SELECT [Column]=4").First();

			Assert.AreEqual(4, data.ColumnA);
			Assert.AreEqual(0, data.Column);
		}

		[Test]
		public void ColumnMappingFillsInProperty()
		{
			Data data = _connection.QuerySql<Data>("SELECT [OtherProperty]=4").First();

			Assert.AreEqual(4, data.Property);
			Assert.AreEqual(0, data.OtherProperty);
		}
	}
}
