using Godot;
using System;
using GMTKGameJam2023;
using GMTKGameJam2023.Scripts;

public partial class Game : Node3D
{
	private enum Controls
	{
		PauseGame
	}
	
	private PackedScene _gruntscene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gruntscene = GD.Load<PackedScene>("res://Units/Grunt/grunt.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsAction(Controls.PauseGame.ToString()))
		{
			GetTree().Paused = true;
			GetNode<Control>("PauseMenu").Show();
			

		}
		if (@event is InputEventMouseButton mouseButtonEvent)
			if (mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.Pressed)
			{
				var camera = GetTree().Root.GetCamera3D();
				var intersection = camera.CastRay(GetWorld3D().DirectSpaceState, mouseButtonEvent.Position);
				PlaceGrunt(intersection["position"].AsVector3());
			}
	}
	private void PlaceGrunt(Vector3 Location)
	{
		
		var newgrunt = _gruntscene.Instantiate<Grunt>();
		newgrunt.Position = Location;
		AddChild(newgrunt);
	}
}
