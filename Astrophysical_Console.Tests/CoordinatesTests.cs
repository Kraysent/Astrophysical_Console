using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astrophysical_Console.Tests
{
    [TestClass]
    public class CoordinatesTests
    {
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

        [TestMethod]
        public void RASeconds()
        {
            //Arrange
            Coordinates c = new Coordinates("12+30+02", "45+00+00");
            int expected = 45002;

            //Act
            int actual = c.RASeconds;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecSeconds()
        {
            //Arrange
            Coordinates c = new Coordinates("12+00+00", "47+02+01");
            int expected = 169321;

            //Act
            int actual = c.DecSeconds;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Middle()
        {
            //Arrange
            Coordinates c1 = new Coordinates(12, 30, 00, 45, 00, 00), c2 = new Coordinates(10, 30, 00, 50, 00, 00);

            //Act

            //Assert
            Assert.AreEqual(new Coordinates(11, 30, 00, 47, 30, 00), Coordinates.Middle(c1, c2));
        }
    }
}
