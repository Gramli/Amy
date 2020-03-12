using Amy.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amy.UnitTests
{
    [TestClass]
    public class SmartFixedCollectionTests
    {
        int randomDataSize = 500;
        int colSize = 50;

        [TestMethod]
        public void SmartFixedCollectionAdding()
        {
            var col = new SmartFixedCollection<string>(colSize);

            //create random data
            var randomData = new List<int>(randomDataSize);
            var random = new Random();
            for(int i=0;i<randomDataSize;i++)
            {
                randomData.Add(random.Next(0, 75));
            }

            //fill col
            foreach (var item in randomData)
            {
                col.Add(item.ToString());
            }

            //check count
            Assert.IsTrue(col.Count() == this.colSize);
            //check duplicates
            Assert.IsFalse(col.GroupBy(x => x).Where(g => g.Count() > 1).Any());
            //remove item
            string lastItem = col.Last();
            col.Remove(lastItem);
            //get first and add
            string first = col.First();
            col.Add(first);
            //length should not change
            Assert.AreEqual(colSize - 1, col.Count());
        }
    }
}
