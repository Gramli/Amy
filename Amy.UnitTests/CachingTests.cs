using Amy.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amy.UnitTests
{
    [TestClass]
    public class CachingTests
    {
        private readonly int wantedCollectionSize = 50;

        [TestMethod]
        public void SmartFixedCollectionAdding()
        {
            var col = new SmartFixedCollection<string>(wantedCollectionSize);

            //create random data
            var randomData = CreateRandomData(5);

            //fill col
            foreach (var item in randomData)
            {
                col.Add(item.ToString());
            }

            //check count
            Assert.IsTrue(col.Count() == this.wantedCollectionSize);
            //check duplicates
            Assert.IsFalse(col.GroupBy(x => x).Where(g => g.Count() > 1).Any());
            //remove item
            var lastItem = col.Last();
            col.Remove(lastItem);
            //get first and add
            var first = col.First();
            col.Add(first);
            //length should not change
            Assert.AreEqual(wantedCollectionSize - 1, col.Count());
        }

        [TestMethod]
        public void SmartFixedCollectionPairAdding()
        {
            var col = new SmartFixedCollectionPair<string, string>(wantedCollectionSize);

            //create random data
            var randomData = CreateRandomData(5);

            //fill col
            foreach (var item in randomData)
            {
                col.Add(item.ToString(), string.Empty);
            }

            //check count
            Assert.IsTrue(col.Count() == this.wantedCollectionSize);
            //check duplicates
            Assert.IsFalse(col.GroupBy(x => x).Where(g => g.Count() > 1).Any());
            //remove item
            var lastItem = col.Last();
            col.Remove(lastItem);
            //get first and add
            var first = col.First();
            col.Add(first);
            //length should not change
            Assert.AreEqual(wantedCollectionSize - 1, col.Count());
        }

        private List<int> CreateRandomData(int iterations)
        {
            var length = wantedCollectionSize * iterations;
            var randomData = new List<int>(length);

            for (var i = 0; i < wantedCollectionSize; i++)
            {
                randomData.Add(i);
            }

            var random = new Random();
            var randomIterationLenght = length - wantedCollectionSize;
            for(var i=0;i< randomIterationLenght; i++)
            {
                var value = random.Next(0, wantedCollectionSize);
                randomData.Add(value);
            }


            return randomData;
        }
    }
}
