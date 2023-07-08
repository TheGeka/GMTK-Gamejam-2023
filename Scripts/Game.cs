using Godot;
using System;
using System.Timers;
using GMTKGameJam2023;
using GMTKGameJam2023.Scripts;
using GMTKGameJam2023.Scripts.Enums;
using GMTKGameJam2023.Units;
using Grunt = GMTKGameJam2023.Units.Grunt.Grunt;
using Timer = System.Timers.Timer;

public partial class Game : Node3D
{
	internal readonly Timer TurnTimer = new Timer()
	{
		Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
		Enabled = true,
		AutoReset = false
	};

	private bool _turnAvailable = true;

	internal SelectableUnits _SelectedUnit;
	Node ChildContainer;
	
	
	
	private PackedScene _gruntscene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gruntscene = GD.Load<PackedScene>("res://Units/Grunt/grunt.tscn");
		ChildContainer = GetNode("UnitContainer");
		TurnTimer.Elapsed += (sender, args) =>
		{
			_turnAvailable = true;
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
		if (@event.IsPauseEvent() && !GetTree().Paused && _turnAvailable)
		{
			_turnAvailable = false;
			ChildContainer.GetTree().Paused = true;
			control.Show();
			

		}
		if (@event is InputEventMouseButton mouseButtonEvent)
			if (mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.Pressed && GetTree().Paused)
			{
				var camera = GetTree().Root.GetCamera3D();
				var intersection = camera.CastRay(GetWorld3D().DirectSpaceState, mouseButtonEvent.Position);
				switch (_SelectedUnit)
				{
					case SelectableUnits.Grunt:
						SpawnGrunt(intersection["position"].AsVector3());
						break;
					case SelectableUnits.Unit2:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
	}
	private void SpawnGrunt(Vector3 Location)
	{
		var newgrunt = _gruntscene.Instantiate<Grunt>();
		newgrunt.Position = Location;
		ChildContainer.AddChild(newgrunt);
	}
}
