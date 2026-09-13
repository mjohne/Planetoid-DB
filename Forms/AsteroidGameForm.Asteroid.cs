/*
 * File:        AsteroidGameForm.Asteroid.cs
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

/// <summary>Represents the main form for the Asteroid game.</summary>
/// <remarks>This class manages the game's state, including the player's ship, asteroids, and bullets. It handles user input, updates the game logic, and renders the game scene using OpenGL.</remarks>
internal partial class AsteroidGameForm
{
	/// <summary>Represents an asteroid.</summary>
	/// <remarks>The asteroid is represented as a circle with a certain radius based on its size. It has properties for its position, velocity, angle, rotation speed, and size. Asteroids move across the screen and can collide with the ship and bullets. When hit by a bullet, larger asteroids break into smaller pieces, while small asteroids are destroyed completely.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private class Asteroid
	{
		/// <summary>Asteroid position X coordinate.</summary>		  
		/// <remarks>The X coordinate of the asteroid's position on the screen. This value is updated each frame based on the asteroid's velocity and is used for rendering and collision detection.</remarks>
		public float X { get; set; }

		/// <summary>Asteroid position Y coordinate.</summary>
		/// <remarks>The Y coordinate of the asteroid's position on the screen. Similar to the X coordinate, it is updated based on the asteroid's velocity and is used for rendering and collision detection.</remarks>
		public float Y { get; set; }

		/// <summary>Asteroid velocity X component.</summary>
		/// <remarks>The X component of the asteroid's velocity. This value determines how fast the asteroid moves horizontally across the screen. The velocity is randomized when the asteroid is spawned, with smaller asteroids typically having higher velocities than larger ones.</remarks>
		public float VelocityX { get; set; }

		/// <summary>Asteroid velocity Y component.</summary>
		/// <remarks>The Y component of the asteroid's velocity. Similar to the X component, it determines the vertical movement of the asteroid. The velocity is also randomized at spawn and contributes to the overall movement pattern of the asteroid on the screen.</remarks>
		public float VelocityY { get; set; }

		/// <summary>Asteroid rotation angle in degrees.</summary>
		/// <remarks>The angle of the asteroid in degrees. This value is used to rotate the asteroid when rendering, giving it a more dynamic and natural appearance. The angle is updated each frame based on the asteroid's rotation speed.</remarks>
		public float Angle { get; set; }

		/// <summary>Asteroid rotation speed in degrees per frame.</summary>
		/// <remarks>The speed at which the asteroid rotates. A positive value means the asteroid rotates clockwise, while a negative value means it rotates counterclockwise. The rotation speed is randomized when the asteroid is spawned, adding variety to the movement and appearance of asteroids in the game.</remarks>
		public float RotationSpeed { get; set; }

		/// <summary>Asteroid size (0 = large, 1 = medium, 2 = small).</summary>
		/// <remarks>The size of the asteroid, which determines its radius for collision detection and its score value when destroyed. Larger asteroids have a size of 0, medium asteroids have a size of 1, and small asteroids have a size of 2. When a large asteroid is hit by a bullet, it breaks into two medium asteroids, and when a medium asteroid is hit, it breaks into two small asteroids. Small asteroids are destroyed completely when hit.</remarks>
		public int Size { get; set; }

		/// <summary>Gets the collision radius for this asteroid based on its size.</summary>
		/// <remarks>The collision radius is determined by the asteroid's size. Larger asteroids have a larger radius, making them easier to hit, while smaller asteroids have a smaller radius, making them harder to hit.</remarks>
		public float Radius => Size switch
		{
			0 => 3.5f,  // Large
			1 => 2.0f,  // Medium
			_ => 1.0f   // Small
		};

		/// <summary>Gets the score value for destroying this asteroid.</summary>
		/// <remarks>The score value is determined by the asteroid's size. Larger asteroids yield fewer points, while smaller asteroids yield more points, reflecting the increased difficulty in destroying them.</remarks>
		public int ScoreValue => Size switch
		{
			0 => 20,   // Large
			1 => 50,   // Medium
			_ => 100   // Small
		};

		/// <summary>Returns a string representation of the asteroid for debugging purposes.</summary>
		/// <returns>A string representation of the asteroid.</returns>
		/// <remarks>This property is used by the debugger to display the asteroid's state in a readable format. It can include information such as position, velocity, angle, and size. The string is generated by calling the ToString() method, which can be overridden to provide a custom representation of the asteroid's properties.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}