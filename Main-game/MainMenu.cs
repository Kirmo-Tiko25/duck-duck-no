using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Export] private Control _mainContainer; //VBoxContainer

	public override void _Ready()
	{
		GetNode<Button>("Play").Pressed += OnPlayPressed;
		GetNode<Button>("Options").Pressed += OnOptionsPressed;
		GetNode<Button>("Quit").Pressed += OnQuitPressed;
	}

	private void OnPlayPressed()
	{
		GD.Print("Switching to Level Select");
		ChangeScene("res://Levels/Level_select.tscn");

	}
	private void OnOptionsPressed()
	{
		GD.Print("Switching to Options menu");
		ChangeScene("res://Main-game/Options.tscn");
	}
	private void OnQuitPressed()
	{
		GD.Print("Quitting game");
		GetTree().Quit();
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
