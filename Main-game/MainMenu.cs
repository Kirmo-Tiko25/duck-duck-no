using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Signal] public delegate void ResumeGameEventHandler();
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/Continue").Visible = false;
		GetNode<HBoxContainer>("VBoxContainer/LevelSelect").Visible = false;
		GetNode<Button>("VBoxContainer/Continue").Pressed += OnContinuePressed;
		GetNode<Button>("VBoxContainer/Level").Pressed += OnLevelPressed;
		GetNode<Button>("VBoxContainer/Start").Pressed += OnStartPressed;
		GetNode<Button>("VBoxContainer/Quit").Pressed += OnQuitPressed;

		// Hook level buttons
		GetNode<Button>("VBoxContainer/LevelSelect/Lake").Pressed += () => LoadLevel(1);
		GetNode<Button>("VBoxContainer/LevelSelect/River").Pressed += () => LoadLevel(2);
		GetNode<Button>("VBoxContainer/LevelSelect/Test_V").Pressed += () => LoadLevel(3);
	}

	private void OnLevelPressed()
	{
		GetNode<HBoxContainer>("VBoxContainer/LevelSelect").Visible = true;
	}

	public void ShowContinue()
	{
		GetNode<Button>("VBoxContainer/Continue").Visible = true;
	}
	private void OnContinuePressed()
	{
		GD.Print("Continue");
		// Emits signal to resume
		EmitSignal(nameof(ResumeGame));
		this.Visible = false;
	}

	private void LoadLevel(int levelNumber)
	{
		GD.Print($"Loading Level {levelNumber}....");
		// load level as child of LEvelRoot
		var LevelRoot = GetNode<Node2D>("//root/Main/Level/LevelRoot");
		string scenePath = $"res://Level/Level{levelNumber}.tscn";
		var scene = GD.Load<PackedScene>(scenePath).Instantiate<Node2D>();
		if (scene != null)
		{
			LevelRoot.AddChild(scene);
		}
		else
		{
			GD.PrintErr($"Level {levelNumber} not found: {scenePath}");
		}
		// Hide menu
		this.Visible = false;

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

	private void OnStartPressed()
	{
		GD.Print("Have Fun!");
		LoadLevel(1);

	}

	private void OnQuitPressed()
	{
		GD.Print("Quitting game");
		GetTree().Quit();
	}

}
