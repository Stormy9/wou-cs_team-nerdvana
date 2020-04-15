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
    class Joey
    {
        [TestMethod]
        public void PetsCreate()
        {
            // Arrange
            PetsController petController = new PetsController();

            // Act
            ViewResult result = petController.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
