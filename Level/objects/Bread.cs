using Godot;
using System;

public partial class Bread : Area2D
{
	private Sprite2D _sprite;
	private CollisionShape2D _collisionShape;
	private GpuParticles2D _crumbParticles;

	public override void _Ready()
	{
		//Get reference of child nodes
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_crumbParticles = GetNode<GpuParticles2D>("CrumbParticles");

		// collision signal
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Check if collision with duck
		if (body.IsInGroup("duck"))
		{
			GetEaten();
		}
	}

	private void GetEaten()
	{
		// Hide collision, and bread
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		_sprite.Visible = false;

		// Bread crumb particles
		_crumbParticles.Emitting = true;

		// after particles finished delete the scene
		SceneTreeTimer timer = GetTree().CreateTimer(_crumbParticles.Lifetime);
		timer.Timeout += QueueFree;
	}

}
