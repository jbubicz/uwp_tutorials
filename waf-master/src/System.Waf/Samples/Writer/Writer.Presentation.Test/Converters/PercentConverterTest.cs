﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;
using System.Windows;
using Waf.Writer.Presentation.Converters;

namespace Test.Writer.Presentation.Converters
{
    [TestClass]
    public class PercentConverterTest
    {
        private CultureInfo currentCulture;


        [TestInitialize]
        public void TestInitialize()
        {
            currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        

        [TestMethod]
        public void ConvertTest()
        {
            PercentConverter converter = new PercentConverter();
            Assert.AreEqual("75 %", converter.Convert(0.75, null, null, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            PercentConverter converter = new PercentConverter();
            Assert.AreEqual(0.75, converter.ConvertBack("75 %", null, null, CultureInfo.InvariantCulture));
            Assert.AreEqual(0.75, converter.ConvertBack("75 %", null, null, null));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.ConvertBack("abc", null, null, CultureInfo.InvariantCulture));
        }
    }
}
