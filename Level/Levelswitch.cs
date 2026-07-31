using Godot;
using System;

public partial class Levelswitch : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Duck player)
		{
			GD.Print("Duck in levelswitch area");
			var main = GetNode<Main>("/root/Main");
			main.LoadScene("res://Level/Level2.tscn");

		}
		else if (body is Chick npc)
		{
			// TODO count your chicks
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
