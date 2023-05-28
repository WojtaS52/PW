using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.API;

namespace ModelTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void CreateModelApiTest()
        {
            ModelAbstractApi modelApi = ModelAbstractApi.StworzModelApi();
            Assert.IsNotNull(modelApi);
        }
    }
}
