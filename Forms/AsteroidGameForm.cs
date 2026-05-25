// This file contains the implementation of the AsteroidGameForm,
// which provides a classic Asteroids arcade game using OpenTK (OpenGL).
using NLog;

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using System.Diagnostics;

namespace Planetoid_DB.Forms;

/// <summary>Displays a classic Asteroids arcade game using OpenTK/OpenGL.</summary>
/// <remarks><para>The form implements the classic Asteroids game where the player controls a triangular ship and must destroy asteroids by shooting them. Larger asteroids break into smaller pieces when hit.</para>
/// <para>Controls: Arrow keys to rotate and thrust, Space to shoot, Enter to start a new game.</para>
/// <para>The game uses OpenGL for rendering and implements simple 2D physics for movement and collision detection.</para></remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AsteroidGameForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used for logging informational messages and debugging output from the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>This property overrides the base class to return the specific <see cref="ToolStripStatusLabel"/> instance used in this form.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	// ---- Game state ----

	/// <summary>Random number generator for game logic.</summary>
	/// <remarks>This instance of <see cref="Random"/> is used for generating random positions and velocities for asteroids, as well as for any other randomization needed in the game logic.</remarks>
	private readonly Random _random = new();

	/// <summary>Whether the OpenGL context is initialized and ready for rendering.</summary>
	/// <remarks>This flag is set to true once the OpenGL context has been successfully created and initialized. It is used to prevent rendering attempts before the context is ready, which could cause errors.</remarks>
	private bool _glReady;

	/// <summary>The embedded OpenTK GLControl that provides the OpenGL rendering surface.</summary>
	/// <remarks>This control hosts the OpenGL context and is used for all rendering operations in the game.</remarks>
	private GLControl _glControl = null!;

	/// <summary>Timer for game updates and rendering.</summary>
	/// <remarks>This timer ticks at a regular interval (approximately 60 times per second) to update the game logic and redraw the scene. The <see cref="GameTimer_Tick"/> event handler is called on each tick to perform these operations.</remarks>
	private readonly System.Windows.Forms.Timer _gameTimer = new();

	/// <summary>Current game state.</summary>
	/// <remarks>The game state is represented by the <see cref="GameState"/> enum, which can be Ready, Playing, or GameOver. The state determines what is rendered on the screen and how input is handled.</remarks>
	private GameState _gameState = GameState.Ready;

	/// <summary>Player ship.</summary>
	/// <remarks>The <see cref="Ship"/> class represents the player's ship, including its position, velocity, angle, and invulnerability status. The ship is controlled by the player and can rotate, thrust forward, and shoot bullets.</remarks>
	private Ship _ship = null!;

	/// <summary>List of active asteroids.</summary>
	/// <remarks>The <see cref="Asteroid"/> class represents an asteroid in the game, including its position, velocity, angle, rotation speed, and size. The list of asteroids is updated each frame to move them and check for collisions.</remarks>
	private readonly List<Asteroid> _asteroids = [];

	/// <summary>List of active bullets.</summary>
	/// <remarks>The <see cref="Bullet"/> class represents a bullet fired by the player's ship, including its position, velocity, and remaining lifetime. The list of bullets is updated each frame to move them, check for collisions, and remove them when they expire.</remarks>
	private readonly List<Bullet> _bullets = [];

	/// <summary>Player's current score.</summary>		  
	/// <remarks>The score is incremented when the player destroys asteroids. It is displayed in the status bar and can be used to track high scores.</remarks>
	private int _score;

	/// <summary>Player's remaining lives.</summary>
	/// <remarks>The player starts with a certain number of lives (3 by default). A life is lost when the ship collides with an asteroid. When all lives are lost, the game is over. The number of lives is displayed in the status bar.</remarks>
	private int _lives = 3;

	/// <summary>Time since last bullet was fired (for rate limiting).</summary>
	/// <remarks>This variable tracks the time elapsed since the player last fired a bullet. It is used to enforce a cooldown period between shots, preventing the player from firing too rapidly. The cooldown duration is defined by the <see cref="ShootCooldown"/> constant.</remarks>
	private double _timeSinceLastShot;

	/// <summary>Set of currently pressed keys.</summary>
	/// <remarks>This set keeps track of which keys are currently pressed down. It is updated in the key down and key up event handlers. The game logic checks this set to determine which actions to perform (e.g., rotate, thrust, shoot) based on the player's input.</remarks>
	private readonly HashSet<Keys> _pressedKeys = [];

	// ---- Game constants ----

	/// <summary>World width in game units.</summary>
	/// <remarks>The game world is defined as a 2D area with a certain width and height. The ship, asteroids, and bullets all exist within this world. The world wraps around at the edges, meaning that objects that move off one edge will reappear on the opposite edge. The dimensions of the world can be adjusted by changing these constants, which will affect the scale of the game and how much space there is for movement.</remarks>
	private const float WorldWidth = 100f;

	/// <summary>World height in game units.</summary>
	/// <remarks>See <see cref="WorldWidth"/> for details on the game world dimensions and behavior.</remarks>
	private const float WorldHeight = 100f;

	/// <summary>Ship rotation speed in degrees per frame.</summary>
	/// <remarks>This constant defines how quickly the player's ship rotates when the left or right arrow keys are pressed. A higher value will make the ship rotate faster, allowing for quicker turns, while a lower value will make it rotate more slowly, requiring more precise timing from the player.</remarks>
	private const float ShipRotationSpeed = 5f;

	/// <summary>Ship thrust acceleration.</summary>
	/// <remarks>This constant defines the acceleration applied to the ship when the up arrow key is pressed. It determines how quickly the ship speeds up in the direction it is currently facing. A higher value will make the ship accelerate faster, while a lower value will result in slower acceleration, affecting the overall feel and responsiveness of the ship's movement.</remarks>
	private const float ShipThrust = 0.15f;

	/// <summary>Maximum ship speed.</summary>
	/// <remarks>This constant defines the maximum speed the ship can reach. It is used to clamp the ship's velocity, preventing it from accelerating indefinitely. A lower value will make the ship easier to control but may reduce the sense of speed, while a higher value will allow for faster movement but may make the ship more difficult to handle, especially for new players.</remarks>
	private const float ShipMaxSpeed = 5f;

	/// <summary>Ship drag coefficient.</summary>
	/// <remarks>This constant defines the drag applied to the ship's velocity each frame. It simulates friction in space, causing the ship to gradually slow down when not thrusting. A value less than 1 will cause the ship to lose speed over time, while a value of 1 would mean no drag and the ship would maintain its velocity indefinitely. Adjusting this value can affect the overall feel of the ship's movement and how much control the player has over it.</remarks>
	private const float ShipDrag = 0.98f;

	/// <summary>Maximum asteroid speed multiplier (lower = slower asteroids).</summary>
	/// <remarks><para>Asteroid speed is randomized up to this multiplier, and smaller asteroids are faster.</para>
	/// <para>Large asteroids are slower, while smaller asteroids are faster, creating a dynamic challenge for the player.</para>
	/// <para>Adjusting this multiplier can make the game easier or harder by controlling how fast the asteroids move across the screen.</para>
	/// <para>Smaller asteroids will have a speed multiplier that is a fraction of this value, making them more challenging to avoid and shoot.</para>
	/// <para>A value of 1.0f would mean asteroids can move up to the same speed as the ship, while a value of 2.0f allows them to be significantly faster, increasing the difficulty.</para>
	/// </remarks>
	private const float AsteroidSpeedMultiplier = 1.5f;

	/// <summary>Bullet speed.</summary>	 
	/// <remarks>This constant defines the speed at which bullets travel when fired. A higher value will make bullets move faster, allowing the player to hit targets more quickly, while a lower value will make bullets slower, requiring more precise aiming and timing.</remarks>
	private const float BulletSpeed = 8f;

	/// <summary>Bullet lifetime in seconds.</summary>
	/// <remarks>This constant defines how long a bullet remains active after being fired. Once the bullet's lifetime expires, it is removed from the game. A shorter lifetime will make bullets disappear quickly, requiring the player to be more accurate and timely with their shots, while a longer lifetime allows bullets to travel further and potentially hit targets that are farther away, making the game easier.</remarks>
	private const float BulletLifetime = 1.5f;

	/// <summary>Minimum time between shots in seconds.</summary>
	/// <remarks>This constant defines the cooldown period between firing bullets. It prevents the player from firing bullets too rapidly, which could make the game too easy and reduce the challenge. A shorter cooldown allows for more rapid firing, while a longer cooldown requires the player to be more strategic with their shots and adds an element of timing to the gameplay.</remarks>
	private const double ShootCooldown = 0.25;

	/// <summary>Initial number of asteroids.</summary>
	/// <remarks>This constant defines how many asteroids are spawned at the start of the game. A higher number will create a more chaotic and challenging initial scenario, while a lower number will make it easier for the player to survive the early stages of the game. The initial asteroids are typically larger and slower, providing an opportunity for the player to get accustomed to the controls before facing smaller, faster asteroids as they progress.</remarks>
	private const int InitialAsteroidCount = 4;

	#region Nested Types

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

	/// <summary>Represents the player's ship.</summary>
	/// <remarks>The ship is represented as a simple triangle that can rotate, thrust forward, and shoot bullets. It has properties for its position, velocity, angle, and invulnerability status. The ship can be damaged by colliding with asteroids, which causes it to lose lives and respawn with temporary invulnerability.</remarks>
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
	}

	/// <summary>Represents an asteroid.</summary>
	/// <remarks>The asteroid is represented as a circle with a certain radius based on its size. It has properties for its position, velocity, angle, rotation speed, and size. Asteroids move across the screen and can collide with the ship and bullets. When hit by a bullet, larger asteroids break into smaller pieces, while small asteroids are destroyed completely.</remarks>
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
	}

	/// <summary>Represents a bullet.</summary>
	/// <remarks>The bullet is represented as a small point that moves in a straight line from the ship's position at the time of firing. It has properties for its position, velocity, and remaining lifetime. Bullets are removed from the game when they exceed their lifetime or when they collide with an asteroid.</remarks>
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
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="AsteroidGameForm"/> class.</summary>
	/// <remarks>The OpenGL context is created programmatically in <see cref="AsteroidGameForm_Load"/> after the designer components have been initialized.</remarks>
	public AsteroidGameForm()
	{
		// The OpenGL control is created and initialized in the Load event handler to ensure that the OpenGL context is properly set up before any rendering occurs. This approach allows for better error handling and ensures that the GLControl is fully ready before we attempt to use it for rendering.
		InitializeComponent();
		_gameTimer.Interval = 16; // ~60 FPS
		_gameTimer.Tick += GameTimer_Tick;
		logger.Info(message: "AsteroidGameForm initialized.");
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Creates and configures the embedded <see cref="GLControl"/> and adds it to the GL panel.</summary>
	/// <remarks>The control is created with an OpenGL compatibility-profile context so that the immediate-mode GL functions (glBegin/glEnd) used for rendering are available.</remarks>
	private void CreateGlControl()
	{
		// Create GLControl with compatibility profile for immediate mode rendering
		GLControlSettings settings = new()
		{
			// Request an OpenGL 2.1 context for compatibility mode, which allows us to use the fixed-function pipeline and immediate mode rendering (glBegin/glEnd) for simplicity in this classic game implementation.
			API = ContextAPI.OpenGL,
			// Compatibility profile is required to use the fixed-function pipeline and immediate mode rendering, which is simpler for this classic game implementation. Core profile would not allow these deprecated features.
			Profile = ContextProfile.Any,
			// Requesting OpenGL 2.1 ensures we have access to the necessary features for rendering while maintaining compatibility with older hardware. This version is sufficient for the simple 2D graphics used in the Asteroids game.
			APIVersion = new Version(major: 2, minor: 1),
		};
		// Create the GLControl with the specified settings and add it to the panel. The control is set to fill the panel and has accessibility properties defined for better usability.
		_glControl = new GLControl(glControlSettings: settings)
		{
			Dock = DockStyle.Fill,
			AccessibleDescription = "OpenGL rendering surface for the Asteroid game",
			AccessibleName = "Asteroid game",
			AccessibleRole = AccessibleRole.Client,
		};
		// Subscribe to GLControl events for rendering and input handling
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.Enter += Control_Enter;
		_glControl.Leave += Control_Leave;
		_glControl.MouseEnter += Control_Enter;
		_glControl.MouseLeave += Control_Leave;
		_glControl.KeyDown += GlControl_KeyDown;
		_glControl.KeyUp += GlControl_KeyUp;
		// Add the GLControl to the panel
		panelGl.Controls.Add(value: _glControl);
	}

	/// <summary>Initializes a new game with default values.</summary>
	/// <remarks>This method sets up the initial state of the game, including the player's ship, asteroids, bullets, score, and lives. It also spawns the initial set of asteroids and transitions the game state to Playing. This method is called when the player starts a new game from the Ready or GameOver states.</remarks>
	private void InitializeGame()
	{
		// Initialize ship in the center of the world with no velocity and temporary invulnerability
		_ship = new Ship
		{
			X = WorldWidth / 2f,
			Y = WorldHeight / 2f,
			Angle = 0f,
			Invulnerable = true,
			InvulnerabilityTime = 3f
		};
		// Clear existing asteroids and bullets, reset score and lives
		_asteroids.Clear();
		_bullets.Clear();
		_score = 0;
		_lives = 3;
		_timeSinceLastShot = 0;
		// Spawn initial asteroids
		for (int i = 0; i < InitialAsteroidCount; i++)
		{
			// Spawn large asteroids at random positions, ensuring they don't spawn too close to the ship
			SpawnAsteroid(size: 0, x: null, y: null);
		}
		// Transition to playing state
		_gameState = GameState.Playing;
		// Update status label to show initial score and lives
		UpdateStatusLabel();
	}

	/// <summary>Spawns a new asteroid at a random position or at the specified position.</summary>
	/// <param name="size">Asteroid size (0 = large, 1 = medium, 2 = small).</param>
	/// <param name="x">X position (null for random).</param>
	/// <param name="y">Y position (null for random).</param>
	/// <remarks><para>This method creates a new asteroid with the specified size and either a random position or a given position. The asteroid's velocity and rotation speed are randomized based on its size, with smaller asteroids generally moving faster. If no position is provided, the method ensures that the asteroid does not spawn too close to the player's ship to prevent immediate collisions at the start of the game.</para></remarks>
	private void SpawnAsteroid(int size, float? x, float? y)
	{
		// Create a new asteroid with randomized velocity and rotation speed based on its size. The position is either random or specified by the parameters.
		Asteroid asteroid = new()
		{
			// Set the asteroid's size, which determines its radius and score value. The size is passed as a parameter to this method.
			Size = size,
			// If x or y is null, generate a random position for the asteroid within the world bounds. Otherwise, use the provided coordinates.
			X = x ?? ((float)_random.NextDouble() * WorldWidth),
			// The Y coordinate is similarly randomized or set based on the parameter. This allows for flexible spawning of asteroids, either at specific locations (e.g., when breaking larger asteroids) or randomly across the screen.
			Y = y ?? ((float)_random.NextDouble() * WorldHeight),
			// Velocity is randomized with a speed multiplier that decreases with size, making smaller asteroids faster. The velocity is also randomized in both X and Y directions to create varied movement patterns.
			VelocityX = ((float)_random.NextDouble() - 0.5f) * 2f * (AsteroidSpeedMultiplier - (size * 0.5f)),
			// The Y velocity is similarly randomized, allowing for movement in any direction. The speed multiplier ensures that smaller asteroids can move faster than larger ones, increasing the challenge as the player progresses.
			VelocityY = ((float)_random.NextDouble() - 0.5f) * 2f * (AsteroidSpeedMultiplier - (size * 0.5f)),
			// The rotation angle is randomized to give each asteroid a unique orientation, and the rotation speed is also randomized to create dynamic movement. The rotation speed can be positive or negative, allowing for both clockwise and counterclockwise rotation.
			Angle = (float)_random.NextDouble() * 360f,
			// Rotation speed is randomized to add variety to the asteroids' movement. Smaller asteroids may have faster rotation speeds, making them more visually dynamic and harder to hit.
			RotationSpeed = ((float)_random.NextDouble() - 0.5f) * 4f
		};
		// Ensure asteroid doesn't spawn on top of the ship at game start
		if (!x.HasValue && !y.HasValue)
		{
			// Calculate the distance from the ship to the asteroid's spawn position. If it's too close, respawn the asteroid to ensure a safe starting area for the player.
			float dx = asteroid.X - _ship.X;
			float dy = asteroid.Y - _ship.Y;
			// If the distance squared is less than 100 (10 units), respawn the asteroid to avoid immediate collision with the ship. This check ensures that the player has a reasonable amount of space to maneuver at the start of the game.
			if ((dx * dx) + (dy * dy) < 100f)
			{
				// Respawn if too close to ship
				SpawnAsteroid(size: size, x: null, y: null);
				return;
			}
		}
		// Add the new asteroid to the list of active asteroids in the game.
		_asteroids.Add(item: asteroid);
	}

	/// <summary>Updates game logic.</summary>
	/// <param name="deltaTime">Time since last update in seconds.</param>
	/// <remarks>This method is called on each tick of the game timer to update the game logic. It handles player input for controlling the ship, updates the positions of the ship, asteroids, and bullets, checks for collisions between bullets and asteroids as well as between the ship and asteroids, and manages the player's score and lives. The method also applies game mechanics such as thrust, rotation, drag, and bullet firing cooldown.</remarks>
	private void UpdateGame(float deltaTime)
	{
		// If the game is not currently in the Playing state, we do not update the game logic. This allows us to pause the game when it is in the Ready or GameOver states, preventing any movement or interactions until the player starts a new game.
		if (_gameState != GameState.Playing)
		{
			return;
		}
		// Update time since last shot for bullet firing cooldown
		_timeSinceLastShot += deltaTime;
		// Check if the ship is currently invulnerable and update the invulnerability timer. If the timer reaches zero, the ship's invulnerability status is set to false, allowing it to be damaged by asteroids again.
		// Update ship invulnerability
		if (_ship.Invulnerable)
		{
			// Decrease the invulnerability timer by the elapsed time since the last update. This allows the ship to remain invulnerable for a set duration after respawning, giving the player a chance to get back into the game without being immediately hit by asteroids.
			_ship.InvulnerabilityTime -= deltaTime;
			if (_ship.InvulnerabilityTime <= 0f)
			{
				_ship.Invulnerable = false;
			}
		}
		// Handle input
		if (_pressedKeys.Contains(item: Keys.Left))
		{
			_ship.Angle -= ShipRotationSpeed;
		}
		if (_pressedKeys.Contains(item: Keys.Right))
		{
			_ship.Angle += ShipRotationSpeed;
		}
		if (_pressedKeys.Contains(item: Keys.Up))
		{
			// Apply thrust
			float angleRad = _ship.Angle * MathF.PI / 180f;
			_ship.VelocityX += MathF.Sin(x: angleRad) * ShipThrust;
			_ship.VelocityY -= MathF.Cos(x: angleRad) * ShipThrust;
			// Clamp velocity
			float speed = MathF.Sqrt(x: (_ship.VelocityX * _ship.VelocityX) + (_ship.VelocityY * _ship.VelocityY));
			if (speed > ShipMaxSpeed)
			{
				_ship.VelocityX = _ship.VelocityX / speed * ShipMaxSpeed;
				_ship.VelocityY = _ship.VelocityY / speed * ShipMaxSpeed;
			}
		}
		if (_pressedKeys.Contains(item: Keys.Space) && _timeSinceLastShot >= ShootCooldown)
		{
			// Fire bullet
			float angleRad = _ship.Angle * MathF.PI / 180f;
			_bullets.Add(item: new Bullet
			{
				X = _ship.X + (MathF.Sin(x: angleRad) * Ship.Size),
				Y = _ship.Y - (MathF.Cos(x: angleRad) * Ship.Size),
				VelocityX = _ship.VelocityX + (MathF.Sin(x: angleRad) * BulletSpeed),
				VelocityY = _ship.VelocityY - (MathF.Cos(x: angleRad) * BulletSpeed)
			});
			_timeSinceLastShot = 0;
		}
		// Apply drag to ship
		_ship.VelocityX *= ShipDrag;
		_ship.VelocityY *= ShipDrag;
		// Update ship position
		_ship.X += _ship.VelocityX;
		_ship.Y += _ship.VelocityY;
		// Wrap ship around screen edges
		_ship.X = (_ship.X + WorldWidth) % WorldWidth;
		_ship.Y = (_ship.Y + WorldHeight) % WorldHeight;
		// Update asteroids
		foreach (Asteroid asteroid in _asteroids)
		{
			asteroid.X += asteroid.VelocityX;
			asteroid.Y += asteroid.VelocityY;
			asteroid.Angle += asteroid.RotationSpeed;
			// Wrap asteroid around screen edges
			asteroid.X = (asteroid.X + WorldWidth) % WorldWidth;
			asteroid.Y = (asteroid.Y + WorldHeight) % WorldHeight;
		}
		// Update bullets
		for (int i = _bullets.Count - 1; i >= 0; i--)
		{
			Bullet bullet = _bullets[index: i];
			bullet.X += bullet.VelocityX;
			bullet.Y += bullet.VelocityY;
			bullet.Lifetime -= deltaTime;
			// Remove bullets that are too old
			if (bullet.Lifetime <= 0f)
			{
				_bullets.RemoveAt(index: i);
			}
			// Wrap bullets around screen edges
			else
			{
				bullet.X = (bullet.X + WorldWidth) % WorldWidth;
				bullet.Y = (bullet.Y + WorldHeight) % WorldHeight;
			}
		}
		// Check bullet-asteroid collisions
		for (int i = _bullets.Count - 1; i >= 0; i--)
		{
			Bullet bullet = _bullets[index: i];
			for (int j = _asteroids.Count - 1; j >= 0; j--)
			{
				Asteroid asteroid = _asteroids[index: j];
				float dx = bullet.X - asteroid.X;
				float dy = bullet.Y - asteroid.Y;
				float distSq = (dx * dx) + (dy * dy);
				// Check if the distance squared between the bullet and asteroid is less than the asteroid's radius squared, indicating a collision. This method avoids the computational cost of calculating a square root for distance comparison.
				if (distSq < asteroid.Radius * asteroid.Radius)
				{
					// Hit!
					_score += asteroid.ScoreValue;
					_bullets.RemoveAt(index: i);
					_asteroids.RemoveAt(index: j);
					// Spawn smaller asteroids
					if (asteroid.Size < 2)
					{
						for (int k = 0; k < 2; k++)
						{
							SpawnAsteroid(size: asteroid.Size + 1, x: asteroid.X, y: asteroid.Y);
						}
					}
					// Update status label to reflect new score and remaining lives
					UpdateStatusLabel();
					break;
				}
			}
		}
		// Check ship-asteroid collisions
		if (!_ship.Invulnerable)
		{
			foreach (Asteroid asteroid in _asteroids)
			{
				float dx = _ship.X - asteroid.X;
				float dy = _ship.Y - asteroid.Y;
				float distSq = (dx * dx) + (dy * dy);
				// Check if the distance squared between the ship and asteroid is less than the sum of their radii squared, indicating a collision. This method avoids the computational cost of calculating a square root for distance comparison.
				if (distSq < (Ship.Size + asteroid.Radius) * (Ship.Size + asteroid.Radius))
				{
					// Ship hit!
					_lives--;
					if (_lives <= 0)
					{
						_gameState = GameState.GameOver;
						UpdateStatusLabel();
					}
					else
					{
						// Respawn ship
						_ship.X = WorldWidth / 2f;
						_ship.Y = WorldHeight / 2f;
						_ship.VelocityX = 0f;
						_ship.VelocityY = 0f;
						_ship.Invulnerable = true;
						_ship.InvulnerabilityTime = 3f;
						UpdateStatusLabel();
					}
					break;
				}
			}
		}
		// Check if all asteroids are destroyed (level complete)
		if (_asteroids.Count == 0 && _gameState == GameState.Playing)
		{
			// Spawn more asteroids for next level
			for (int i = 0; i < InitialAsteroidCount + 1; i++)
			{
				SpawnAsteroid(size: 0, x: null, y: null);
			}
		}
	}

	/// <summary>Sets up the OpenGL viewport and orthographic projection matrix for the current control size.</summary>
	/// <remarks>The projection is an orthographic projection matching the game world coordinates.</remarks>
	private void SetupProjection()
	{
		// Set viewport to match control size
		int w = _glControl.Width;
		int h = Math.Max(val1: _glControl.Height, val2: 1);
		GL.Viewport(x: 0, y: 0, width: w, height: h);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		// Orthographic projection: map world coordinates to screen
		float aspect = (float)w / h;
		float worldAspect = WorldWidth / WorldHeight;
		// Adjust projection to maintain aspect ratio and prevent distortion. If the screen is wider than the world, we add extra width to the left and right. If the screen is taller than the world, we add extra height to the top and bottom. This ensures that the game world is displayed correctly without stretching, regardless of the control's aspect ratio.
		if (aspect > worldAspect)
		{
			// Screen is wider than world
			float extraWidth = WorldWidth * ((aspect / worldAspect) - 1f);
			GL.Ortho(left: -extraWidth / 2f, right: WorldWidth + (extraWidth / 2f), bottom: WorldHeight, top: 0, zNear: -1, zFar: 1);
		}
		else
		{
			// Screen is taller than world
			float extraHeight = WorldHeight * ((worldAspect / aspect) - 1f);
			GL.Ortho(left: 0, right: WorldWidth, bottom: WorldHeight + (extraHeight / 2f), top: -extraHeight / 2f, zNear: -1, zFar: 1);
		}
		// Switch back to modelview matrix for rendering
		GL.MatrixMode(mode: MatrixMode.Modelview);
	}

	/// <summary>Renders the full game scene to the OpenGL surface.</summary>
	/// <remarks>This method is called in the GLControl's Paint event handler to draw the current state of the game. It clears the screen, sets up blending and line smoothing for better visuals, and then renders the ship, asteroids, bullets, and any relevant text based on the current game state (Ready, Playing, GameOver). Finally, it swaps the buffers to display the rendered frame on the screen.</remarks>
	private void RenderScene()
	{
		// If the OpenGL context is not ready, we cannot render anything, so we return early. This check prevents any rendering attempts before the GLControl has been fully initialized and the OpenGL context is available, which could lead to errors or crashes.
		if (!_glReady)
		{
			return;
		}
		// Set up OpenGL state for 2D rendering
		_glControl.MakeCurrent();
		GL.ClearColor(red: 0.0f, green: 0.0f, blue: 0.0f, alpha: 1f);
		GL.Clear(mask: ClearBufferMask.ColorBufferBit);
		GL.Enable(cap: EnableCap.Blend);
		GL.BlendFunc(sfactor: BlendingFactor.SrcAlpha, dfactor: BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(cap: EnableCap.LineSmooth);
		GL.Hint(target: HintTarget.LineSmoothHint, mode: HintMode.Nicest);
		// Set up the projection matrix based on the current control size and aspect ratio. This ensures that the game world is rendered correctly without distortion, regardless of the size of the GLControl.
		SetupProjection();
		GL.LoadIdentity();
		// Render game objects based on current game state
		if (_gameState == GameState.Ready)
		{
			DrawTextCentered(text: "ASTEROIDS", y: (WorldHeight / 2f) - 5f, scale: 0.5f);
			DrawTextCentered(text: "Press ENTER to start", y: (WorldHeight / 2f) + 5f, scale: 0.3f);
		}
		else if (_gameState == GameState.Playing)
		{
			DrawAsteroids();
			DrawBullets();
			DrawShip();
		}
		else if (_gameState == GameState.GameOver)
		{
			DrawAsteroids();
			DrawBullets();
			DrawTextCentered(text: "GAME OVER", y: (WorldHeight / 2f) - 5f, scale: 0.5f);
			DrawTextCentered(text: $"Score: {_score}", y: (WorldHeight / 2f) + 5f, scale: 0.3f);
			DrawTextCentered(text: "Press ENTER to restart", y: (WorldHeight / 2f) + 10f, scale: 0.3f);
		}
		// Swap buffers to display the rendered frame on the screen. This is necessary for double buffering, which helps prevent flickering and provides smoother visuals by rendering to an off-screen buffer and then swapping it with the on-screen buffer.
		_glControl.SwapBuffers();
	}

	/// <summary>Draws the player's ship.</summary>
	/// <remarks>The ship is rendered as a simple triangle that points in the direction of the ship's angle. If the ship is currently invulnerable, it flashes by alternating between being drawn and not drawn based on the invulnerability timer. Additionally, if the player is applying thrust (holding the Up key), a flame is drawn at the back of the ship to indicate thrusting. The ship's position and rotation are applied using OpenGL transformations to ensure it is rendered correctly in the game world.</remarks>
	private void DrawShip()
	{
		// Draw ship as a triangle
		GL.PushMatrix();
		GL.Translate(x: _ship.X, y: _ship.Y, z: 0f);
		GL.Rotate(angle: _ship.Angle, x: 0f, y: 0f, z: 1f);
		// Flash ship if invulnerable
		if (!_ship.Invulnerable || (int)(_ship.InvulnerabilityTime * 10) % 2 == 0)
		{
			GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);
			GL.LineWidth(width: 2f);
			GL.Begin(mode: PrimitiveType.LineLoop);
			GL.Vertex2(x: 0f, y: -Ship.Size);
			GL.Vertex2(x: -Ship.Size * 0.7f, y: Ship.Size);
			GL.Vertex2(x: Ship.Size * 0.7f, y: Ship.Size);
			GL.End();
			// Draw thrust flame if thrusting
			if (_pressedKeys.Contains(item: Keys.Up))
			{
				GL.Color3(red: 1.0f, green: 0.5f, blue: 0.0f);
				GL.Begin(mode: PrimitiveType.LineStrip);
				GL.Vertex2(x: -Ship.Size * 0.3f, y: Ship.Size);
				GL.Vertex2(x: 0f, y: Ship.Size + 1f);
				GL.Vertex2(x: Ship.Size * 0.3f, y: Ship.Size);
				GL.End();
			}
		}
		// Restore matrix state after drawing the ship
		GL.PopMatrix();
		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws all asteroids.</summary>
	/// <remarks>Each asteroid is rendered as an irregular polygon to give it a more natural, rocky appearance. The asteroids are translated and rotated based on their current position and angle in the game world. This method iterates through all active asteroids and draws them using OpenGL line loops.</remarks>
	private void DrawAsteroids()
	{
		// Set color and line width for drawing asteroids
		GL.Color3(red: 0.7f, green: 0.7f, blue: 0.7f);
		GL.LineWidth(width: 2f);
		// Draw each asteroid as an irregular polygon
		foreach (Asteroid asteroid in _asteroids)
		{
			// Save the current matrix state, apply transformations for the asteroid's position and rotation, and then draw the asteroid as an irregular polygon. After drawing, we restore the previous matrix state to ensure that subsequent drawing operations are not affected by the transformations applied to this asteroid.
			GL.PushMatrix();
			GL.Translate(x: asteroid.X, y: asteroid.Y, z: 0f);
			GL.Rotate(angle: asteroid.Angle, x: 0f, y: 0f, z: 1f);
			// Draw asteroid as irregular polygon
			float radius = asteroid.Radius;
			int points = 8;
			GL.Begin(mode: PrimitiveType.LineLoop);
			for (int i = 0; i < points; i++)
			{
				float angle = i * 2f * MathF.PI / points;
				float r = radius * (0.7f + (0.3f * (float)Math.Sin(a: i * 1.5)));
				GL.Vertex2(x: MathF.Cos(x: angle) * r, y: MathF.Sin(x: angle) * r);
			}
			// End drawing the asteroid polygon
			GL.End();
			// Restore matrix state after drawing the asteroid
			GL.PopMatrix();
		}
		// Reset line width after drawing asteroids
		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws all bullets.</summary>
	/// <remarks>Bullets are rendered as small points that move across the screen. This method iterates through all active bullets and draws them using OpenGL points. The color and size of the points are set to make the bullets visible against the background and other game elements.</remarks>
	private void DrawBullets()
	{
		// Set color and point size for drawing bullets
		GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);
		GL.PointSize(size: 3f);
		// Draw each bullet as a point at its current position. This method iterates through the list of active bullets and issues a vertex for each bullet's position, which is rendered as a point on the screen. After drawing all bullets, we reset the point size to the default value.
		GL.Begin(mode: PrimitiveType.Points);
		foreach (Bullet bullet in _bullets)
		{
			// Draw bullet as a point at its current position
			GL.Vertex2(x: bullet.X, y: bullet.Y);
		}
		// End drawing bullets
		GL.End();
		// Reset point size after drawing bullets
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws centered text at the specified position.</summary>
	/// <param name="text">Text to draw.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Text scale.</param>
	/// <remarks>This method calculates the X position needed to center the text based on its length and the specified scale, and then calls the <see cref="DrawText"/> method to render the text at the calculated position. The text is rendered using simple line-based characters for a retro look, and the scale parameter allows for adjusting the size of the text as needed.</remarks>
	private void DrawTextCentered(string text, float y, float scale)
	{
		// Simple text rendering using lines (very basic)
		float x = (WorldWidth / 2f) - (text.Length * 2f * scale);
		DrawText(text: text, x: x, y: y, scale: scale);
	}

	/// <summary>Draws text at the specified position.</summary>
	/// <param name="text">Text to draw.</param>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Text scale.</param>
	/// <remarks>This method renders the specified text at the given X and Y position with the specified scale. Each character in the text is drawn using simple line-based rendering to create a retro, pixelated look. The method iterates through each character in the string and calls the <see cref="DrawChar"/> method to render it, adjusting the X position for each subsequent character based on the scale.</remarks>
	private void DrawText(string text, float x, float y, float scale)
	{
		// Set color and line width for drawing text
		GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);
		GL.LineWidth(width: 2f);
		// Draw each character in the text string at the appropriate position, adjusting for scale. The X position is incremented for each character to ensure they are spaced correctly based on the scale factor. This method relies on the DrawChar method to render each individual character using line-based rendering.
		float currentX = x;
		foreach (char c in text)
		{
			DrawChar(c: c, x: currentX, y: y, scale: scale);
			currentX += 4f * scale;
		}
		// Reset line width after drawing text
		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws a single character at the specified position.</summary>
	/// <param name="c">Character to draw.</param>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Character scale.</param>
	/// <remarks>This method renders a single character at the specified position using simple line-based rendering. The character is scaled according to the provided scale parameter, allowing for adjustable text size. The method uses OpenGL commands to draw lines that form the shape of the character.</remarks>
	private static void DrawChar(char c, float x, float y, float scale)
	{
		// Very simple line-based character rendering
		GL.PushMatrix();
		GL.Translate(x: x, y: y, z: 0f);
		GL.Scale(x: scale, y: scale, z: 1f);
		// Draw character using lines based on the character's shape. Each case in the switch statement corresponds to a specific character and defines the lines needed to render that character. The coordinates for the vertices are chosen to create a recognizable shape for each character, and the lines are drawn using OpenGL's immediate mode rendering.
		GL.Begin(mode: PrimitiveType.Lines);
		switch (c)
		{
			case 'A':
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 1.5f, y: 0f);
				GL.Vertex2(x: 1.5f, y: 0f); GL.Vertex2(x: 3f, y: 3f);
				GL.Vertex2(x: 0.75f, y: 1.8f); GL.Vertex2(x: 2.25f, y: 1.8f);
				break;
			case 'C':
				GL.Vertex2(x: 3f, y: 0.5f); GL.Vertex2(x: 2f, y: 0f);
				GL.Vertex2(x: 2f, y: 0f); GL.Vertex2(x: 1f, y: 0f);
				GL.Vertex2(x: 1f, y: 0f); GL.Vertex2(x: 0f, y: 0.5f);
				GL.Vertex2(x: 0f, y: 0.5f); GL.Vertex2(x: 0f, y: 2.5f);
				GL.Vertex2(x: 0f, y: 2.5f); GL.Vertex2(x: 1f, y: 3f);
				GL.Vertex2(x: 1f, y: 3f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 3f, y: 2.5f);
				break;
			case 'D':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 3f, y: 2.5f);
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 3f, y: 0.5f);
				GL.Vertex2(x: 3f, y: 0.5f); GL.Vertex2(x: 2f, y: 0f);
				GL.Vertex2(x: 2f, y: 0f); GL.Vertex2(x: 0f, y: 0f);
				break;
			case 'E':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 3f, y: 3f);
				GL.Vertex2(x: 0f, y: 1.5f); GL.Vertex2(x: 2f, y: 1.5f);
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 3f, y: 0f);
				break;
			case 'G':
				GL.Vertex2(x: 3f, y: 0.5f); GL.Vertex2(x: 2f, y: 0f);
				GL.Vertex2(x: 2f, y: 0f); GL.Vertex2(x: 1f, y: 0f);
				GL.Vertex2(x: 1f, y: 0f); GL.Vertex2(x: 0f, y: 0.5f);
				GL.Vertex2(x: 0f, y: 0.5f); GL.Vertex2(x: 0f, y: 2.5f);
				GL.Vertex2(x: 0f, y: 2.5f); GL.Vertex2(x: 1f, y: 3f);
				GL.Vertex2(x: 1f, y: 3f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 3f, y: 2.5f);
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 3f, y: 1.5f);
				GL.Vertex2(x: 3f, y: 1.5f); GL.Vertex2(x: 1.5f, y: 1.5f);
				break;
			case 'I':
				GL.Vertex2(x: 1.5f, y: 0f); GL.Vertex2(x: 1.5f, y: 3f);
				GL.Vertex2(x: 0.5f, y: 3f); GL.Vertex2(x: 2.5f, y: 3f);
				GL.Vertex2(x: 0.5f, y: 0f); GL.Vertex2(x: 2.5f, y: 0f);
				break;
			case 'M':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 1.5f, y: 1.5f);
				GL.Vertex2(x: 1.5f, y: 1.5f); GL.Vertex2(x: 3f, y: 3f);
				GL.Vertex2(x: 3f, y: 3f); GL.Vertex2(x: 3f, y: 0f);
				break;
			case 'N':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 3f, y: 0f);
				GL.Vertex2(x: 3f, y: 0f); GL.Vertex2(x: 3f, y: 3f);
				break;
			case 'O':
				GL.Vertex2(x: 1f, y: 0f); GL.Vertex2(x: 2f, y: 0f);
				GL.Vertex2(x: 2f, y: 0f); GL.Vertex2(x: 3f, y: 0.5f);
				GL.Vertex2(x: 3f, y: 0.5f); GL.Vertex2(x: 3f, y: 2.5f);
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 1f, y: 3f);
				GL.Vertex2(x: 1f, y: 3f); GL.Vertex2(x: 0f, y: 2.5f);
				GL.Vertex2(x: 0f, y: 2.5f); GL.Vertex2(x: 0f, y: 0.5f);
				GL.Vertex2(x: 0f, y: 0.5f); GL.Vertex2(x: 1f, y: 0f);
				break;
			case 'P':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 3f, y: 2.5f);
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 3f, y: 1.8f);
				GL.Vertex2(x: 3f, y: 1.8f); GL.Vertex2(x: 2f, y: 1.3f);
				GL.Vertex2(x: 2f, y: 1.3f); GL.Vertex2(x: 0f, y: 1.3f);
				break;
			case 'R':
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 3f, y: 2.5f);
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 3f, y: 1.8f);
				GL.Vertex2(x: 3f, y: 1.8f); GL.Vertex2(x: 2f, y: 1.3f);
				GL.Vertex2(x: 2f, y: 1.3f); GL.Vertex2(x: 0f, y: 1.3f);
				GL.Vertex2(x: 2f, y: 1.3f); GL.Vertex2(x: 3f, y: 0f);
				break;
			case 'S':
				GL.Vertex2(x: 3f, y: 2.5f); GL.Vertex2(x: 2f, y: 3f);
				GL.Vertex2(x: 2f, y: 3f); GL.Vertex2(x: 1f, y: 3f);
				GL.Vertex2(x: 1f, y: 3f); GL.Vertex2(x: 0f, y: 2.5f);
				GL.Vertex2(x: 0f, y: 2.5f); GL.Vertex2(x: 0f, y: 2f);
				GL.Vertex2(x: 0f, y: 2f); GL.Vertex2(x: 1f, y: 1.5f);
				GL.Vertex2(x: 1f, y: 1.5f); GL.Vertex2(x: 2f, y: 1.5f);
				GL.Vertex2(x: 2f, y: 1.5f); GL.Vertex2(x: 3f, y: 1f);
				GL.Vertex2(x: 3f, y: 1f); GL.Vertex2(x: 3f, y: 0.5f);
				GL.Vertex2(x: 3f, y: 0.5f); GL.Vertex2(x: 2f, y: 0f);
				GL.Vertex2(x: 2f, y: 0f); GL.Vertex2(x: 1f, y: 0f);
				GL.Vertex2(x: 1f, y: 0f); GL.Vertex2(x: 0f, y: 0.5f);
				break;
			case 'T':
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 3f, y: 3f);
				GL.Vertex2(x: 1.5f, y: 3f); GL.Vertex2(x: 1.5f, y: 0f);
				break;
			case 'V':
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 1.5f, y: 0f);
				GL.Vertex2(x: 1.5f, y: 0f); GL.Vertex2(x: 3f, y: 3f);
				break;
			case 'W':
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 0.75f, y: 0f);
				GL.Vertex2(x: 0.75f, y: 0f); GL.Vertex2(x: 1.5f, y: 1.5f);
				GL.Vertex2(x: 1.5f, y: 1.5f); GL.Vertex2(x: 2.25f, y: 0f);
				GL.Vertex2(x: 2.25f, y: 0f); GL.Vertex2(x: 3f, y: 3f);
				break;
			case ':':
				GL.Vertex2(x: 1.5f, y: 1f); GL.Vertex2(x: 1.5f, y: 1f);
				GL.Vertex2(x: 1.5f, y: 2f); GL.Vertex2(x: 1.5f, y: 2f);
				break;
			case ' ':
				// Space - no lines
				break;
			default:
				// Draw a box for unknown characters
				GL.Vertex2(x: 0f, y: 0f); GL.Vertex2(x: 3f, y: 0f);
				GL.Vertex2(x: 3f, y: 0f); GL.Vertex2(x: 3f, y: 3f);
				GL.Vertex2(x: 3f, y: 3f); GL.Vertex2(x: 0f, y: 3f);
				GL.Vertex2(x: 0f, y: 3f); GL.Vertex2(x: 0f, y: 0f);
				break;
		}
		GL.End();
		// Restore matrix state after drawing the character
		GL.PopMatrix();
	}

	/// <summary>Updates the status bar label with the current game state.</summary>
	/// <remarks>This method updates the text of the status label to reflect the current game state. When the game is in the Ready state, it shows instructions for starting the game. During the Playing state, it displays the current score, remaining lives, and number of asteroids. When the game is over, it shows a game over message along with the final score. This provides feedback to the player about their progress and how to interact with the game.</remarks>
	private void UpdateStatusLabel()
	{
		// Update the status label text based on the current game state. This switch expression checks the value of _gameState and sets the label text accordingly. It provides different information to the player depending on whether they are about to start the game, currently playing, or have just finished a game over scenario.
		labelInformation.Text = _gameState switch
		{
			GameState.Ready => "Arrow keys: move, Space: shoot, Enter: start",
			GameState.Playing => $"Score: {_score}  Lives: {_lives}  Asteroids: {_asteroids.Count}",
			GameState.GameOver => $"Game Over - Final Score: {_score}",
			_ => ""
		};
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the <see cref="Form.Load"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Creates the <see cref="GLControl"/>, initializes the OpenGL context, and triggers the first render.</remarks>
	private void AsteroidGameForm_Load(object? sender, EventArgs e)
	{
		// Clear the status bar at the start of the form load
		ClearStatusBar(label: labelInformation);
		// Attempt to create the GLControl and initialize the OpenGL context. If successful, we set the _glReady flag to true, update the status label, invalidate the GLControl to trigger a repaint, and start the game timer. If any exceptions occur during this process (such as issues with creating the OpenGL context), we catch the exception, log an error message, and show an error message to the user indicating that game rendering could not be initialized.
		try
		{
			CreateGlControl();
			_glControl.MakeCurrent();
			_glReady = true;
			UpdateStatusLabel();
			_glControl.Invalidate();
			_gameTimer.Start();
		}
		// Log any exceptions that occur during OpenGL initialization and show an error message to the user. This catch block ensures that if there are any issues with setting up the OpenGL context (such as missing drivers or incompatible hardware), the application will handle it gracefully by logging the error and informing the user, rather than crashing or behaving unpredictably.
		catch (Exception ex)
		{
			logger.Error(message: "AsteroidGameForm: failed to initialize OpenGL context: {0}", args: ex);
			ShowErrorMessage(message: $"Failed to initialize game rendering: {ex.Message}");
		}
	}

	/// <summary>Handles the <see cref="Form.FormClosing"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Stops the game timer when the form is closing.</remarks>
	private void AsteroidGameForm_FormClosing(object? sender, FormClosingEventArgs e) =>
		// Stop the game timer when the form is closing
		_gameTimer.Stop();

	/// <summary>Intercepts arrow and space keys before WinForms navigation handling so they always reach the game logic.</summary>
	/// <param name="msg">The Windows message.</param>
	/// <param name="keyData">The key data.</param>
	/// <returns><see langword="true"/> if the key was handled; otherwise the base implementation result.</returns>
	/// <remarks>This method overrides the default key processing to ensure that arrow keys and the space bar are captured and sent to the game logic, even if the GLControl does not have focus. By adding these keys to the _pressedKeys set, we can track their state in the game logic and respond accordingly (e.g., moving the ship or firing bullets). Returning true indicates that we have handled the key press, preventing it from being processed further by WinForms, which might otherwise cause unintended behavior such as changing focus or scrolling.</remarks>
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		// Intercept arrow keys and space bar to ensure they are captured for game controls, even if the GLControl does not have focus. This allows the player to control the game using the keyboard without needing to click on the GLControl first. By adding these keys to the _pressedKeys set, we can track their state in the game logic and respond accordingly (e.g., moving the ship or firing bullets). Returning true indicates that we have handled the key press, preventing it from being processed further by WinForms, which might otherwise cause unintended behavior such as changing focus or scrolling.
		switch (keyData)
		{
			case Keys.Left:
			case Keys.Right:
			case Keys.Up:
			case Keys.Down:
			case Keys.Space:
				_pressedKeys.Add(item: keyData);
				return true;
		}
		// For all other keys, call the base implementation to allow normal processing. This ensures that keys that are not specifically handled by our game logic will still be processed by the form as usual, allowing for standard behavior such as navigating between controls or triggering other events.
		return base.ProcessCmdKey(ref msg, keyData);
	}

	/// <summary>Handles the <see cref="GlControl_KeyDown"/> event.</summary>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Tracks pressed keys and handles game state changes.</remarks>
	protected override void OnKeyDown(KeyEventArgs e)
	{
		// Call the base implementation to ensure that any default key handling is performed. This is important for maintaining standard behavior for keys that are not specifically handled by our game logic, and it allows the form to process other key events as needed. After calling the base method, we add the pressed key to the _pressedKeys set to track its state in the game logic, and we check if the Enter key was pressed to potentially start or restart the game based on the current game state.
		base.OnKeyDown(e: e);
		_pressedKeys.Add(item: e.KeyCode);
		if (e.KeyCode == Keys.Enter)
		{
			if (_gameState is GameState.Ready or GameState.GameOver)
			{
				InitializeGame();
			}
		}
	}

	/// <summary>Handles the <see cref="GlControl_KeyUp"/> event.</summary>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Tracks released keys.</remarks>
	protected override void OnKeyUp(KeyEventArgs e)
	{
		// Call the base implementation to ensure that any default key handling is performed. This allows the form to process key releases as needed for keys that are not specifically handled by our game logic. After calling the base method, we remove the released key from the _pressedKeys set to update its state in the game logic, ensuring that we accurately track which keys are currently pressed and which have been released.
		base.OnKeyUp(e: e);
		_pressedKeys.Remove(item: e.KeyCode);
	}

	/// <summary>Handles the <see cref="Control.KeyDown"/> event of the GL control.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Key event arguments.</param>
	/// <remarks>Forwards key presses from the GL control to the game logic when the GL control has focus.</remarks>
	private void GlControl_KeyDown(object? sender, KeyEventArgs e)
	{
		// Add the pressed key to the _pressedKeys set to track its state in the game logic. This allows us to respond to key presses such as moving the ship or firing bullets when the GL control has focus. Additionally, if the Enter key is pressed while the game is in the Ready or GameOver state, we call InitializeGame() to start or restart the game. Setting e.Handled to true indicates that we have processed this key event and prevents it from being handled further by other controls or default behavior.
		_pressedKeys.Add(item: e.KeyCode);
		if (e.KeyCode == Keys.Enter && _gameState is GameState.Ready or GameState.GameOver)
		{
			InitializeGame();
		}
		e.Handled = true;
	}

	/// <summary>Handles the <see cref="Control.KeyUp"/> event of the GL control.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Key event arguments.</param>
	/// <remarks>Forwards key releases from the GL control to the game logic when the GL control has focus.</remarks>
	private void GlControl_KeyUp(object? sender, KeyEventArgs e)
	{
		// Remove the released key from the _pressedKeys set to update its state in the game logic. This ensures that we accurately track which keys are currently pressed and which have been released when the GL control has focus. Setting e.Handled to true indicates that we have processed this key event and prevents it from being handled further by other controls or default behavior.
		_pressedKeys.Remove(item: e.KeyCode);
		e.Handled = true;
	}

	#endregion

	#region GLControl event handlers

	/// <summary>Handles the <see cref="Control.Paint"/> event of the GL control to redraw the scene.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Paint event arguments.</param>
	/// <remarks>Triggers rendering of the game scene whenever the GL control needs to be repainted.</remarks>
	private void GlControl_Paint(object? sender, PaintEventArgs e) => RenderScene();

	/// <summary>Handles the <see cref="Control.Resize"/> event of the GL control to update the viewport.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Updates the OpenGL viewport and projection matrix whenever the GL control is resized.</remarks>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		// If the OpenGL context is not ready, we cannot update the viewport or projection, so we return early. This check prevents potential errors that could occur if we try to make OpenGL calls before the context has been properly initialized. If the context is ready, we make it current, call SetupProjection() to update the projection matrix based on the new size of the GL control, and then invalidate the control to trigger a repaint with the updated viewport.
		if (!_glReady)
		{
			return;
		}
		_glControl.MakeCurrent();
		SetupProjection();
		_glControl.Invalidate();
	}

	#endregion

	#region Timer event handlers

	/// <summary>Handles the game timer tick event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Updates game logic and triggers a repaint.</remarks>
	private void GameTimer_Tick(object? sender, EventArgs e)
	{
		// Update game logic with a fixed time step (assuming 60 FPS) and invalidate the GL control to trigger a repaint. This method is called at regular intervals by the game timer, and it advances the game state by calling UpdateGame() with a fixed delta time. After updating the game logic, we call Invalidate() on the GL control to request that it be repainted, which will cause the GlControl_Paint event handler to be invoked and the scene to be rendered with the updated game state.
		UpdateGame(deltaTime: 0.016f); // Assume 60 FPS
		_glControl?.Invalidate();
	}

	#endregion
}