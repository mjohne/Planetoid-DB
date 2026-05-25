using Krypton.Toolkit;

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using System.Diagnostics;

using Timer = System.Windows.Forms.Timer;

namespace Planetoid_DB.Forms;

/// <summary>Displays a small Asteroid game implemented with OpenTK.</summary>
/// <remarks>Use the arrow keys to move the ship and Space to shoot asteroids.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public sealed class AsteroidGameForm : BaseKryptonForm
{
	/// <summary>Represents a single asteroid in the game.</summary>
	/// <remarks>Stores position, radius and falling speed for one asteroid.</remarks>
	private struct Asteroid
	{
		/// <summary>The x-coordinate of the asteroid center.</summary>
		/// <remarks>Measured in OpenGL world space units.</remarks>
		public float X;

		/// <summary>The y-coordinate of the asteroid center.</summary>
		/// <remarks>Measured in OpenGL world space units.</remarks>
		public float Y;

		/// <summary>The radius of the asteroid.</summary>
		/// <remarks>Used for drawing and collision checks.</remarks>
		public float Radius;

		/// <summary>The vertical speed of the asteroid.</summary>
		/// <remarks>Measured in world space units per second.</remarks>
		public float Speed;
	}

	/// <summary>Represents a single projectile fired by the player.</summary>
	/// <remarks>Stores bullet position in world space.</remarks>
	private struct Bullet
	{
		/// <summary>The x-coordinate of the bullet center.</summary>
		/// <remarks>Measured in OpenGL world space units.</remarks>
		public float X;

		/// <summary>The y-coordinate of the bullet center.</summary>
		/// <remarks>Measured in OpenGL world space units.</remarks>
		public float Y;
	}

	/// <summary>Stores the visual components container used by the form.</summary>
	/// <remarks>Disposed together with the form when it closes.</remarks>
	private readonly ToolStripContainer toolStripContainer = new();

	/// <summary>Stores the main host panel for the OpenGL control.</summary>
	/// <remarks>The GL control is created dynamically and added to this panel.</remarks>
	private readonly KryptonPanel panelHost = new();

	/// <summary>Stores the status strip shown at the bottom of the form.</summary>
	/// <remarks>Displays score, lives and controls help text.</remarks>
	private readonly KryptonStatusStrip statusStrip = new();

	/// <summary>Stores the status label used for runtime game information.</summary>
	/// <remarks>Updated every frame with score/lives/game-over details.</remarks>
	private readonly ToolStripStatusLabel labelInformation = new();

	/// <summary>Stores the timer used to drive the game loop.</summary>
	/// <remarks>Ticks approximately every 16 ms (~60 FPS).</remarks>
	private readonly Timer gameTimer = new() { Interval = 16 };

	/// <summary>Stores the random number generator for asteroid spawning.</summary>
	/// <remarks>Used to randomize spawn position, size and speed.</remarks>
	private readonly Random random = new();

	/// <summary>Stores all currently active asteroids.</summary>
	/// <remarks>Mutated by update and collision logic on each frame.</remarks>
	private readonly List<Asteroid> asteroids = [];

	/// <summary>Stores all currently active bullets.</summary>
	/// <remarks>Mutated by update and collision logic on each frame.</remarks>
	private readonly List<Bullet> bullets = [];

	/// <summary>Stores the OpenTK control used for rendering.</summary>
	/// <remarks>Created on form load and disposed when the form closes.</remarks>
	private GLControl glControl = null!;

	/// <summary>Stores whether the OpenGL context is ready for rendering.</summary>
	/// <remarks>Prevents draw calls before the GL control is initialized.</remarks>
	private bool glReady;

	/// <summary>Stores whether the left arrow key is currently held down.</summary>
	/// <remarks>Used to move the ship continuously while pressed.</remarks>
	private bool moveLeft;

	/// <summary>Stores whether the right arrow key is currently held down.</summary>
	/// <remarks>Used to move the ship continuously while pressed.</remarks>
	private bool moveRight;

	/// <summary>Stores whether the fire key is currently held down.</summary>
	/// <remarks>Used for continuous bullet firing with cooldown.</remarks>
	private bool firePressed;

	/// <summary>Stores whether the current round has ended.</summary>
	/// <remarks>Set to true when the player has no lives left.</remarks>
	private bool gameOver;

	/// <summary>Stores the current x-position of the player's ship.</summary>
	/// <remarks>The ship y-position is fixed near the bottom edge.</remarks>
	private float shipX = 320f;

	/// <summary>Stores cooldown before the next bullet can be fired.</summary>
	/// <remarks>Measured in seconds and decremented in the game loop.</remarks>
	private float fireCooldown;

	/// <summary>Stores cooldown until the next asteroid spawn.</summary>
	/// <remarks>Measured in seconds and decremented in the game loop.</remarks>
	private float spawnCooldown = 0.3f;

	/// <summary>Stores current player score.</summary>
	/// <remarks>Increases when asteroids are destroyed.</remarks>
	private int score;

	/// <summary>Stores remaining player lives.</summary>
	/// <remarks>Decreases when asteroids reach the bottom or hit the ship.</remarks>
	private int lives = 3;

	/// <summary>Stores the timestamp of the previous update tick.</summary>
	/// <remarks>Used to calculate frame delta time in seconds.</remarks>
	private DateTime lastUpdateUtc;

	/// <summary>Gets the status label that receives shared accessibility text updates.</summary>
	/// <remarks>Overrides the base implementation for status bar integration.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Initializes a new instance of the <see cref="AsteroidGameForm"/> class.</summary>
	/// <remarks>Configures UI controls and hooks game/input events.</remarks>
	public AsteroidGameForm()
	{
		InitializeComponent();
		gameTimer.Tick += GameTimer_Tick;
		Load += AsteroidGameForm_Load;
		FormClosing += AsteroidGameForm_FormClosing;
		KeyDown += AsteroidGameForm_KeyDown;
		KeyUp += AsteroidGameForm_KeyUp;
	}

	/// <summary>Initializes the static UI structure for the form.</summary>
	/// <remarks>Creates a host panel, status strip and base layout container.</remarks>
	private void InitializeComponent()
	{
		SuspendLayout();
		Text = "Asteroid Game";
		StartPosition = FormStartPosition.CenterScreen;
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		ClientSize = new Size(800, 620);
		ControlBox = false;
		MinimizeBox = false;
		MaximizeBox = false;
		AccessibleDescription = "Small Asteroid game rendered with OpenTK";
		AccessibleName = "Asteroid game form";
		AccessibleRole = AccessibleRole.Dialog;

		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.AccessibleDescription = "Container for the Asteroid game interface";
		toolStripContainer.AccessibleName = "Game container";
		toolStripContainer.ContentPanel.Controls.Add(panelHost);
		toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
		toolStripContainer.Enter += Control_Enter;
		toolStripContainer.Leave += Control_Leave;
		toolStripContainer.MouseEnter += Control_Enter;
		toolStripContainer.MouseLeave += Control_Leave;

		panelHost.Dock = DockStyle.Fill;
		panelHost.PanelBackStyle = PaletteBackStyle.FormMain;
		panelHost.AccessibleDescription = "OpenGL rendering area for the Asteroid game";
		panelHost.AccessibleName = "Game rendering area";
		panelHost.Enter += Control_Enter;
		panelHost.Leave += Control_Leave;
		panelHost.MouseEnter += Control_Enter;
		panelHost.MouseLeave += Control_Leave;

		statusStrip.Dock = DockStyle.None;
		statusStrip.Items.Add(labelInformation);
		statusStrip.AccessibleDescription = "Status information for the Asteroid game";
		statusStrip.AccessibleName = "Game status bar";
		statusStrip.Enter += Control_Enter;
		statusStrip.Leave += Control_Leave;
		statusStrip.MouseEnter += Control_Enter;
		statusStrip.MouseLeave += Control_Leave;

		labelInformation.Text = "Use Arrow keys + Space";
		labelInformation.AccessibleDescription = "Shows score and lives for the Asteroid game";
		labelInformation.AccessibleName = "Game status";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;

		Controls.Add(toolStripContainer);
		ResumeLayout(performLayout: false);
	}

	/// <summary>Handles the form load event and creates the OpenGL control.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Initializes rendering and starts the timer-based game loop.</remarks>
	private void AsteroidGameForm_Load(object? sender, EventArgs e)
	{
		CreateGlControl();
		lastUpdateUtc = DateTime.UtcNow;
		UpdateStatusText();
		gameTimer.Start();
	}

	/// <summary>Handles form closing and stops game resources.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Stops timers and clears dynamic game collections.</remarks>
	private void AsteroidGameForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		gameTimer.Stop();
		asteroids.Clear();
		bullets.Clear();
	}

	/// <summary>Handles key-down events for movement, shooting and restart.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Key event arguments.</param>
	/// <remarks>Tracks held input states and allows round reset on game over.</remarks>
	private void AsteroidGameForm_KeyDown(object? sender, KeyEventArgs e)
	{
		switch (e.KeyCode)
		{
			case Keys.Left:
			case Keys.A:
				moveLeft = true;
				break;
			case Keys.Right:
			case Keys.D:
				moveRight = true;
				break;
			case Keys.Space:
				firePressed = true;
				break;
			case Keys.R when gameOver:
				RestartGame();
				break;
		}
	}

	/// <summary>Handles key-up events for movement and shooting.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Key event arguments.</param>
	/// <remarks>Clears held input states when relevant keys are released.</remarks>
	private void AsteroidGameForm_KeyUp(object? sender, KeyEventArgs e)
	{
		switch (e.KeyCode)
		{
			case Keys.Left:
			case Keys.A:
				moveLeft = false;
				break;
			case Keys.Right:
			case Keys.D:
				moveRight = false;
				break;
			case Keys.Space:
				firePressed = false;
				break;
		}
	}

	/// <summary>Creates and configures the OpenTK rendering control.</summary>
	/// <remarks>Wires paint and resize events used by the game renderer.</remarks>
	private void CreateGlControl()
	{
		GLControlSettings settings = new()
		{
			API = ContextAPI.OpenGL,
			Profile = ContextProfile.Any,
			APIVersion = new Version(major: 2, minor: 1),
		};
		glControl = new GLControl(glControlSettings: settings)
		{
			Dock = DockStyle.Fill,
			BackColor = Color.Black,
			AccessibleDescription = "OpenGL rendering surface of the Asteroid game",
			AccessibleName = "Asteroid game renderer",
			AccessibleRole = AccessibleRole.Client,
		};
		glControl.Paint += GlControl_Paint;
		glControl.Resize += GlControl_Resize;
		glControl.Enter += Control_Enter;
		glControl.Leave += Control_Leave;
		glControl.MouseEnter += Control_Enter;
		glControl.MouseLeave += Control_Leave;
		panelHost.Controls.Add(glControl);
		glControl.MakeCurrent();
		GL.ClearColor(Color.Black);
		glReady = true;
	}

	/// <summary>Handles timer ticks and advances the game simulation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Calculates delta time, updates state and triggers redraw.</remarks>
	private void GameTimer_Tick(object? sender, EventArgs e)
	{
		if (!glReady)
		{
			return;
		}
		DateTime nowUtc = DateTime.UtcNow;
		float deltaSeconds = (float)(nowUtc - lastUpdateUtc).TotalSeconds;
		lastUpdateUtc = nowUtc;
		if (deltaSeconds <= 0f)
		{
			return;
		}
		UpdateGame(deltaSeconds);
		UpdateStatusText();
		glControl.Invalidate();
	}

	/// <summary>Handles OpenGL control resize and updates the viewport.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Ensures rendering coordinates match the current control size.</remarks>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		if (!glReady)
		{
			return;
		}
		glControl.MakeCurrent();
		GL.Viewport(x: 0, y: 0, width: glControl.Width, height: glControl.Height);
	}

	/// <summary>Handles OpenGL paint events and renders the current frame.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Paint event arguments.</param>
	/// <remarks>Renders ship, bullets and asteroids using immediate-mode OpenGL.</remarks>
	private void GlControl_Paint(object? sender, PaintEventArgs e)
	{
		if (!glReady)
		{
			return;
		}
		glControl.MakeCurrent();
		GL.Clear(mask: ClearBufferMask.ColorBufferBit);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		GL.Ortho(left: 0d, right: glControl.Width, bottom: glControl.Height, top: 0d, zNear: -1d, zFar: 1d);
		GL.MatrixMode(mode: MatrixMode.Modelview);
		GL.LoadIdentity();

		DrawShip();
		DrawBullets();
		DrawAsteroids();

		glControl.SwapBuffers();
	}

	/// <summary>Updates all game entities and collision rules.</summary>
	/// <param name="deltaSeconds">Elapsed time since last update.</param>
	/// <remarks>Applies movement, spawning, firing and hit detection logic.</remarks>
	private void UpdateGame(float deltaSeconds)
	{
		if (gameOver)
		{
			return;
		}
		float direction = 0f;
		if (moveLeft)
		{
			direction -= 1f;
		}
		if (moveRight)
		{
			direction += 1f;
		}
		shipX = Math.Clamp(value: shipX + (direction * 360f * deltaSeconds), min: 18f, max: glControl.Width - 18f);

		fireCooldown -= deltaSeconds;
		if (firePressed && fireCooldown <= 0f)
		{
			bullets.Add(item: new Bullet { X = shipX, Y = glControl.Height - 56f });
			fireCooldown = 0.18f;
		}

		for (int idx = bullets.Count - 1; idx >= 0; idx--)
		{
			Bullet bullet = bullets[idx];
			bullet.Y -= 520f * deltaSeconds;
			if (bullet.Y < -10f)
			{
				bullets.RemoveAt(index: idx);
				continue;
			}
			bullets[idx] = bullet;
		}

		spawnCooldown -= deltaSeconds;
		if (spawnCooldown <= 0f)
		{
			SpawnAsteroid();
		}

		for (int idx = asteroids.Count - 1; idx >= 0; idx--)
		{
			Asteroid asteroid = asteroids[idx];
			asteroid.Y += asteroid.Speed * deltaSeconds;
			if (asteroid.Y - asteroid.Radius > glControl.Height)
			{
				asteroids.RemoveAt(index: idx);
				RemoveLife();
				continue;
			}
			asteroids[idx] = asteroid;
		}

		ProcessBulletCollisions();
		ProcessShipCollisions();
	}

	/// <summary>Spawns one asteroid at a randomized position and speed.</summary>
	/// <remarks>Also resets the cooldown for the next spawn.</remarks>
	private void SpawnAsteroid()
	{
		float radius = random.Next(minValue: 12, maxValue: 30);
		float x = random.NextSingle() * Math.Max(val1: 1f, val2: glControl.Width - (2f * radius)) + radius;
		float speed = 90f + (random.NextSingle() * 120f);
		asteroids.Add(item: new Asteroid { X = x, Y = -radius, Radius = radius, Speed = speed });
		spawnCooldown = 0.35f + (random.NextSingle() * 0.35f);
	}

	/// <summary>Processes collisions between bullets and asteroids.</summary>
	/// <remarks>On hit both entities are removed and score is increased.</remarks>
	private void ProcessBulletCollisions()
	{
		for (int asteroidIndex = asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
		{
			Asteroid asteroid = asteroids[asteroidIndex];
			for (int bulletIndex = bullets.Count - 1; bulletIndex >= 0; bulletIndex--)
			{
				Bullet bullet = bullets[bulletIndex];
				float dx = bullet.X - asteroid.X;
				float dy = bullet.Y - asteroid.Y;
				if ((dx * dx) + (dy * dy) > asteroid.Radius * asteroid.Radius)
				{
					continue;
				}
				bullets.RemoveAt(index: bulletIndex);
				asteroids.RemoveAt(index: asteroidIndex);
				score += 10;
				break;
			}
		}
	}

	/// <summary>Processes collisions between the ship and asteroids.</summary>
	/// <remarks>On ship hit the asteroid is removed and one life is lost.</remarks>
	private void ProcessShipCollisions()
	{
		float shipY = glControl.Height - 36f;
		const float shipHitRadius = 17f;
		for (int asteroidIndex = asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
		{
			Asteroid asteroid = asteroids[asteroidIndex];
			float dx = shipX - asteroid.X;
			float dy = shipY - asteroid.Y;
			float radius = shipHitRadius + asteroid.Radius;
			if ((dx * dx) + (dy * dy) > radius * radius)
			{
				continue;
			}
			asteroids.RemoveAt(index: asteroidIndex);
			RemoveLife();
		}
	}

	/// <summary>Removes one life and handles game-over transition.</summary>
	/// <remarks>Stops gameplay updates once no lives remain.</remarks>
	private void RemoveLife()
	{
		lives = Math.Max(val1: 0, val2: lives - 1);
		if (lives > 0)
		{
			return;
		}
		gameOver = true;
		firePressed = false;
		moveLeft = false;
		moveRight = false;
	}

	/// <summary>Resets the game state to start a new round.</summary>
	/// <remarks>Clears active entities and restores score/lives defaults.</remarks>
	private void RestartGame()
	{
		asteroids.Clear();
		bullets.Clear();
		score = 0;
		lives = 3;
		gameOver = false;
		shipX = glControl.Width / 2f;
		fireCooldown = 0f;
		spawnCooldown = 0.3f;
		lastUpdateUtc = DateTime.UtcNow;
	}

	/// <summary>Draws the player ship triangle.</summary>
	/// <remarks>The ship is rendered near the bottom edge of the screen.</remarks>
	private void DrawShip()
	{
		float y = glControl.Height - 30f;
		GL.Color3(0.20f, 0.95f, 1.00f);
		GL.Begin(mode: PrimitiveType.Triangles);
		GL.Vertex2(x: shipX, y: y - 16f);
		GL.Vertex2(x: shipX - 14f, y: y + 12f);
		GL.Vertex2(x: shipX + 14f, y: y + 12f);
		GL.End();
	}

	/// <summary>Draws all active bullets.</summary>
	/// <remarks>Bullets are rendered as small yellow rectangles.</remarks>
	private void DrawBullets()
	{
		GL.Color3(1f, 0.92f, 0.32f);
		foreach (Bullet bullet in bullets)
		{
			GL.Begin(mode: PrimitiveType.Quads);
			GL.Vertex2(x: bullet.X - 2f, y: bullet.Y - 8f);
			GL.Vertex2(x: bullet.X + 2f, y: bullet.Y - 8f);
			GL.Vertex2(x: bullet.X + 2f, y: bullet.Y + 8f);
			GL.Vertex2(x: bullet.X - 2f, y: bullet.Y + 8f);
			GL.End();
		}
	}

	/// <summary>Draws all active asteroids.</summary>
	/// <remarks>Asteroids are rendered as filled circles with orange color.</remarks>
	private void DrawAsteroids()
	{
		GL.Color3(0.95f, 0.55f, 0.20f);
		foreach (Asteroid asteroid in asteroids)
		{
			DrawFilledCircle(centerX: asteroid.X, centerY: asteroid.Y, radius: asteroid.Radius, segments: 18);
		}
	}

	/// <summary>Draws one filled circle using triangle fan geometry.</summary>
	/// <param name="centerX">The circle center x-coordinate.</param>
	/// <param name="centerY">The circle center y-coordinate.</param>
	/// <param name="radius">The circle radius.</param>
	/// <param name="segments">The number of fan segments.</param>
	/// <remarks>Used for asteroid rendering in immediate-mode OpenGL.</remarks>
	private static void DrawFilledCircle(float centerX, float centerY, float radius, int segments)
	{
		GL.Begin(mode: PrimitiveType.TriangleFan);
		GL.Vertex2(x: centerX, y: centerY);
		for (int idx = 0; idx <= segments; idx++)
		{
			double angle = (idx * Math.PI * 2d) / segments;
			float x = centerX + (radius * (float)Math.Cos(d: angle));
			float y = centerY + (radius * (float)Math.Sin(a: angle));
			GL.Vertex2(x: x, y: y);
		}
		GL.End();
	}

	/// <summary>Updates the status label with the current game state.</summary>
	/// <remarks>Shows controls, score, lives, and game-over restart hint.</remarks>
	private void UpdateStatusText()
	{
		labelInformation.Text = gameOver
			? $"Game over! Score: {score} | Press R to restart | Esc to close"
			: $"Score: {score} | Lives: {lives} | Move: ←/→ | Shoot: Space | Esc to close";
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation used in debugger views.</returns>
	/// <remarks>Provides quick state inspection while debugging.</remarks>
	private string GetDebuggerDisplay() => $"Score={score}, Lives={lives}, Asteroids={asteroids.Count}, Bullets={bullets.Count}";
}
