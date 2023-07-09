using System.Collections.Generic;
using System.Linq;
using GMTKGameJam2023.Scripts;
using Godot;

namespace GMTKGameJam2023.Units;

public partial class Units : CharacterBody3D
{
    protected virtual float HitPoints { get; set; }
    protected virtual float CarryWeight { get; set; }
    protected virtual float Reach { get; set; }
    protected virtual float Damage { get; set; }
    protected virtual float AttackSpeed { get; set; }
    protected virtual float MovementSpeed { get; set; }
    protected virtual Area3D attackRadiusArea { get; set; }

    protected List<Units> _enemiesInRange { get; set; } = new();
    protected double _swingTimer;
    protected Units _target;

    public bool Selected
    {
        get => _selected;
        set
        {
            var ring = GetNode<MeshInstance3D>("SelectionRing");
            ring.Visible = value;
        }
    }

    protected virtual List<string> EnemyGroups { get; set; }

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private NavigationAgent3D _navigationAgent;
    private bool _selected;

    public Vector3 MovementTarget
    {
        get => _navigationAgent.TargetPosition;
        set => _navigationAgent.TargetPosition = value;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        attackRadiusArea = GetNode<Area3D>("Area3D");
        attackRadiusArea.BodyEntered += OnAttackRadiusEntered;
        attackRadiusArea.BodyExited += AttackRadiusAreaOnBodyExited;
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 0.5f;
        _navigationAgent.TargetDesiredDistance = 0.5f;
        GD.Print($"Spawned at {Position}");
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
    }

    private void AttackRadiusAreaOnBodyExited(Node3D exitedUnit)
    {
        if (exitedUnit != this && IsEnemy(exitedUnit))
        {
            _enemiesInRange.Remove((Units)exitedUnit);
            if (_target == exitedUnit) _target = null;
        }
    }

    protected virtual void OnAttackRadiusEntered(Node3D enteredUnit)
    {
        if (this != enteredUnit && IsEnemy(enteredUnit))
        {
            _enemiesInRange.Add((Units)enteredUnit);
            TryAttack(enteredUnit);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_navigationAgent.IsNavigationFinished()) return;

        var currentAgentPosition = GlobalTransform.Origin;
        var nextPathPosition = _navigationAgent.GetNextPathPosition();

        var newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
        newVelocity *= MovementSpeed;

        Velocity = newVelocity;

        MoveAndSlide();
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _swingTimer += delta;
        if (_enemiesInRange.Count > 0) _target = _enemiesInRange.First();
        if (_swingTimer > AttackSpeed && _target != null) TryAttack(_target);
    }

    public virtual void TryAttack(Node3D enemy)
    {
        var enemyManager = (Units)enemy;
        enemyManager.TakeDamage(Damage);
    }

    public virtual void TakeDamage(float damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
            // GD.Print($"[{Name}] Taking {damage} damage");
            // GD.Print($"Hitpoints below 0: {HitPoints}");
            Die();
    }

    private void Die()
    {
        RemoveChild(this);
        Free();
    }

    private bool IsEnemy(Node target)
    {
        return target.GetGroups().Any(targetsGroup => EnemyGroups.Contains(targetsGroup));
    }
}