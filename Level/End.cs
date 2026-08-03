using Godot;
using System;
using System.Buffers;

public partial class End : Node2D
{
	private Label _followerCountLabel;
	private FollowerManager _manager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_manager = GetNode<FollowerManager>("/root/Main/FollowerManager");
		int count = _manager.GetActiveFollowerCount();
		_followerCountLabel = GetNode<Label>("FollowerCountLabel");
		_followerCountLabel.Text = $" Cheers you got {count} ducklings.";
		// quit Button
		GetNode<Button>("Control/Quit").Pressed += OnQuitPressed;
	}

	private void OnQuitPressed()
	{
		GD.Print("Quitting game");
		GetTree().Quit();
	}
}
