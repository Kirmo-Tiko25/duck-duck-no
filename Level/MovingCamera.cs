using Godot;
using System;

public partial class MovingCamera : Camera2D
{
	private Boolean _canMove = false;
	[Export] public float ScrollSpeed { get; set; } = 100.0f; // pix per sec
	[Export] public Boolean AutoScroll = true;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		GD.Print("Game started.");
		if (AutoScroll)
		{
			StartMovementTimer();
		}
	}

	private async void StartMovementTimer()
	{
		GD.Print("Waiting for 5 seconds...");
		// Create a one-shot timer for 5 seconds and wait for its 'timeout' signal
		await ToSignal(GetTree().CreateTimer(5.0), SceneTreeTimer.SignalName.Timeout);

		// Activate movement
		_canMove = true;
	}

	private void TriggerDelayedAction()
	{
		throw new NotImplementedException();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_canMove) return;

		// Moving up
		Vector2 position = Position;
		position.Y -= ScrollSpeed * (float)delta;
		Position = position;
	}
}
