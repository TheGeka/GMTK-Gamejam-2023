using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GMTKGameJam2023;

public partial class Survivor : Units.Units
{
	protected override float HitPoints { get; set; } = 100;
	protected override float CarryWeight { get; set; } = 1;
	protected override float AttackSpeed { get; set; } = 2;
	protected override float Damage { get; set; } = 3;
	protected override float Reach { get; set; } = 1;
	protected override float MovementSpeed { get; set; } = 10.0f;
	protected override List<string> EnemyGroups { get; set; } = new() { "Horde" };

	public override void _Process(double delta)
	{
		_swingTimer += delta;
		if (_enemiesInRange.Count > 0) _target = _enemiesInRange.First();
		if (_swingTimer > AttackSpeed && _target != null)
		{
			if (_target.GlobalPosition.DistanceTo(GlobalPosition) >= 0.1)
				MovementTarget = GlobalPosition - -(GlobalPosition - _target.GlobalPosition);

			TryAttack(_target);
		}
	}

	public override void _Input(InputEvent @event)
	{
		return;
		base._Input(@event);
	}
}
