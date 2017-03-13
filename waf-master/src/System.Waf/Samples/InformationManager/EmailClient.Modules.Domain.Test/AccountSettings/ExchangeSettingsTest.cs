﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [TestClass]
    public class ExchangeSettingsTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var exchangeSettings = new ExchangeSettings();

            AssertHelper.PropertyChangedEvent(exchangeSettings, x => x.ServerPath, () => exchangeSettings.ServerPath = "exchange.example.com");
            Assert.AreEqual("exchange.example.com", exchangeSettings.ServerPath);

            AssertHelper.PropertyChangedEvent(exchangeSettings, x => x.UserName, () => exchangeSettings.UserName = "bill");
            Assert.AreEqual("bill", exchangeSettings.UserName);
        }

        [TestMethod]
        public void CloneTest()
        {
            var exchangeSettings = new ExchangeSettings() { ServerPath = "exchange.example.com", UserName = "bill" };
            var clone = (ExchangeSettings)exchangeSettings.Clone();

            Assert.AreNotEqual(exchangeSettings, clone);
            Assert.AreEqual(exchangeSettings.ServerPath, clone.ServerPath);
            Assert.AreEqual(exchangeSettings.UserName, clone.UserName);
        }

        [TestMethod]
        public void ValidationTest()
        {
            var exchangeSettings = new ExchangeSettings();
            Assert.AreEqual("The Exchange Server field is required.", exchangeSettings.Validate("ServerPath"));

            exchangeSettings.ServerPath = "exchange.example.com";
            Assert.AreEqual("", exchangeSettings.Validate("ServerPath"));
        }
    }
}
