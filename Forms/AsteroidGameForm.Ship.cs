/*
 * File:        AsteroidGameForm.Ship.cs
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

/// <summary>Represents the main form for the Asteroids game, handling rendering, input, and game logic.</summary>
/// <remarks>This class manages the game's rendering loop, user input, and overall game state, including the player's ship, asteroids, and bullets.</remarks>
internal partial class AsteroidGameForm
{
	/// <summary>Represents the player's ship.</summary>
	/// <remarks>The ship is represented as a simple triangle that can rotate, thrust forward, and shoot bullets. It has properties for its position, velocity, angle, and invulnerability status. The ship can be damaged by colliding with asteroids, which causes it to lose lives and respawn with temporary invulnerability.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private class Ship
	{
		/// <summary>Ship position X coordinate.</summary>
		/// <remarks>The X coordinate of the ship's position in the game world. The ship's position is updated each frame based on its velocity, and it wraps around the edges of the world. The initial position is typically set to the center of the world when the game starts or when the ship respawns after losing a life.</remarks>
		public float X { get; set; }

		/// <summary>Ship position Y coordinate.</summary>
		/// <remarks>The Y coordinate of the ship's position in the game world. Similar to the X coordinate, it is updated based on the ship's velocity and wraps around the edges of the world. The initial Y position is also set to the center of the world at the start of the game or upon respawn.</remarks>
		public float Y { get; set; }

		/// <summary>Ship velocity X component.</summary>
		/// <remarks>The X component of the ship's velocity. This value is updated when the player applies thrust and is affected by drag. The velocity determines how the ship moves across the screen, and it is clamped to a maximum speed to prevent it from accelerating indefinitely.</remarks>
		public float VelocityX { get; set; }

		/// <summary>Ship velocity Y component.</summary>
		/// <remarks>The Y component of the ship's velocity. Similar to the X component, it is updated based on player input and drag, and it determines the ship's movement in the vertical direction. The velocity is also clamped to a maximum speed for balanced gameplay.</remarks>
		public float VelocityY { get; set; }

		/// <summary>Ship rotation angle in degrees (0 = pointing up).</summary>
		/// <remarks>The angle of the ship in degrees. This value determines the direction the ship is facing and is used to calculate the direction of thrust and bullets. The angle is updated based on player input, allowing the ship to rotate left or right.</remarks>
		public float Angle { get; set; }

		/// <summary>Ship size (radius for collision detection).</summary>
		/// <remarks>The size of the ship, which is used for collision detection with asteroids. This value represents the radius of the ship's collision circle. A larger size makes it easier for the player to collide with asteroids, while a smaller size requires more precise maneuvering to avoid collisions. The visual representation of the ship may be a triangle, but for simplicity in collision detection, it is treated as a circle with this radius.</remarks>
		public const float Size = 1.5f;

		/// <summary>Whether the ship is currently invulnerable (after respawn).</summary>
		/// <remarks>When the ship respawns after losing a life, it becomes temporarily invulnerable to give the player a chance to get back into the game. During this time, the ship cannot be damaged by asteroids. The invulnerability status is typically indicated visually (e.g., flashing or semi-transparent) to let the player know they are safe for a short period.</remarks>
		public bool Invulnerable { get; set; }

		/// <summary>Time remaining for invulnerability in seconds.</summary>
		/// <remarks>This value counts down from a set duration (e.g., 3 seconds) when the ship respawns. Once it reaches zero, the ship's invulnerability status is set to false, and it can be damaged by asteroids again. This timer is updated each frame during the game update logic.</remarks>
		public float InvulnerabilityTime { get; set; }

		/// <summary>Returns a string representation of the ship for debugging purposes.</summary>
		/// <returns>A string representation of the ship.</returns>
		/// <remarks>This property is used by the debugger to display the ship's state in a readable format. It can include information such as position, velocity, angle, and invulnerability status. The string is generated by calling the ToString() method, which can be overridden to provide a custom representation of the ship's properties.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}