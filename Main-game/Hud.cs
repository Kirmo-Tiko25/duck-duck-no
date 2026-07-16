using Godot;
using System;

public partial class Hud : Control
{
	private MainMenu pauseMenuInstance;

	public override void _Ready()
	{
		GetNode<TouchScreenButton>("PauseButton").Pressed += OnPausePressed;
		pauseMenuInstance = GetNode<MainMenu>("//root/Main/UI/MainMenu");
		pauseMenuInstance.Connect(nameof(MainMenu.ResumeGame), Callable.From(OnResumeGame));
	}

	private void OnPausePressed()
	{
		//if paused ignore
		if (GetTree().Paused) return;

		// "signals"
		pauseMenuInstance.Visible = true;
		pauseMenuInstance.ShowContinue();

		// Pause game
		GetTree().Paused = true;
	}

	private void OnResumeGame()
	{
		GetTree().Paused = false;
	}
}
