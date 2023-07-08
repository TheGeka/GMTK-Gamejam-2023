using Godot;
using System;
using System.Timers;
using GMTKGameJam2023;
using GMTKGameJam2023.Scripts;
using Timer = System.Timers.Timer;

public partial class Game : Node3D
{
	internal Timer _turnTimer = new Timer()
	{
		Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
		Enabled = true,
		AutoReset = false
	};

	internal bool TurnAvailable = true;
	
	private enum Controls
	{
		PauseGame
	}
	
	private PackedScene _gruntscene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gruntscene = GD.Load<PackedScene>("res://Units/Grunt/grunt.tscn");
		_turnTimer.Elapsed += (sender, args) =>
		{
			TurnAvailable = true;
			GD.Print("Timer elapsed");
		};
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		var control = GetNode<Control>("PauseMenu");
		if (@event.IsPauseEvent() && !GetTree().Paused && TurnAvailable)
		{
			TurnAvailable = false;
			GetTree().Paused = true;
			control.Show();
			

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
