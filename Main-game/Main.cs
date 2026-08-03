using Godot;
using System;
using System.Dynamic;

public partial class Main : Node
{
	[Export] private Node2D LevelRoot { get; set; }
	[Export] private AnimationPlayer Fader { get; set; }
	[Export] private Timer SceneTransitionTimer { get; set; }
	private bool isFading = false;
	private string pendingScenePath = "";

	public int chicks = 0;

	public override void _Ready()
	{
		Fader.Play("FadeIn");
	}
	public void LoadScene(string scenePath)
	{
		if (string.IsNullOrEmpty(scenePath) || isFading)
			return;

		// fadeout
		isFading = true;
		Fader.Play("FadeOut");

		// save scene path
		pendingScenePath = scenePath;
		GD.Print($"Fading out... Loading {scenePath}");

		// start timer
		SceneTransitionTimer.Start();
	}

	private void OnFadeTimeout()
	{
		if (string.IsNullOrEmpty(pendingScenePath))
			return;

		// remove current scene if any
		foreach (Node child in LevelRoot.GetChildren())
			child.QueueFree();

		// Load new Scene
		var newScene = GD.Load<PackedScene>(pendingScenePath);
		Node newInstance = newScene.Instantiate();
		LevelRoot.AddChild(newInstance);

		// Fade in
		Fader.Play("FadeIn");

		isFading = false;
		pendingScenePath = "";
	}

}
