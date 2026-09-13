/*
 * File:        AsteroidGameForm.GameState.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Displays a classic Asteroids arcade game using OpenTK/OpenGL.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Represents the main form for the Asteroids game, handling game state management, rendering, and user input.</summary>
/// <remarks>This class manages the game's state transitions, including starting a new game, playing the game, and handling game over conditions. It also coordinates the updating and rendering of game objects such as the player's ship, asteroids, and bullets.</remarks>
internal partial class AsteroidGameForm
{
	/// <summary>Represents the game state.</summary>
	/// <remarks>The game can be in one of three states: Ready (waiting for the player to start), Playing (active gameplay), or GameOver (the player has lost all lives). The state affects what is rendered on the screen and how input is handled. For example, in the Ready state, the game may display a title screen and wait for the player to press Enter to start, while in the GameOver state, it may display a game over message and the final score.</remarks>
	private enum GameState
	{
		/// <summary>Game is ready to start.</summary>
		/// <remarks>In this state, the game displays a title screen and waits for the player to press Enter to start a new game. No gameplay occurs in this state, and the ship, asteroids, and bullets are not active.</remarks>
		Ready,
		/// <summary>Game is currently being played.</summary>
		/// <remarks>In this state, the game is active and the player can control the ship, shoot bullets, and interact with asteroids. The game logic updates the positions of all objects, checks for collisions, and handles scoring and lives. The player can lose lives by colliding with asteroids, and the game transitions to GameOver when all lives are lost.</remarks>
		Playing,
		/// <summary>Game is over.</summary>
		/// <remarks>In this state, the player has lost all lives. The game displays a game over message and the final score. The player can press Enter to restart the game, which will transition back to the Ready state and reset all game variables.</remarks>
		GameOver
	}
}