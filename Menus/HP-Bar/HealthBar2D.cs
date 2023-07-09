using Godot;
using System;

public partial class HealthBar2D : TextureProgressBar
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var p = GetParent().GetParent<Sprite3D>();
		p.Texture = GetViewport().GetTexture();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateHpBar(float value)
	{
		if (value < MaxValue * 0.7)
		{
			TextureProgress = GD.Load<Texture2D>("res://Resources/Textures/HP-Bar/barHorizontal_yellow_mid 200.png");
		}

		if (value < MaxValue * 0.35)
		{
			TextureProgress = GD.Load<Texture2D>("res://Resources/Textures/HP-Bar/barHorizontal_red_mid 200.png");
		}

		if (value <= 0)
		{
			Visible = false;
		}

		Value = value;
	}
}
