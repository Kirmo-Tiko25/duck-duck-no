using Godot;
using System;
using System.Collections.Generic;

public partial class FollowerManager : Node
{
	private HashSet<Node> _activeFollowers = new();
	public int ActiveFollowerCount => _activeFollowers.Count;

	[Export] private Label _followerCounterLabel; //Update UI

	public void AddFollower(Node follower)
	{
		if (!_activeFollowers.Contains(follower))
		{
			_activeFollowers.Add(follower);
			UpdateUI();
			GD.Print($"Follower added. Total: {ActiveFollowerCount}");
		}
	}

	public void RemoveFollower(Node follower)
	{
		if (_activeFollowers.Remove(follower))
		{
			UpdateUI();
			GD.Print($"Follower Removed. Total: {ActiveFollowerCount}");
		}
	}

	private void UpdateUI()
	{
		if (_followerCounterLabel != null)
			_followerCounterLabel.Text = $"Ducklings: {_activeFollowers.Count} / 3";
	}
	public int GetActiveFollowerCount()
	{
		return ActiveFollowerCount;
		//return _activeFollowers.Count;
	}
}
