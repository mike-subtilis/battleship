using BattleshipGame;
using BattleshipGame.Core;
using BattleshipGameTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil;
using NSubstitute;
using System;

namespace BattleshipGameTest
{
    public class GameApiTest
    {
        public class MockBase
        {
            protected GameApi _sut;
            protected Game _game;
            protected Guid _id;
            protected IGameFactory _factory;
            protected IAuthorizer _authorizer;
            protected IRepository<Game> _repo;
            protected ILogger _logger;

            protected void InitWithGame(Game game)
            {
                _game = game;
                _id = new Guid();
                _factory = Substitute.For<IGameFactory>();
                _factory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(_game);

                _repo = Substitute.For<IRepository<Game>>();
                _repo.Add(Arg.Any<Game>()).Returns(_id);
                _repo.Get(Arg.Any<Guid>()).Returns(_game);

                _authorizer = Substitute.For<IAuthorizer>();
                _logger = Substitute.For<ILogger>();

                _sut = new GameApi(_factory, _repo, _authorizer, _logger);
            }
        }

        [TestClass]
        public class WhenCreating : MockBase
        {
            private Guid _createResult;

            [TestInitialize]
            public void Init()
            {
                InitWithGame(MockGame.ThatIsWaitingForBothPlayersToPlaceAShip());
                _createResult = _sut.Create("myPlayer1", "myPlayer2", "powerUser");
            }

            [TestMethod]
            public void ShouldReturnAValidGuid()
            {
                Assert.AreEqual(_id, _createResult);
            }

            [TestMethod]
            public void ShouldAuthorizeForCreatePermission()
            {
                _authorizer.Received().AssertAuthorizedForCreate("powerUser");
            }

            [TestMethod]
            public void ShouldCreateAGameFromTheFactory()
            {
                _factory.Received().Create("myPlayer1", "myPlayer2");
            }

            [TestMethod]
            public void ShouldAddTheNewGameToTheRepo()
            {
                _repo.Received().Add(_game);
            }

            [TestMethod]
            public void ShouldLogSomethingInformative()
            {
                _logger.Received().Info(Arg.Any<string>());
            }
        }

        [TestClass]
        public class WhenReading : MockBase
        {
            private Game _readResult;

            [TestInitialize]
            public void Init()
            {
                InitWithGame(MockGame.ThatIsWaitingForBothPlayersToPlaceAShip());
                _readResult = _sut.Read(_id, "someUser");
            }

            [TestMethod]
            public void ShouldReturnTheGame()
            {
                Assert.AreEqual(_game, _readResult);
            }

            [TestMethod]
            public void ShouldAuthorizeForReadPermission()
            {
                _authorizer.Received().AssertAuthorizedForRead("someUser", _id);
            }

            [TestMethod]
            public void ShouldGetTheNewGameFromTheRepo()
            {
                _repo.Received().Get(_id);
            }
        }

        [TestClass]
        public class WhenPlacingShips : MockBase
        {
            [TestInitialize]
            public void Init()
            {
                InitWithGame(MockGame.ThatIsWaitingForBothPlayersToPlaceAShip());
                _sut.PlaceShip(_id, "myPlayer1", new Location("A3"), new Location("A5"));
            }

            [TestMethod]
            public void ShouldAuthorizeForPlaceShipPermission()
            {
                _authorizer.Received().AssertAuthorizedForPlaceShip("myPlayer1", _id);
            }

            [TestMethod]
            public void ShouldUpdateTheGameInToTheRepo()
            {
                _repo.Received().Update(_id, _game);
            }

            [TestMethod]
            public void ShouldLogSomethingInformative()
            {
                _logger.Received().Info(Arg.Any<string>());
            }
        }

        [TestClass]
        public class WhenAttacking : MockBase
        {
            [TestInitialize]
            public void Init()
            {
                InitWithGame(MockGame.ThatIsOnTheFirstTurn());
                _sut.Attack(_id, "myPlayer1", new Location("A3"));
            }

            [TestMethod]
            public void ShouldAuthorizeForAttackPermission()
            {
                _authorizer.Received().AssertAuthorizedForAttack("myPlayer1", _id);
            }

            [TestMethod]
            public void ShouldUpdateTheGameInToTheRepo()
            {
                _repo.Received().Update(_id, _game);
            }

            [TestMethod]
            public void ShouldLogSomethingInformative()
            {
                _logger.Received().Info(Arg.Any<string>());
            }
        }
    }
}
