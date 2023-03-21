using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Tests
{
    [TestClass()]
    public class LogicClassTests
    {
        [TestMethod()]
        public void MessegeTransmitterTest()
        {
            LogicClass logic = new LogicClass();
            for(int i = 0; i< 5; i++)
            {
                logic.MessegeTransmitter();
            }
            Assert.AreEqual(5, logic.GetCounter());
        }
    }
}