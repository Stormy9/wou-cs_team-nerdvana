using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Petopia;
using Petopia.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Tricia!
namespace Petopia_tests
{
    [TestClass]
    public class PetsControllerTest
    {
        [TestMethod]
        public void PetsCreate()
        {
            // Arrange
            PetsController petController = new PetsController();

            // Act
            ViewResult result = petController.CreatePet() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        //-------------------------------------------------------------------------------
        [TestMethod]
        public void PetsEdit()
        {
            // Arrange
            PetsController petController = new PetsController();

            // Act
            ViewResult result = petController.EditPet(5) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        //-------------------------------------------------------------------------------
        [TestMethod]
        public void PetsDetails()
        {
            // Arrange
            PetsController petController = new PetsController();

            // Act
            ViewResult result = petController.Details(5) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        //-------------------------------------------------------------------------------
    }
}
