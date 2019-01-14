using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astrophysical_Console.Tests
{
    [TestClass]
    public class CoordinatesTests
    {
        [DataTestMethod]
        [DataRow("1+1+5", 3665, 7328)]
        [DataRow("1+1+9", 3669, 7328)]
        public void RAToString(string expected, int raSeconds, int decSeconds)
        {
            //Arrange
            string actual;
            Coordinates coords = new Coordinates(raSeconds, decSeconds);

            //Act
            actual = coords.RAToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [DataTestMethod]
        [DataRow("2+2+8", 3665, 7328)]
        [DataRow("2+2+10", 3669, 7330)]
        public void DecToString(string expected, int raSeconds, int decSeconds)
        {
            //Arrange
            string actual;
            Coordinates coords = new Coordinates(raSeconds, decSeconds);

            //Act
            actual = coords.DecToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlusOperator()
        {
            //Arrange
            Coordinates c1 = new Coordinates(12, 10, 5, 45, 50, 11);
            Coordinates c2 = new Coordinates(6, 55, 30, 10, 3, 2);
            Coordinates expected = new Coordinates(19, 5, 35, 55, 53, 13);
            
            //Act
            Coordinates actual = c1 + c2;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MinusOperator()
        {
            //Arrange
            Coordinates c1 = new Coordinates(12, 10, 5, 45, 50, 11);
            Coordinates c2 = new Coordinates(6, 55, 30, 10, 3, 2);
            Coordinates expected = new Coordinates(5, 14, 35, 35, 47, 9);

            //Act
            Coordinates actual = c1 - c2;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
