﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tests
{
    [TestClass()]
    public class ApiModelTests
    {
        [TestMethod]
        public void StworzApiModelTest()
        {
            ModelAbstractApi log = ModelAbstractApi.StworzModelApi();
            Assert.IsNotNull(log);
        }
    }
}