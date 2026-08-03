using Godot;
using System;

public partial class EndSwitch : Area2D
{
	int endCount;
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
			var followerManager = GetNode<FollowerManager>("/root/Main/FollowerManager");
			endCount = followerManager.GetActiveFollowerCount();
			main.chicks = endCount;
			main.LoadScene("res://Level/end.tscn");
		}
	}
}
