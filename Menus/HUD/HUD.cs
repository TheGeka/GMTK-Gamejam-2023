using Godot;
using System;

public partial class HUD : CanvasLayer
{
	private Label ResourceLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResourceLabel = GetNode<Label>("ResourceLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateResources(int resource)
	{
		ResourceLabel.Text = $"{resource} Resource";
	}
}
