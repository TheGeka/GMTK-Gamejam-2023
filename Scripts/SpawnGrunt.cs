using Godot;

namespace GMTKGameJam2023.Scripts;

public partial class SpawnGrunt : Node3D
{
	private PackedScene gruntscene;

	private void PlaceGrunt(Vector3 Location)
	{
		gruntscene = GD.Load<PackedScene>("res://Units/Grunt/grunt.tscn");
		var newgrunt = gruntscene.Instantiate<Units.Grunt.Grunt>();
		newgrunt.Position = Location;
		AddChild(newgrunt);
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
    }
}
