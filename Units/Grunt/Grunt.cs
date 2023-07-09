using System.Collections.Generic;
using System.Linq;

namespace GMTKGameJam2023;

public partial class Grunt : Units.Units
{
    public static int Cost { get; set; } = 50;
    protected override float HitPoints { get; set; } = 20;
    protected override float CarryWeight { get; set; } = 1;
    protected override float Damage { get; set; } = 5;
    protected override float AttackSpeed { get; set; } = 1;
    protected override float Reach { get; set; } = 1;
    protected override float MovementSpeed { get; set; } = 10.0f;
    protected override List<string> EnemyGroups { get; set; } = new() { "Protagonists" };

    public override void _Process(double delta)
    {
        _swingTimer += delta;
        if (_enemiesInRange.Count > 0) _target = _enemiesInRange.First();
        if (_swingTimer > AttackSpeed && _target != null)
        {
            MovementTarget = _target.Position;
            TryAttack(_target);
        }
    }
}