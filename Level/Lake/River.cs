using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class River : Area2D
{
	[Export] public float FlowSpeed = 150f; // speed in pixel per second
	[Export] public Vector2 FlowVelocity;
	//private HashSet<Node2D> _duckInRiver = new(); // storing ducks in river

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		// Get the rivers local forward direction
		Vector2 riverDirection = Transform.X;

		// Apply velocity in the river's local direction
		Vector2 targetVelocity = riverDirection * FlowSpeed;

		// smoothly interpolate current velocity to target
		FlowVelocity = targetVelocity;
	}

	private void OnBodyEntered(Node body)
	{
		// Only reacts if duck
		//if (body.IsInGroup("duck"))
		if (body is Duck player)
		{
			//_duckInRiver.Add(body as Node2D);
			GD.Print("Duck in river");
			player.RiverVelocity += FlowVelocity;

		}
		else if (body is Chick npc)
		{
			npc.RiverVelocity += FlowVelocity;
		}
	}

	private void OnBodyExited(Node body)
	{
		// Only reacts if duck
		if (body is Duck player)
		{
			//_duckInRiver.Add(body as Node2D);
			GD.Print("Duck out of river");
			player.RiverVelocity -= FlowVelocity;
		}
		if (body is Chick npc)
		{
			//_duckInRiver.Add(body as Node2D);
			GD.Print("chick out of river");
			npc.RiverVelocity -= FlowVelocity;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{/*
		foreach (Node2D duck in _duckInRiver)
		{


			if (duck is not CharacterBody2D character) continue;

			// Get the rivers local forward direction
			Vector2 riverDirection = Transform.X;

			// Apply velocity in the river's local direction
			Vector2 targetVelocity = riverDirection * FlowSpeed;

			// smoothly interpolate current velocity to target
			character.Velocity = character.Velocity.Lerp(targetVelocity, (float)delta * 6.0f);

		}*/
	}
}
