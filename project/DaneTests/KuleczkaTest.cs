using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dane;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaneTests
{
    [TestClass]
    public class KuleczkaTest
    {
        private static readonly int TestXPos = 5;
        private static readonly int TestYPos = 5;
        private static readonly float TestXSpeed = 0.2f;
        private static readonly float TestYSpeed = -0.1f;
        private static readonly int TestDiameter = 2;
        private static readonly Vector2 Position = new(TestXPos, TestYPos);
        private static readonly Vector2 Speed = new(TestXSpeed, TestYSpeed);

        private readonly Kuleczka _testBall;

        public KuleczkaTest()
        {
            _testBall = new Kuleczka(TestDiameter, Speed,Position);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_testBall);

            Assert.AreEqual((int)_testBall.Pozycja.X, TestXPos);
            Assert.AreEqual((int)_testBall.Pozycja.Y, TestYPos);
            Assert.AreEqual(_testBall.Srednica, TestDiameter);
        }

        [TestMethod]
        public void MoveTest()
        {
            float delta = 100f;
            var ball = new Kuleczka(TestDiameter, Vector2.Zero, Position)
            {
                Szybkosc = new Vector2(0f, -2.5f)
            };
            Assert.AreEqual(ball.Szybkosc.X, 0f);

            ball.Poruszanie(delta);
            Assert.AreEqual(ball.Pozycja.X, TestXPos);

            ball.Poruszanie(delta);
            ball.Poruszanie(delta);
            ball.Poruszanie(delta);

            ball.Szybkosc = new Vector2(3f, 5f);
            Assert.AreEqual(ball.Szybkosc, new Vector2(3f, 5f));

            ball.Poruszanie(delta);
            Assert.AreEqual(ball.Pozycja.Y, 0f);

            ball.Poruszanie(delta);
            ball.Poruszanie(delta);
            ball.Poruszanie(delta);

            Assert.AreEqual(ball.Pozycja, new Vector2(17f, 15f));
        }

        [TestMethod]
        // teraz equals test
        public void EqualTest()
        {
            Kuleczka newBall = _testBall;
            Assert.AreEqual(_testBall, newBall);
        }
    }
}
