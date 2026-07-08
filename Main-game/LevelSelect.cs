using Godot;
using System;

public partial class LevelSelect : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hook level buttons
		GetNode<Button>("GridContainer/TestH").Pressed += () => LoadLevel(1);
		GetNode<Button>("GridContainer/TestV").Pressed += () => LoadLevel(2);
		GetNode<Button>("GridContainer/Lake").Pressed += () => LoadLevel(3);

		// Back Button
		GetNode<Button>("Back").Pressed += OnBackPressed;
	}


	private void LoadLevel(int levelNumber)
	{
		GD.Print($"Loading Level {levelNumber}....");
		string scenePath = $"res://Scenes/Levels/Level{levelNumber}.tscn";
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
		ChangeScene("rest://Scenes/Menu/MainMenu.tscn");
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
