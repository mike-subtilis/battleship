# Battleship

## Code structure
* BattleshipGame: This is the core dll that provides the game classes and logic.
  * Core - all logic and classes required for running a game
  * Implementations - sample implementations of the non-functional dependencies for a repository, authorization, and a factory for 
the Game class to make testing easier.
  * root - the "API" to the game.
* BattleshipGameTest: Unit tests for BattleshipGame
* ConsoleGameRunner: The Console based I/O game runner
* MyUtil: I put logging classes here because they are very generic and don't belong in BattleshipGame

## 3rd party
The following must be refreshed through Nuget before the game will run
* NSubstitute - BattleshipGameTest assesmbly
* NLog - MyUtil assembly & ConsoleGameRunner assembly

## Random Thoughts
* I used a TDD/BDD approach for this solution and started with what the minimalistic API would need to be to expose all 
required functionality to the user.
* I approached this more like a game service that could be wrapped by a web interface.  This led me to add
some simple authorization checks because I feel it's important to think about it for any service.  In a more production-ized
solution where we don't want to communicate the complete game state to any user, the read would need to be broken up 
into reading the ship layout for each player and attack record for each player.
* There is no authentication because that should be handled by the Web or Console or other remote wrapper.  In
the case of the console, we trust the user's input completely.
* I considered hooking up an IOC container, but opted not to given the time alotted and the one line that would use it
* More is public in the core game logic than probably should be.  I didn't have time to do a pass over it to see how much
more I could lock down.  The main thought is that the user should only be able to really interact through the GameApi class
and the game state would be transmitted through that.\
