﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Petopia;
using Petopia.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Corrin!
namespace Petopia_tests
{
    [TestClass]
    class Corrin    // did this go up now?
    {
        [TestMethod]
        public void PetsCreate()
        {
            // Arrange
            PetsController petController = new PetsController();

            // Act
            ViewResult result = petController.AddPet() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
