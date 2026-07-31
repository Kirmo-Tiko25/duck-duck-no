using Godot;
using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public partial class Chick : CharacterBody2D
{
	// Export variables
	[Export] public float WaterSpeed { get; set; } = 150.0f;
	[Export] public float LandSpeed { get; set; } = 80.0f;
	[Export] public float RotationSpeed { get; set; } = 3.0f;
	[Export] public float FollowDistance = 600f; // starts following
	[Export] public float StopDistance = 50f; // stops near Duck
	[Export] float Smoothing = 0.2f; // how quickly catches up to Duck
	private Duck _player;
	private Vector2 _targetPosition;
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
		_player = GetParent().GetNode<Duck>("Duck");
		if (_player == null)
		{
			GD.PrintErr("Chick: Could not find PlayerDuck!");
		}

		// Connect Signals
		_landDetector.BodyEntered += OnBodyEntered;
		_landDetector.BodyExited += OnBodyExited;

		currentSpeed = WaterSpeed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;

		// calculate desired position
		Vector2 playerPos = _player.Position;
		Vector2 directionToPlayer = (playerPos - Position).Normalized();

		float distaceToPlayer = Position.DistanceTo(playerPos);

		if (distaceToPlayer <= StopDistance)
		{
			// stop or idle
			_targetPosition = Position;
		}

		else if (distaceToPlayer < FollowDistance)
		{
			// start following when close enough
			_targetPosition = playerPos - directionToPlayer * StopDistance;
		}
		else
		{
			// stop or idle
			_targetPosition = Position;
		}

		// movement
		Vector2 velocity = (_targetPosition - Position).Normalized() * currentSpeed;
		//Vector2 smoothedVelocity = Velocity.Lerp(desiredVelocity, Smoothing);

		// randomenss for wandering TODO

		// Move and collide
		//Vector2 motion = velocity * (float)delta;

		// combine player and enviroment velocity
		// Vector2 motion = smoothedVelocity * (float)delta;
		Velocity = velocity + RiverVelocity;

		MoveAndSlide();

		// rotation
		if (velocity.Length() > 0.1f)
		{
			Rotation = velocity.Angle() + MathF.PI / 2f;
		}

		//Animations
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (Velocity.Length() > 0)
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
		else
		{
			animatedSprite2D.Play("default");
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		// Only reacts if entered body is part of land
		if (body.IsInGroup("ground"))
		{
			isOnLand = true;
			currentSpeed = LandSpeed;
			isinRiver = false;
			SetCollisionLayerValue(5, false);
		}
		// Only reacts if entered body is part of river
		else if (body.IsInGroup("river"))
		{
			if (!isOnLand)
			{
				isinRiver = true;
				GD.Print("Duckling in flowing river");
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
			}
		}
	}

}
