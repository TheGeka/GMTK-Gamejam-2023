using Godot;
using System;

public partial class ShapeCast3D : Godot.ShapeCast3D
{
	private PackedScene gruntdetection;
	public void detection()
	{
		var node = GetNode<ShapeCast3D>("res://Units/Grunt/shape_cast_3d.tscn");
		GD.Print(node.GetCollider(0));
	}
}
