using Microsoft.VisualStudio.TestTools.UnitTesting;
using EVE_Market.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.EveCrestModule.Models.Resources.Market;

namespace EVE_Market.Helpers.Tests
{
    [TestClass()]
    public class EveApiWrapperTests
    {
        private EveApiWrapper eaw = new EveApiWrapper();

        [TestMethod()]
        public void getItemUriTest()
        {
            var result = eaw.getItemUri(34);
            Assert.AreEqual(result, "https://public-crest.eveonline.com/types/34/");
        }
        [TestMethod()]
        public void GetRegionOrdersTestBis()
        {
            var result = eaw.GetRegionOrders("The Forge", "Tritanium").Result;
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Count(), 0);
            Assert.IsInstanceOfType(result, typeof(MarketOrderCollection));
            
        }

        [TestMethod()]
        public void GetRegionOrdersTest()
        {
            var result = eaw.GetRegionOrders("The Forge", 34).Result;
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Count(), 0);
            Assert.IsInstanceOfType(result, typeof(MarketOrderCollection));
            
        }

        [TestMethod()]
        public void GetRegionOrdersTest1()
        {
            var itemList = new List<long>();
            itemList.Add(34);
            itemList.Add(35);
            itemList.Add(36);
            itemList.Add(37);
            itemList.Add(38);
            itemList.Add(39);
            itemList.Add(40);
            itemList.Add(41);
            itemList.Add(42);
            itemList.Add(43);
            itemList.Add(44);
            itemList.Add(45);
            var result = eaw.GetRegionOrders("The Forge", itemList);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 12);
            Assert.IsInstanceOfType(result, typeof(List<MarketOrderCollection>));
        }
    }
}