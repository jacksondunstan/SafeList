using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace UnitTests
{
	[TestFixture]
	public class TestSafeList
	{
		private SafeList<string> list;

		[SetUp]
		public void SetUp()
		{
			list = new SafeList<string>() { "zero", "one", "two", "three", "four" };
		}

		private void Iterate<T>(SafeList<T> list, Action<T> callback, params T[] expected)
		{
			var found = new List<T>();
			foreach (var cur in list)
			{
				found.Add(cur);
				callback(cur);
			}
			CollectionAssert.AreEqual(expected, found);
		}

		private void IterateTrigger<T>(
			SafeList<T> list,
			T triggerElement,
			Action trigger,
			params T[] expected
		)
			where T : IEquatable<T>
		{
			Iterate(
				list,
				cur => {
					if (cur.Equals(triggerElement))
					{
						trigger();
					}
				},
				expected
			);
		}

		[Test]
		public void IterateYieldsAllValues()
		{
			Iterate(list, cur => {}, "zero", "one", "two", "three", "four");
		}

		[Test]
		public void IterateTwiceYieldsAllValuesBothTimes()
		{
			Iterate(list, cur => {}, "zero", "one", "two", "three", "four");
			Iterate(list, cur => {}, "zero", "one", "two", "three", "four");
		}

		[Test]
		public void SimultaneousIterationsYieldAllValues()
		{
			var found = new List<string>();
			foreach (var cur in list)
			{
				found.Add(cur);
				Iterate(list, c => {}, "zero", "one", "two", "three", "four");
			}
			CollectionAssert.AreEqual(new []{"zero", "one", "two", "three", "four"}, found);
		}
		
		[TestCase(1, new []{"zero", "one", "two", "three", "four"})]
		[TestCase(2, new []{"zero", "one", "two", "three", "four"})]
		[TestCase(3, new []{"zero", "one", "two", "new", "three", "four"})]
		public void Insert(int insertIndex, string[] expected)
		{
			IterateTrigger(
				list,
				"two",
				() => list.Insert(insertIndex, "new"),
				expected
			);
		}

		[TestCase("one", new[]{"zero", "one", "two", "three", "four"})]
		[TestCase("two", new[]{"zero", "one", "two", "three", "four"})]
		[TestCase("three", new[]{"zero", "one", "two", "four"})]
		public void Remove(string removeElement, string[] expected)
		{
			IterateTrigger(
				list,
				"two",
				() => list.Remove(removeElement),
				expected
			);
		}

		[TestCase(1, new []{"zero", "one", "two", "three", "four"})]
		[TestCase(2, new []{"zero", "one", "two", "three", "four"})]
		[TestCase(3, new []{"zero", "one", "two", "four"})]
		public void RemoveAt(int removeIndex, string[] expected)
		{
			IterateTrigger(
				list,
				"two",
				() => list.RemoveAt(removeIndex),
				expected
			);
		}

		[Test]
		public void ClearEndsIteration()
		{
			IterateTrigger(
				list,
				"two",
				() => list.Clear(),
				"zero", "one", "two"
			);
		}

		[Test]
		public void AddedElementIncludedInIteration()
		{
			IterateTrigger(
				list,
				"two",
				() => list.Add("new"),
				"zero", "one", "two", "three", "four", "new"
			);
		}

		[Test]
		public void RemoveRangeRemovesJustThoseElements()
		{
			IterateTrigger(
				list,
				"two",
				() => list.RemoveRange(1, 3),
				"zero", "one", "two", "four"
			);
		}

		[Test]
		public void RemoveAllRemovesMatchedElements()
		{
			IterateTrigger(
				list,
				"one",
				() => list.RemoveAll(e => e == "two" || e == "three"),
				"zero", "one", "four"
			);
		}
	}
}
