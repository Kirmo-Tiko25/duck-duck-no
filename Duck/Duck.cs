using Godot;
using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
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
	private bool isinRiver = false;
	public Vector2 RiverVelocity = Vector2.Zero;

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
		Rotation += rotationDirection * RotationSpeed * (float)delta;

		// move foreward
		Vector2 _imputVelocity = Vector2.Zero;

		if (Input.IsActionPressed("ui_up"))
		{
			// Vector2.Up rotated by our current rotation (Up default)
			_imputVelocity = Vector2.Up.Rotated(Rotation) * currentSpeed;
		}

		// move back
		if (Input.IsActionPressed("ui_down"))
		{
			// Vector2.Up rotated by our current rotation (Up default)
			_imputVelocity = Vector2.Down.Rotated(Rotation) * currentSpeed / 3;
		}
		// check if in river
		Vector2 riverVelocity = Vector2.Zero;
		if (isinRiver)
		{
			riverVelocity = RiverVelocity;
		}

		// combine player and enviroment velocity
		Velocity = _imputVelocity + RiverVelocity;

		MoveAndSlide();

		//Animations
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (_imputVelocity.Length() > 0)
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
			SetCollisionLayerValue(5, false);
		}
		// Only reacts if entered body is part of river
		else if (body.IsInGroup("river"))
		{
			if (!isOnLand)
			{
				isinRiver = true;
				GD.Print("Duck in flowing river");
			}

		}
		// Only reacts if entered body is part of bridge
		else if (body.IsInGroup("bridge"))
		{
			if (isOnLand)
			{
				SetCollisionLayerValue(7, true);
				GD.Print("Duck on bridge");
			}

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
				SetCollisionLayerValue(5, true);
				GD.Print("Duck left land, back to swimming");
			}
		}
		// IF exited land check if other land is in contact
		else if (body.IsInGroup("river"))
		{
			// Only switch back to water if no land body contact
			var GetOverlappingBodies = _landDetector.GetOverlappingBodies();
			bool stillOnRiver = false;

			foreach (Node2D b in GetOverlappingBodies)
			{
				if (b.IsInGroup("river"))
				{
					stillOnRiver = true;
					break;
				}
			}


			if (!stillOnRiver)
			{
				isinRiver = false;
				currentSpeed = WaterSpeed;
				GD.Print("Duck left river");
			}
		}
		// IF exited bridge
		else if (body.IsInGroup("bridge"))
		{
			if (isOnLand)
			{
				SetCollisionLayerValue(7, false);
				GD.Print("Duck left bridge");
			}
		}
	}

}
