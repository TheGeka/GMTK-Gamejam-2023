namespace GMTKGameJam2023;

public partial class Grunt : Units.Units
{
	protected override float HitPoints { get; set; } = 1;
	protected override float CarryWeight { get; set; } = 1;
	protected override float Reach { get; set; } = 1;
	protected override float MovementSpeed { get; set; } = 10.0f;
}
