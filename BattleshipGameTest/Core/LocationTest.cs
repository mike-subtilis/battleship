using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipGame.Core;
using System.Linq;

namespace BattleshipGameTest.Core
{
    public class LocationTest
    {
        [TestClass]
        public class WhenParsingLocationCodes
        {
            [TestMethod]
            public void ShouldParseRow()
            {
                var rowIndex = Location.GetRowFromCode("C5");
                Assert.AreEqual(4, rowIndex);
            }

            [TestMethod]
            public void ShouldParseCol()
            {
                var colIndex = Location.GetColFromCode("C5");
                Assert.AreEqual(2, colIndex);
            }

            [TestMethod]
            public void ShouldOutputCorrectCode()
            {
                var location = new Location { Row = 4, Col = 0 };
                Assert.AreEqual("A5", location.ToString());
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException))]
            public void ShouldNotAllowRowsLowerThanRange()
            {
                Location.GetRowFromCode("E0");
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException))]
            public void ShouldNotAllowRowsHigherThanRange()
            {
                Location.GetRowFromCode("E9");
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException))]
            public void ShouldNotAllowColsHigherThanRange()
            {
                Location.GetColFromCode("I2");
            }
        }
        
        [TestClass]
        public class WhenMeasuringLength
        {
            [TestMethod]
            public void ShouldMeasureHorizontally()
            {
                var length = Location.GetLength(new Location("A4"), new Location("F4"));
                Assert.AreEqual(6, length);
            }

            [TestMethod]
            public void ShouldMeasureVertically()
            {
                var length = Location.GetLength(new Location("A4"), new Location("A7"));
                Assert.AreEqual(4, length);
            }

            [TestMethod]
            public void ShouldMeasureHorizontallyBackward()
            {
                var length = Location.GetLength(new Location("F4"), new Location("A4"));
                Assert.AreEqual(6, length);
            }

            [TestMethod]
            public void ShouldMeasureVerticallyBackward()
            {
                var length = Location.GetLength(new Location("A7"), new Location("A4"));
                Assert.AreEqual(4, length);
            }

            [TestMethod]
            public void ShouldMeasureASingleSquare()
            {
                var length = Location.GetLength(new Location("E2"), new Location("E2"));
                Assert.AreEqual(1, length);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotMeasureDiagonally()
            {
                Location.GetLength(new Location("E2"), new Location("H5"));
            }
        }

        [TestClass]
        public class WhenGettingLocationsInBetweenTwoLocations
        {
            [TestMethod]
            public void ShouldNotReturnDuplicatesWhenStartAndEndAreTheSame()
            {
                var list = Location.GetLocationsInBetween(new Location("G4"), new Location("G4"));
                var array = list.ToArray();
                Assert.AreEqual(1, array.Length);
                Assert.AreEqual("G4", array[0].ToString());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowDiagonals()
            {
                Location.GetLocationsInBetween(new Location("A1"), new Location("B2"));
            }

            [TestMethod]
            public void ShouldWorkHorizontally()
            {
                var list = Location.GetLocationsInBetween(new Location("D4"), new Location("G4"));
                var locationCodes = list.Select(loc => loc.ToString()).ToList();
                Assert.AreEqual(4, locationCodes.Count);

                Assert.IsTrue(locationCodes.Contains("D4"), "Should contain D4");
                Assert.IsTrue(locationCodes.Contains("E4"), "Should contain E4");
                Assert.IsTrue(locationCodes.Contains("F4"), "Should contain F4");
                Assert.IsTrue(locationCodes.Contains("G4"), "Should contain G4");
            }

            [TestMethod]
            public void ShouldWorkHorizontallyBackward()
            {
                var list = Location.GetLocationsInBetween(new Location("G4"), new Location("D4"));
                var locationCodes = list.Select(loc => loc.ToString()).ToList();
                Assert.AreEqual(4, locationCodes.Count);

                Assert.IsTrue(locationCodes.Contains("D4"), "Should contain D4");
                Assert.IsTrue(locationCodes.Contains("E4"), "Should contain E4");
                Assert.IsTrue(locationCodes.Contains("F4"), "Should contain F4");
                Assert.IsTrue(locationCodes.Contains("G4"), "Should contain G4");
            }

            [TestMethod]
            public void ShouldWorkVertically()
            {
                var list = Location.GetLocationsInBetween(new Location("D4"), new Location("D6"));
                var locationCodes = list.Select(loc => loc.ToString()).ToList();
                Assert.AreEqual(3, locationCodes.Count);

                Assert.IsTrue(locationCodes.Contains("D4"), "Should contain D4");
                Assert.IsTrue(locationCodes.Contains("D5"), "Should contain D5");
                Assert.IsTrue(locationCodes.Contains("D6"), "Should contain D6");
            }

            [TestMethod]
            public void ShouldWorkVerticallyBackward()
            {
                var list = Location.GetLocationsInBetween(new Location("D6"), new Location("D4"));
                var locationCodes = list.Select(loc => loc.ToString()).ToList();
                Assert.AreEqual(3, locationCodes.Count);

                Assert.IsTrue(locationCodes.Contains("D4"), "Should contain D4");
                Assert.IsTrue(locationCodes.Contains("D5"), "Should contain D5");
                Assert.IsTrue(locationCodes.Contains("D6"), "Should contain D6");
            }
        }
    }
}
