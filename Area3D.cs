using Godot;
using System;

public partial class Area3D : Godot.Area3D
{
	public void detection()
	{
		var detection_aura = GetNode<Area3D>("area_3d.tscn");
		detection_aura.BodyEntered += body =>
		{
			GD.Print("collision");
		};
	}
}
