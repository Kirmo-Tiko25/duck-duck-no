using Godot;
using System;

public partial class WaterBackground : Node2D
{
	[Export] private float ScrollSpeed = 0.5f; // water flow speed
	[Export] private float WaveAnimationSpeed; // wave pulse/roation speed
	[Export] private float MaxWaveOpacity = 0.7f; // test how strongly draw waves

	private TileMapLayer _waterBase;
	private TileMapLayer _waves;

	private Vector2 _baseOffset = Vector2.Zero;
	private float _waveRotation = 0f;
	private float _waveOpacity = 0f;
	private float _wavePhase = 0f; //for smooth pulses

	public override void _Ready()
	{
		_waterBase = GetNode<TileMapLayer>("WaterBase");
		_waves = GetNode<TileMapLayer>("Waves");
	}

	public override void _Process(double delta)
	{
		// Scroll base water if needed
		_baseOffset += new Vector2(ScrollSpeed * (float)delta, ScrollSpeed * (float)delta);
		//_waterBase.Offset = _baseOffset;

		// Animate wave tile: rotation + opacity pulsing
		_wavePhase += WaveAnimationSpeed * (float)delta;
		_waveRotation = Mathf.Sin(_wavePhase) * 15f; // small oscillation in rotation (15)
		_waveOpacity = (Mathf.Sin(_wavePhase) + 1f) / 2f * MaxWaveOpacity; // Smooth sine wave from 0 to MAx

		//_waves.Rotation = _waveRotation * Mathf.DegToRad;
		_waves.Modulate = new Color(1, 1, 1, _waveOpacity); // apply opacity
	}
}
