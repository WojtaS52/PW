using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.API;

namespace ModelTests
{
    // model tests by MR & WS
    [TestClass]
    public class IValidatorTest
    {
        [TestMethod]
        public void BallCountValidatorTest()
        {
            const int min = 1;
            const int max = 20;

            InterfaceValidator<int> validator = new ValidatorKulek(min, max);

            Assert.IsTrue(validator.IsValid(max - 1));
            Assert.IsTrue(validator.IsValid(min + 1));

            Assert.IsFalse(validator.IsValid(max + 1));
            Assert.IsFalse(validator.IsValid(min - 1));
        }
    }
}
