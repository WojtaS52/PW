using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dane.API;
using Logika.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModelTests
{
    [TestClass]
    // BIG TESTS INCOMING
    public class SimControlTest
    {
        private readonly int _testWidth;
        private readonly int _testHeight;
        private readonly int _testMinDiameter;
        private readonly int _testMaxDiameter;
        private readonly DaneAbstractApi _dataFixture;

        private LogikaAbstractApi _controller;
        private IEnumerable<InterfejsKuleczka>? _balls;

        public SimControlTest()
        {
            _dataFixture = new DataFixture();
            _controller = LogikaAbstractApi.StworzLogikaApi(_dataFixture);

            _testWidth = _dataFixture.SzerokoscPlanszy;
            _testHeight = _dataFixture.WysokoscPlanszy;
            _testMinDiameter = _dataFixture.minSrednicaKuli;
            _testMaxDiameter = _dataFixture.maxSrednicaKuli;
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_controller);
        }

        [TestMethod]
        public void SimulationTest()
        {
            _controller = LogikaAbstractApi.StworzLogikaApi(_dataFixture);
            Assert.IsNotNull(_controller);

            _balls = _controller.StworzKuleczki(2);
            Assert.AreEqual(_balls.Count(), 2);

            float xPos = _balls.First().Pozycja.X;
            Thread.Sleep(100);
            _balls.First().Poruszanie(1f);
            Assert.AreNotEqual(_balls.First().Pozycja.X, xPos);
        }

        [TestMethod]
        public void RandomBallsCreationTest()
        {
            _controller = LogikaAbstractApi.StworzLogikaApi(_dataFixture);
            Assert.IsNotNull(_controller);

            int ballNumber = 10;

            var balls = _controller.StworzKuleczki(ballNumber);
            int counter = 0;

            foreach (var b in balls)
            {
                Assert.IsNotNull(b);
                Assert.IsTrue(IsBetween(b.Srednica, _testMinDiameter, _testMaxDiameter));
                Assert.IsTrue(IsBetween(b.Srednica/2, _testMinDiameter / 2, _testMaxDiameter / 2));
                Assert.IsTrue(IsBetween(b.Pozycja.X, 0, _testWidth));
                Assert.IsTrue(IsBetween(b.Pozycja.Y, 0, _testHeight));
                counter++;
            }
            Assert.AreEqual(ballNumber, counter);
        }

        private static bool IsBetween(float value, float bottom, float top)
        {
            return value <= top && value >= bottom;
        }

        private class DataFixture : DaneAbstractApi
        {
            public override int WysokoscPlanszy => 100;
            public override int SzerokoscPlanszy => 100;
            public override float predkosc => 50;
            public override int minSrednicaKuli => 20;
            public override int maxSrednicaKuli => 50;
        }
    }
}
