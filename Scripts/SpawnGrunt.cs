using Godot;

namespace GMTKGameJam2023.Scripts;

public partial class SpawnGrunt : Node3D
{
    private PackedScene gruntscene;

    private void PlaceGrunt(Vector3 Location)
    {
        gruntscene = GD.Load<PackedScene>("res://grunt.tscn");
        var newgrunt = gruntscene.Instantiate<Grunt>();
        newgrunt.Position = Location;
        AddChild(newgrunt);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton mouseButtonEvent)
            if (mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.Pressed)
            {
                var camera = GetTree().Root.GetCamera3D();
                var intersection = camera.CastRay(GetWorld3D().DirectSpaceState, mouseButtonEvent.Position);
                PlaceGrunt(intersection["position"].AsVector3());
            }
    }
}