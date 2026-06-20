using Godot;
using System;

public partial class Duck : CharacterBody2D
{
	// Export variables
	[Export] public float MoveSpeed { get; set; } = 200.0f;
	[Export] public float RotationSpeed { get; set; } = 3.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float floatDelta = (float)delta;

		// Rotation
		float rotationDirection = 0.0f;
		if (Input.IsActionPressed("ui_right"))
		{
			rotationDirection += 1.0f;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			rotationDirection -= 1.0f;
		}

		// overtime rotate
		Rotation += rotationDirection * RotationSpeed * floatDelta;

		// move foreward
		Vector2 velocity = Vector2.Zero;
		if (Input.IsActionPressed("ui_up"))
		{
			// Vector2.Up rotated by our current rotation (Up default)
			velocity = Vector2.Up.Rotated(Rotation) * MoveSpeed;
		}

		// move back
		if (Input.IsActionPressed("ui_down"))
		{
			// Vector2.Up rotated by our current rotation (Up default)
			velocity = Vector2.Down.Rotated(Rotation) * MoveSpeed / 3;
		}
		// apply velcoity to move
		Velocity = velocity;
		MoveAndSlide();

		// keep player in screen
		Vector2 playerPos = Position;
		Vector2 screenMin = GetParent<Node>().GetNode<Camera2D>("MovingCamera").GetScreenCenterPosition() - (GetViewportRect().Size / 2);
		Vector2 screenMax = screenMin + GetViewportRect().Size;

		// Keep player inside the visible screen bounds
		playerPos.Y = Mathf.Clamp(playerPos.Y, screenMin.Y, screenMax.Y);
		playerPos.X = Mathf.Clamp(playerPos.X, screenMin.X, screenMax.X);
		Position = playerPos;
	}
}
