using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipGame.Core;
using BattleshipGameTest.Mocks;

namespace BattleshipGameTest.Core
{
    public class GameLogicTest
    {
        [TestClass]
        public class WhenPlacingShips
        {
            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldOnlyAllowPlacingShipsInTheBoardSetupPhase()
            {
                var sut = new Game(); // without 2 players, this game will be in Open state
                sut.Player1 = "myPlayer1";
                sut.PlaceShip("myPlayer1", new Location("A1"), new Location("A1"));
            }

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

            [TestMethod]
            public void ShouldStartTheGameWhenBothShipsArePlaced()
            {
                var sut = new Game("myPlayer1", "myPlayer2");
                sut.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
                sut.PlaceShip("myPlayer2", new Location("B4"), new Location("B6"));
                Assert.AreEqual(GameStatus.InProgress, sut.Status);
                Assert.AreEqual(1, sut.CurrentTurnNumber);
                Assert.AreEqual(1, sut.CurrentTurnPlayer);
            }
        }

        [TestClass]
        public class WhenAttackingGivenANewGame
        {
            private Game _sut;

            [TestInitialize]
            public void Init()
            {
                _sut = MockGame.ThatIsOnTheFirstTurn();
            }

            [TestMethod]
            public void ShouldAllowPlayer1ToAttack()
            {
                _sut.Attack("myPlayer1", new Location("A3"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlayer2ToAttack()
            {
                _sut.Attack("myPlayer2", new Location("A3"));
            }

            [TestMethod]
            public void ShouldAttackTheSpecifiedLocation()
            {
                var result = _sut.Attack("myPlayer1", new Location("A3"));
                Assert.AreEqual("A3", result.ToString());
            }

            [TestMethod]
            public void ShouldMiss()
            {
                var result = _sut.Attack("myPlayer1", new Location("A3"));
                Assert.IsFalse(result.IsHit);
            }

            [TestMethod]
            public void ShouldHit()
            {
                var result = _sut.Attack("myPlayer1", new Location("B5"));
                Assert.IsTrue(result.IsHit);
            }
        }

        [TestClass]
        public class WhenAttackingGivenANearlyOverGame
        {
            private Game _sut;

            [TestInitialize]
            public void Init()
            {
                _sut = MockGame.ThatIsNearlyDone();
            }

            [TestMethod]
            public void ShouldAllowPlayer2ToAttackASpotThatPlayer1Attacked()
            {
                var result = _sut.Attack("myPlayer2", new Location("H1"));
                Assert.AreEqual("H1", result.ToString());
                Assert.IsFalse(result.IsHit);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlayer2ToMissTheSameSpotAgain()
            {
                _sut.Attack("myPlayer2", new Location("H2"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowPlayer2ToHitTheSameSpotAgain()
            {
                _sut.Attack("myPlayer2", new Location("C3"));
            }

            [TestMethod]
            public void ShouldAllowPlayer1ToWin()
            {
                _sut.Attack("myPlayer2", new Location("H4"));
                var result = _sut.Attack("myPlayer1", new Location("B6"));
                Assert.AreEqual("B6", result.ToString());
                Assert.IsTrue(result.IsHit, "B6 should be a hit");
                Assert.AreEqual(GameStatus.Completed, _sut.Status);
                Assert.AreEqual("myPlayer1", _sut.Winner);
            }

            [TestMethod]
            public void ShouldAllowPlayer2ToWin()
            {
                var result = _sut.Attack("myPlayer2", new Location("A3"));
                Assert.AreEqual("A3", result.ToString());
                Assert.IsTrue(result.IsHit, "A3 should be a hit");
                Assert.AreEqual(GameStatus.Completed, _sut.Status);
                Assert.AreEqual("myPlayer2", _sut.Winner);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowAttackingAfterTheGameIsOver()
            {
                _sut.Attack("myPlayer2", new Location("A3"));
                _sut.Attack("myPlayer1", new Location("B6"));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldNotAllowAnUnknownPlayerToAttack()
            {
                _sut.Attack("myPlayerUnknown", new Location("B6"));
            }
        }
    }
}
