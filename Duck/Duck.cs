using Godot;
using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Duck : CharacterBody2D
{
	// Export variables
	[Export] public float WaterSpeed { get; set; } = 200.0f;
	[Export] public float LandSpeed { get; set; } = 100.0f;
	[Export] public float RotationSpeed { get; set; } = 3.0f;
	private float currentSpeed;

	// Area2D detector
	private Area2D _landDetector;
	private bool isOnLand = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_landDetector = GetNode<Area2D>("Area2D");

		// Connect Signals
		_landDetector.BodyEntered += OnBodyEntered;
		_landDetector.BodyExited += OnBodyExited;

		currentSpeed = WaterSpeed;
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
			velocity = Vector2.Up.Rotated(Rotation) * currentSpeed;
		}

		// move back
		if (Input.IsActionPressed("ui_down"))
		{
			// Vector2.Up rotated by our current rotation (Up default)
			velocity = Vector2.Down.Rotated(Rotation) * currentSpeed / 3;
		}
		// apply velcoity to move
		Velocity = velocity;
		MoveAndSlide();

		//Animations
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length() > 0)
		{
			if (isOnLand)
			{
				animatedSprite2D.Play("walk");
			}
			else
			{
				animatedSprite2D.Play("waddle");
			}

		}
		else if (rotationDirection > 0)
		{
			animatedSprite2D.Play("right");
		}
		else if (rotationDirection < 0)
		{
			animatedSprite2D.Play("left");
		}
		else
		{
			animatedSprite2D.Play("default");
		}

		// keep player in screen
		/*
		Vector2 playerPos = Position;
		Vector2 screenMin = GetParent<Node>().GetNode<Camera2D>("MovingCamera").GetScreenCenterPosition() - (GetViewportRect().Size / 2);
		Vector2 screenMax = screenMin + GetViewportRect().Size;

		// Keeps player inside the visible screen bounds
		playerPos.Y = Mathf.Clamp(playerPos.Y, screenMin.Y, screenMax.Y);
		playerPos.X = Mathf.Clamp(playerPos.X, screenMin.X, screenMax.X);
		Position = playerPos; */
	}

	private void OnBodyEntered(Node2D body)
	{
		// Only reacts if entered body is part of land
		if (body.IsInGroup("ground"))
		{
			isOnLand = true;
			currentSpeed = LandSpeed;
			GD.Print("Duck Landed");
		}
	}

	private void OnBodyExited(Node2D body)
	{
		// IF exited land check if other land is in contact
		if (body.IsInGroup("ground"))
		{
			// Only switch back to water if no land body contact
			var GetOverlappingBodies = _landDetector.GetOverlappingBodies();
			bool stillOnGround = false;

			foreach (Node2D b in GetOverlappingBodies)
			{
				if (b.IsInGroup("ground"))
				{
					stillOnGround = true;
					break;
				}
			}


			if (!stillOnGround)
			{
				isOnLand = false;
				currentSpeed = WaterSpeed;
				GD.Print("Duck left land, back to swimming");
			}
		}
	}

	// method that checks if duck is in water
	public bool IsInWater()
	{
		return !isOnLand;
	}
}
