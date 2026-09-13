/*
 * File:        AsteroidGameForm.Bullet.cs
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

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents the main form for the Asteroid game, handling game logic, rendering, and user input.</summary>
/// <remarks>This class manages the game's state, including the player's ship, asteroids, and bullets. It handles user input, updates the game logic, and renders the game scene using OpenGL.</remarks>
internal partial class AsteroidGameForm
{
	/// <summary>Represents a bullet.</summary>
	/// <remarks>The bullet is represented as a small point that moves in a straight line from the ship's position at the time of firing. It has properties for its position, velocity, and remaining lifetime. Bullets are removed from the game when they exceed their lifetime or when they collide with an asteroid.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private class Bullet
	{
		/// <summary>Bullet position X coordinate.</summary>
		/// <remarks>The X coordinate of the bullet's position in the game world. This value is updated each frame based on the bullet's velocity, and it wraps around the edges of the world. The initial position of the bullet is typically set to the tip of the ship when fired, and its velocity is determined by the ship's current velocity plus a component in the direction the ship is facing.</remarks>
		public float X { get; set; }

		/// <summary>Bullet position Y coordinate.</summary>
		/// <remarks>The Y coordinate of the bullet's position in the game world. Similar to the X coordinate, it is updated based on the bullet's velocity and wraps around the edges of the world. The initial Y position is also set to the tip of the ship when fired, and its velocity is influenced by the ship's current velocity and direction.</remarks>
		public float Y { get; set; }

		/// <summary>Bullet velocity X component.</summary>
		/// <remarks>The X component of the bullet's velocity. This value is calculated when the bullet is fired, based on the ship's current velocity and the direction the ship is facing. The bullet's velocity determines how fast it moves across the screen, and it is typically faster than the ship's maximum speed to allow for effective shooting.</remarks>
		public float VelocityX { get; set; }

		/// <summary>Bullet velocity Y component.</summary>
		/// <remarks>The Y component of the bullet's velocity. Similar to the X component, it is calculated based on the ship's velocity and direction at the time of firing. The bullet's velocity in both X and Y directions determines its trajectory across the screen, and it is designed to allow the player to hit asteroids effectively while providing a sense of speed and responsiveness.</remarks>
		public float VelocityY { get; set; }

		/// <summary>Bullet lifetime remaining in seconds.</summary>
		/// <remarks>The remaining lifetime of the bullet in seconds. This value decreases over time and determines how long the bullet remains active in the game. When the lifetime reaches zero, the bullet is removed from the game.</remarks>
		public float Lifetime { get; set; } = BulletLifetime;

		/// <summary>Gets a string representation of the bullet for debugging purposes.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}

}