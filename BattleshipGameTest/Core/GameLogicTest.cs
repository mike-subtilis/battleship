using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipGame.Core;

namespace BattleshipGameTest.Core
{
    public class GameLogicTest
    {
        [TestClass]
        public class WhenPlacingShips
        {
            [TestMethod]
            public void ShouldAllowPlayer1ToPlaceShips()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
            }

            [TestMethod]
            public void ShouldAllowPlayer2ToPlaceShips()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer2", new Location("A3"), new Location("C3"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlayerUnknownToPlaceShips()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayerUnknown", new Location("A3"), new Location("C3"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlacingShipsTwice()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
                sut.PlaceShip("myPlayer1", new Location("D5"), new Location("D7"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlacingShipsDiagonally()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer1", new Location("A3"), new Location("C5"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlacingShipsThatAreTooBig()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer2", new Location("D2"), new Location("D7"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlacingShipsThatAreTooSmall()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer2", new Location("D2"), new Location("D2"));
            }

            [TestMethod]
            public void ShouldAllowPlacingShipsBackwards()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer2", new Location("D7"), new Location("D5"));
            }
        }
    }
}
