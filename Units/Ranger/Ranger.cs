namespace GMTKGameJam2023
{

    public partial class Ranger : Units.Units
    {
        protected override float HitPoints { get; set; } = 1;
        protected override float CarryWeight { get; set; } = 0;
        protected override float Reach { get; set; } = 10;
        protected override float MovementSpeed { get; set; } = 10.0f;
    }
}
