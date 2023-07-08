namespace GMTKGameJam2023
{
    public partial class Survivor : Units.Units
    {
        protected override float HitPoints { get; set; } = 10;
        protected override float CarryWeight { get; set; } = 1;
        protected override float Reach { get; set; } = 5;
        protected override float MovementSpeed { get; set; } = 10.0f;
    }
}