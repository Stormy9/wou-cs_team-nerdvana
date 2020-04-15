using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Petopia;
using Petopia.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Petopia_tests
{
    [TestClass]
    public class Joey
    {
        [TestMethod]
        public void CareProvidersCreate()
        {
            // Arrange
            CareProvidersController careProviderController = new CareProvidersController();

            // Act
            ViewResult result = careProviderController.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
