namespace GMTKGameJam2023;

public partial class Grunt : Units
{
    private int hp = 1;
    private int carryweight = 1;
    private int reach = 1;
    protected override float MovementSpeed { get; set; } = 60.0f;
}