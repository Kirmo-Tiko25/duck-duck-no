using Godot;
using System;

public partial class LevelSelect : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hook level buttons
		GetNode<Button>("Control/GridContainer/TestH").Pressed += () => LoadLevel(3);
		GetNode<Button>("Control/GridContainer/TestV").Pressed += () => LoadLevel(2);
		GetNode<Button>("Control/GridContainer/Lake").Pressed += () => LoadLevel(1);

		// Back Button
		GetNode<Button>("Control/Back").Pressed += OnBackPressed;
	}


	private void LoadLevel(int levelNumber)
	{
		GD.Print($"Loading Level {levelNumber}....");
		string scenePath = $"res://Level/Level{levelNumber}.tscn";
		var scene = GD.Load<PackedScene>(scenePath);
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.PrintErr($"Level {levelNumber} not found: {scenePath}");
		}
	}
	private void OnBackPressed()
	{
		ChangeScene("res://Main-game/main_menu.tscn");
	}

	private void ChangeScene(string scenePath)
	{
		var scene = GD.Load<PackedScene>(scenePath);
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.PrintErr($"Failed to load scene: {scenePath}");
		}
	}
}
