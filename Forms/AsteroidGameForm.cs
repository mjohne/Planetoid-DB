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
	private readonly Random _random = new();

	/// <summary>Whether the OpenGL context is initialized and ready for rendering.</summary>
	private bool _glReady;

	/// <summary>The embedded OpenTK GLControl that provides the OpenGL rendering surface.</summary>
	private GLControl _glControl = null!;

	/// <summary>Timer for game updates and rendering.</summary>
	private readonly System.Windows.Forms.Timer _gameTimer = new();

	/// <summary>Current game state.</summary>
	private GameState _gameState = GameState.Ready;

	/// <summary>Player ship.</summary>
	private Ship _ship = null!;

	/// <summary>List of active asteroids.</summary>
	private readonly List<Asteroid> _asteroids = [];

	/// <summary>List of active bullets.</summary>
	private readonly List<Bullet> _bullets = [];

	/// <summary>Player's current score.</summary>
	private int _score;

	/// <summary>Player's remaining lives.</summary>
	private int _lives = 3;

	/// <summary>Time since last bullet was fired (for rate limiting).</summary>
	private double _timeSinceLastShot;

	/// <summary>Set of currently pressed keys.</summary>
	private readonly HashSet<Keys> _pressedKeys = [];

	// ---- Game constants ----

	/// <summary>World width in game units.</summary>
	private const float WorldWidth = 100f;

	/// <summary>World height in game units.</summary>
	private const float WorldHeight = 100f;

	/// <summary>Ship rotation speed in degrees per frame.</summary>
	private const float ShipRotationSpeed = 5f;

	/// <summary>Ship thrust acceleration.</summary>
	private const float ShipThrust = 0.15f;

	/// <summary>Maximum ship speed.</summary>
	private const float ShipMaxSpeed = 5f;

	/// <summary>Ship drag coefficient.</summary>
	private const float ShipDrag = 0.98f;

	/// <summary>Bullet speed.</summary>
	private const float BulletSpeed = 8f;

	/// <summary>Bullet lifetime in seconds.</summary>
	private const float BulletLifetime = 1.5f;

	/// <summary>Minimum time between shots in seconds.</summary>
	private const double ShootCooldown = 0.25;

	/// <summary>Initial number of asteroids.</summary>
	private const int InitialAsteroidCount = 4;

	#region Nested Types

	/// <summary>Represents the game state.</summary>
	private enum GameState
	{
		/// <summary>Game is ready to start.</summary>
		Ready,
		/// <summary>Game is currently being played.</summary>
		Playing,
		/// <summary>Game is over.</summary>
		GameOver
	}

	/// <summary>Represents the player's ship.</summary>
	private class Ship
	{
		/// <summary>Ship position X coordinate.</summary>
		public float X { get; set; }

		/// <summary>Ship position Y coordinate.</summary>
		public float Y { get; set; }

		/// <summary>Ship velocity X component.</summary>
		public float VelocityX { get; set; }

		/// <summary>Ship velocity Y component.</summary>
		public float VelocityY { get; set; }

		/// <summary>Ship rotation angle in degrees (0 = pointing up).</summary>
		public float Angle { get; set; }

		/// <summary>Ship size (radius for collision detection).</summary>
		public const float Size = 1.5f;

		/// <summary>Whether the ship is currently invulnerable (after respawn).</summary>
		public bool Invulnerable { get; set; }

		/// <summary>Time remaining for invulnerability in seconds.</summary>
		public float InvulnerabilityTime { get; set; }
	}

	/// <summary>Represents an asteroid.</summary>
	private class Asteroid
	{
		/// <summary>Asteroid position X coordinate.</summary>
		public float X { get; set; }

		/// <summary>Asteroid position Y coordinate.</summary>
		public float Y { get; set; }

		/// <summary>Asteroid velocity X component.</summary>
		public float VelocityX { get; set; }

		/// <summary>Asteroid velocity Y component.</summary>
		public float VelocityY { get; set; }

		/// <summary>Asteroid rotation angle in degrees.</summary>
		public float Angle { get; set; }

		/// <summary>Asteroid rotation speed in degrees per frame.</summary>
		public float RotationSpeed { get; set; }

		/// <summary>Asteroid size (0 = large, 1 = medium, 2 = small).</summary>
		public int Size { get; set; }

		/// <summary>Gets the collision radius for this asteroid based on its size.</summary>
		public float Radius => Size switch
		{
			0 => 3.5f,  // Large
			1 => 2.0f,  // Medium
			_ => 1.0f   // Small
		};

		/// <summary>Gets the score value for destroying this asteroid.</summary>
		public int ScoreValue => Size switch
		{
			0 => 20,   // Large
			1 => 50,   // Medium
			_ => 100   // Small
		};
	}

	/// <summary>Represents a bullet.</summary>
	private class Bullet
	{
		/// <summary>Bullet position X coordinate.</summary>
		public float X { get; set; }

		/// <summary>Bullet position Y coordinate.</summary>
		public float Y { get; set; }

		/// <summary>Bullet velocity X component.</summary>
		public float VelocityX { get; set; }

		/// <summary>Bullet velocity Y component.</summary>
		public float VelocityY { get; set; }

		/// <summary>Bullet lifetime remaining in seconds.</summary>
		public float Lifetime { get; set; } = BulletLifetime;
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="AsteroidGameForm"/> class.</summary>
	/// <remarks>The OpenGL context is created programmatically in <see cref="AsteroidGameForm_Load"/> after the designer components have been initialized.</remarks>
	public AsteroidGameForm()
	{
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
		GLControlSettings settings = new()
		{
			API = ContextAPI.OpenGL,
			Profile = ContextProfile.Any,
			APIVersion = new Version(major: 2, minor: 1),
		};
		_glControl = new GLControl(glControlSettings: settings)
		{
			Dock = DockStyle.Fill,
			AccessibleDescription = "OpenGL rendering surface for the Asteroid game",
			AccessibleName = "Asteroid game",
			AccessibleRole = AccessibleRole.Client,
		};
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.Enter += Control_Enter;
		_glControl.Leave += Control_Leave;
		_glControl.MouseEnter += Control_Enter;
		_glControl.MouseLeave += Control_Leave;
		panelGl.Controls.Add(value: _glControl);
	}

	/// <summary>Initializes a new game with default values.</summary>
	private void InitializeGame()
	{
		_ship = new Ship
		{
			X = WorldWidth / 2f,
			Y = WorldHeight / 2f,
			Angle = 0f,
			Invulnerable = true,
			InvulnerabilityTime = 3f
		};
		_asteroids.Clear();
		_bullets.Clear();
		_score = 0;
		_lives = 3;
		_timeSinceLastShot = 0;

		// Spawn initial asteroids
		for (int i = 0; i < InitialAsteroidCount; i++)
		{
			SpawnAsteroid(size: 0, x: null, y: null);
		}

		_gameState = GameState.Playing;
		UpdateStatusLabel();
	}

	/// <summary>Spawns a new asteroid at a random position or at the specified position.</summary>
	/// <param name="size">Asteroid size (0 = large, 1 = medium, 2 = small).</param>
	/// <param name="x">X position (null for random).</param>
	/// <param name="y">Y position (null for random).</param>
	private void SpawnAsteroid(int size, float? x, float? y)
	{
		Asteroid asteroid = new()
		{
			Size = size,
			X = x ?? (float)_random.NextDouble() * WorldWidth,
			Y = y ?? (float)_random.NextDouble() * WorldHeight,
			VelocityX = ((float)_random.NextDouble() - 0.5f) * 2f * (3f - size * 0.5f),
			VelocityY = ((float)_random.NextDouble() - 0.5f) * 2f * (3f - size * 0.5f),
			Angle = (float)_random.NextDouble() * 360f,
			RotationSpeed = ((float)_random.NextDouble() - 0.5f) * 4f
		};

		// Ensure asteroid doesn't spawn on top of the ship at game start
		if (!x.HasValue && !y.HasValue)
		{
			float dx = asteroid.X - _ship.X;
			float dy = asteroid.Y - _ship.Y;
			if (dx * dx + dy * dy < 100f)
			{
				// Respawn if too close to ship
				SpawnAsteroid(size: size, x: null, y: null);
				return;
			}
		}

		_asteroids.Add(item: asteroid);
	}

	/// <summary>Updates game logic.</summary>
	/// <param name="deltaTime">Time since last update in seconds.</param>
	private void UpdateGame(float deltaTime)
	{
		if (_gameState != GameState.Playing)
		{
			return;
		}

		_timeSinceLastShot += deltaTime;

		// Update ship invulnerability
		if (_ship.Invulnerable)
		{
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
			float speed = MathF.Sqrt(_ship.VelocityX * _ship.VelocityX + _ship.VelocityY * _ship.VelocityY);
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
				X = _ship.X + MathF.Sin(x: angleRad) * Ship.Size,
				Y = _ship.Y - MathF.Cos(x: angleRad) * Ship.Size,
				VelocityX = _ship.VelocityX + MathF.Sin(x: angleRad) * BulletSpeed,
				VelocityY = _ship.VelocityY - MathF.Cos(x: angleRad) * BulletSpeed
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
				float distSq = dx * dx + dy * dy;

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
				float distSq = dx * dx + dy * dy;

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
		int w = _glControl.Width;
		int h = Math.Max(val1: _glControl.Height, val2: 1);
		GL.Viewport(x: 0, y: 0, width: w, height: h);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();

		// Orthographic projection: map world coordinates to screen
		float aspect = (float)w / h;
		float worldAspect = WorldWidth / WorldHeight;

		if (aspect > worldAspect)
		{
			// Screen is wider than world
			float extraWidth = WorldWidth * (aspect / worldAspect - 1f);
			GL.Ortho(left: -extraWidth / 2f, right: WorldWidth + extraWidth / 2f, bottom: WorldHeight, top: 0, zNear: -1, zFar: 1);
		}
		else
		{
			// Screen is taller than world
			float extraHeight = WorldHeight * (worldAspect / aspect - 1f);
			GL.Ortho(left: 0, right: WorldWidth, bottom: WorldHeight + extraHeight / 2f, top: -extraHeight / 2f, zNear: -1, zFar: 1);
		}

		GL.MatrixMode(mode: MatrixMode.Modelview);
	}

	/// <summary>Renders the full game scene to the OpenGL surface.</summary>
	private void RenderScene()
	{
		if (!_glReady)
		{
			return;
		}

		_glControl.MakeCurrent();
		GL.ClearColor(red: 0.0f, green: 0.0f, blue: 0.0f, alpha: 1f);
		GL.Clear(mask: ClearBufferMask.ColorBufferBit);
		GL.Enable(cap: EnableCap.Blend);
		GL.BlendFunc(sfactor: BlendingFactor.SrcAlpha, dfactor: BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(cap: EnableCap.LineSmooth);
		GL.Hint(target: HintTarget.LineSmoothHint, mode: HintMode.Nicest);

		SetupProjection();
		GL.LoadIdentity();

		if (_gameState == GameState.Ready)
		{
			DrawTextCentered(text: "ASTEROIDS", y: WorldHeight / 2f - 5f, scale: 0.5f);
			DrawTextCentered(text: "Press ENTER to start", y: WorldHeight / 2f + 5f, scale: 0.3f);
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
			DrawTextCentered(text: "GAME OVER", y: WorldHeight / 2f - 5f, scale: 0.5f);
			DrawTextCentered(text: $"Score: {_score}", y: WorldHeight / 2f + 5f, scale: 0.3f);
			DrawTextCentered(text: "Press ENTER to restart", y: WorldHeight / 2f + 10f, scale: 0.3f);
		}

		_glControl.SwapBuffers();
	}

	/// <summary>Draws the player's ship.</summary>
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

		GL.PopMatrix();
		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws all asteroids.</summary>
	private void DrawAsteroids()
	{
		GL.Color3(red: 0.7f, green: 0.7f, blue: 0.7f);
		GL.LineWidth(width: 2f);

		foreach (Asteroid asteroid in _asteroids)
		{
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
				float r = radius * (0.7f + 0.3f * (float)Math.Sin(a: i * 1.5));
				GL.Vertex2(x: MathF.Cos(x: angle) * r, y: MathF.Sin(x: angle) * r);
			}
			GL.End();

			GL.PopMatrix();
		}

		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws all bullets.</summary>
	private void DrawBullets()
	{
		GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);
		GL.PointSize(size: 3f);
		GL.Begin(mode: PrimitiveType.Points);
		foreach (Bullet bullet in _bullets)
		{
			GL.Vertex2(x: bullet.X, y: bullet.Y);
		}
		GL.End();
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws centered text at the specified position.</summary>
	/// <param name="text">Text to draw.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Text scale.</param>
	private void DrawTextCentered(string text, float y, float scale)
	{
		// Simple text rendering using lines (very basic)
		float x = WorldWidth / 2f - text.Length * 2f * scale;
		DrawText(text: text, x: x, y: y, scale: scale);
	}

	/// <summary>Draws text at the specified position.</summary>
	/// <param name="text">Text to draw.</param>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Text scale.</param>
	private void DrawText(string text, float x, float y, float scale)
	{
		GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);
		GL.LineWidth(width: 2f);

		float currentX = x;
		foreach (char c in text)
		{
			DrawChar(c: c, x: currentX, y: y, scale: scale);
			currentX += 4f * scale;
		}

		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws a single character at the specified position.</summary>
	/// <param name="c">Character to draw.</param>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="scale">Character scale.</param>
	private void DrawChar(char c, float x, float y, float scale)
	{
		// Very simple line-based character rendering
		GL.PushMatrix();
		GL.Translate(x: x, y: y, z: 0f);
		GL.Scale(x: scale, y: scale, z: 1f);

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

		GL.PopMatrix();
	}

	/// <summary>Updates the status bar label with the current game state.</summary>
	private void UpdateStatusLabel()
	{
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
		ClearStatusBar(label: labelInformation);
		try
		{
			CreateGlControl();
			_glControl.MakeCurrent();
			_glReady = true;
			UpdateStatusLabel();
			_glControl.Invalidate();
			_gameTimer.Start();
		}
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
	private void AsteroidGameForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		_gameTimer.Stop();
	}

	/// <summary>Handles the <see cref="Form.KeyDown"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Tracks pressed keys and handles game state changes.</remarks>
	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e: e);

		_pressedKeys.Add(item: e.KeyCode);

		if (e.KeyCode == Keys.Enter)
		{
			if (_gameState == GameState.Ready || _gameState == GameState.GameOver)
			{
				InitializeGame();
			}
		}
	}

	/// <summary>Handles the <see cref="Form.KeyUp"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Tracks released keys.</remarks>
	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e: e);
		_pressedKeys.Remove(item: e.KeyCode);
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
		UpdateGame(deltaTime: 0.016f); // Assume 60 FPS
		_glControl?.Invalidate();
	}

	#endregion
}
