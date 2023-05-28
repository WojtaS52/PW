//mr_ws_MC_śr-12:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dane.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaneTests
{
    [TestClass]
    public class DataApiTest
    {
        [TestMethod]
        public void DataApiCreateTest()
        {
            DaneAbstractApi dataApi = DaneAbstractApi.StworzDaneApi();
            Assert.IsNotNull(dataApi);
        }

        [TestMethod]
        public void PropertiesTest()
        {
            DaneAbstractApi dataApi = DaneAbstractApi.StworzDaneApi();
            Assert.AreNotEqual(dataApi.WysokoscPlanszy, default);
            Assert.AreNotEqual(dataApi.SzerokoscPlanszy, default);
            Assert.AreNotEqual(dataApi.minSrednicaKuli, default);
            Assert.AreNotEqual(dataApi.maxSrednicaKuli, default);
            Assert.AreNotEqual(dataApi.predkosc, default);
        }
    }
}
